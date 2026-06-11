using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, InputSystem_Actions.IPlayerActions, IKnockbackable
{
    [SerializeField] private CharacterData data;
    [SerializeField] private AttackController _attackController;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private InputSystem_Actions _input;
    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    private float _coyoteTimer;
    private float _jumpBufferTimer;
    private bool _jumpQueued;
    private bool _jumpHeld;
    private bool _grounded;
    private float _hitstunTimer;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _input = new InputSystem_Actions();
        _input.Player.SetCallbacks(this);
    }

    void OnEnable() => _input.Player.Enable();
    void OnDisable() => _input.Player.Disable();

    void Update()
    {
        _grounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        _coyoteTimer = _grounded ? data.coyoteTime : _coyoteTimer - Time.deltaTime;
        _jumpBufferTimer -= Time.deltaTime;

        if (_jumpBufferTimer > 0f && _coyoteTimer > 0f)
        {
            _jumpQueued = true;
            _jumpBufferTimer = 0f;
            _coyoteTimer = 0f;
        }
    }

    void FixedUpdate()
    {
        _hitstunTimer -= Time.fixedDeltaTime;
        bool inHitstun = _hitstunTimer > 0f;

        if (!inHitstun) TryJump();
        ApplyGravity();
        if (!inHitstun) Move();
        CapFallSpeed();
    }

    private void TryJump()
    {
        if (!_jumpQueued) return;
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, data.jumpForce);
        _jumpQueued = false;
    }

    // Gravité variable + apex hang
    private void ApplyGravity()
    {
        float gravity = Physics2D.gravity.y;
        float multiplier;
        if (_rb.linearVelocity.y < 0f)
            multiplier = data.fallGravityMultiplier;          // chute
        else if (_rb.linearVelocity.y > 0f && !_jumpHeld)
            multiplier = data.lowJumpMultiplier;              // jump-cut
        else
            multiplier = data.riseGravityMultiplier;          // montée bouton tenu

        bool nearApex = Mathf.Abs(_rb.linearVelocity.y) < data.apexThreshold;
        if (nearApex && _jumpHeld) multiplier *= data.apexGravityScale; // flottement au sommet

        _rb.linearVelocity += Vector2.up * (gravity * (multiplier - 1f) * Time.fixedDeltaTime);
    }

    // Mouvement horizontal avec accel/décel
    private void Move()
    {
        float targetSpeed = _moveInput.x * data.moveSpeed;
        bool accelerating = Mathf.Abs(targetSpeed) > 0.01f;
        float rate = accelerating
            ? (_grounded ? data.groundAccel : data.airAccel)
            : (_grounded ? data.groundDecel : data.airDecel);
        float newX = Mathf.MoveTowards(_rb.linearVelocity.x, targetSpeed, rate * Time.fixedDeltaTime);
        _rb.linearVelocity = new Vector2(newX, _rb.linearVelocity.y);
    }

    private void CapFallSpeed()
    {
        if (_rb.linearVelocity.y < -data.maxFallSpeed)
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, -data.maxFallSpeed);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>();
        if (_moveInput.x != 0f)
            transform.localScale = new Vector3(Mathf.Sign(_moveInput.x), 1f, 1f);
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) { _jumpBufferTimer = data.jumpBufferTime; _jumpHeld = true; }
        if (ctx.canceled) _jumpHeld = false;
    }

    public void ApplyKnockback(Vector2 sourcePosition, Vector2 force, float hitstun)
    {
        float dirX = Mathf.Sign(transform.position.x - sourcePosition.x);
        _rb.linearVelocity = new Vector2(dirX * force.x, force.y);
        _hitstunTimer = hitstun;
    }

    // Stubs — implemented by future components (PlayerCombat, etc.)
    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.started) _attackController.Attack();
    }
    public void OnInteract(InputAction.CallbackContext ctx) { }
    public void OnCrouch(InputAction.CallbackContext ctx) { }
    public void OnSprint(InputAction.CallbackContext ctx) { }
    public void OnLook(InputAction.CallbackContext ctx) { }
    public void OnPrevious(InputAction.CallbackContext ctx) { }
    public void OnNext(InputAction.CallbackContext ctx) { }
}
