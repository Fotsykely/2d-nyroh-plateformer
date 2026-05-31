using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] private AttackData data;
    [SerializeField] private Collider2D hitbox;

    private bool _isAttacking;
    private float _attackTimer;

    void Update()
    {
        if (_attackTimer > 0f)
        {
            _attackTimer -= Time.deltaTime;
            if (_attackTimer <= 0f)
            {
                hitbox.enabled = false;
                _isAttacking = false;
            }
        }
    }

    public void Attack()
    {
        if (_isAttacking) return;
        _isAttacking = true;
        hitbox.enabled = true;
        _attackTimer = data.attackDuration;
    }
}
