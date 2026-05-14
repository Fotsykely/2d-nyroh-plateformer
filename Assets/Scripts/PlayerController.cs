using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    [SerializeField] private CharacterData data;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private InputSystem_Actions _input;
    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    private float _coyoteTimer;
    private float _jumpBufferTimer;
    private bool _jumpQueued;
    private bool _jumpHeld;

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
        bool grounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        _coyoteTimer = grounded ? data.coyoteTime : _coyoteTimer - Time.deltaTime;
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
        if (_jumpQueued)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, data.jumpForce);
            _jumpQueued = false;
        }
        _rb.linearVelocity = new Vector2(_moveInput.x * data.moveSpeed, _rb.linearVelocity.y);

        if (_rb.linearVelocity.y < 0f)
            _rb.linearVelocity += Vector2.up * (Physics2D.gravity.y * (data.fallGravityMultiplier - 1f) * Time.fixedDeltaTime);

        if (_rb.linearVelocity.y > 0f && !_jumpHeld)
            _rb.linearVelocity += Vector2.up * (Physics2D.gravity.y * (data.lowJumpMultiplier - 1f) * Time.fixedDeltaTime);

        if (_rb.linearVelocity.y < -data.maxFallSpeed)
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, -data.maxFallSpeed);
    }

    public void OnMove(InputAction.CallbackContext ctx) => _moveInput = ctx.ReadValue<Vector2>();

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) { _jumpBufferTimer = data.jumpBufferTime; _jumpHeld = true; }
        if (ctx.canceled) _jumpHeld = false;
    }

    // Stubs — implemented by future components (PlayerCombat, etc.)
    public void OnAttack(InputAction.CallbackContext ctx) { }
    public void OnInteract(InputAction.CallbackContext ctx) { }
    public void OnCrouch(InputAction.CallbackContext ctx) { }
    public void OnSprint(InputAction.CallbackContext ctx) { }
    public void OnLook(InputAction.CallbackContext ctx) { }
    public void OnPrevious(InputAction.CallbackContext ctx) { }
    public void OnNext(InputAction.CallbackContext ctx) { }
}
