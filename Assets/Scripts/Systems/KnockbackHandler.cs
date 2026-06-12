using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class KnockbackHandler : MonoBehaviour, IKnockbackable
{
    private Rigidbody2D _rb;
    private float _hitstunTimer;

    public bool IsInHitstun => _hitstunTimer > 0f;

    void Awake() => _rb = GetComponent<Rigidbody2D>();
    void FixedUpdate() => _hitstunTimer -= Time.fixedDeltaTime;

    public void ApplyKnockback(Vector2 sourcePosition, Vector2 force, float hitstun)
    {
        float dirX = Mathf.Sign(transform.position.x - sourcePosition.x);
        _rb.linearVelocity = new Vector2(dirX * force.x, force.y);
        _hitstunTimer = hitstun;
    }
}
