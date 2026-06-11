using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PatrolEnemyController : MonoBehaviour
{
    [SerializeField] private PatrolEnemyData data;
    [SerializeField] private DamageDealer contactDamageDealer;
    [SerializeField] private Transform edgeCheck;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D _rb;
    private float _direction = 1f;
    private float _spawnX;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spawnX = transform.position.x;

        if (data.contactAttack != null)
        {
            contactDamageDealer.Damage = data.contactAttack.damage;
            contactDamageDealer.KnockbackForce = data.contactAttack.knockbackForce;
            contactDamageDealer.HitstunDuration = data.contactAttack.hitstunDuration;
        }
    }

    void FixedUpdate() => Patrol();

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
