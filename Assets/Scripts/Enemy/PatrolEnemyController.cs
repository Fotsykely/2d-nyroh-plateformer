using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PatrolEnemyController : MonoBehaviour
{
    [SerializeField] private PatrolEnemyData data;
    [SerializeField] private AttackData contactAttack;
    [SerializeField] private DamageDealer contactDamageDealer;
    [SerializeField] private KnockbackHandler _knockbackHandler;
    [SerializeField] private Transform edgeCheck;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D _rb;
    private float _direction = 1f;
    private float _spawnX;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spawnX = transform.position.x;

        if (contactAttack != null)
        {
            contactDamageDealer.Damage = contactAttack.damage;
            contactDamageDealer.KnockbackForce = contactAttack.knockbackForce;
            contactDamageDealer.HitstunDuration = contactAttack.hitstunDuration;
        }
    }

    void FixedUpdate()
    {
        if (_knockbackHandler != null && _knockbackHandler.IsInHitstun) return;
        Patrol();
    }

    private void Patrol()
    {
        bool groundAhead = Physics2D.Raycast(edgeCheck.position, Vector2.down, 0.5f, groundLayer);
        bool outsideRange = Mathf.Abs(transform.position.x - _spawnX) >= data.patrolRange;

        if (!groundAhead || outsideRange)
            Flip();

        _rb.linearVelocity = new Vector2(_direction * data.moveSpeed, _rb.linearVelocity.y);
    }

    private void Flip()
    {
        _direction *= -1f;
        transform.localScale = new Vector3(_direction, 1f, 1f);
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (edgeCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(edgeCheck.position, edgeCheck.position + Vector3.down * 0.5f);
    }
#endif
}
