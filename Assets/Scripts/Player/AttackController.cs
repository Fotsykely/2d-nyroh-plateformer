using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] private AttackData[] attacks;
    [SerializeField] private BoxCollider2D hitbox;

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

    public void Attack(int index = 0)
    {
        if (attacks == null || index < 0 || index >= attacks.Length) return;
        Attack(attacks[index]);
    }

    public void Attack(AttackData data)
    {
        if (_isAttacking || data == null) return;
        _isAttacking = true;

        hitbox.offset = data.hitboxOffset;
        hitbox.size = data.hitboxSize;
        hitbox.enabled = true;
        _attackTimer = data.attackDuration;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (attacks == null) return;
        foreach (var data in attacks)
        {
            if (data == null) continue;
            Vector3 center = transform.position + new Vector3(data.hitboxOffset.x, data.hitboxOffset.y);
            Vector3 size = new(data.hitboxSize.x, data.hitboxSize.y, 0.01f);

            Gizmos.color = data.gizmoColor;
            Gizmos.DrawCube(center, size);

            Gizmos.color = new Color(data.gizmoColor.r, data.gizmoColor.g, data.gizmoColor.b, 1f);
            Gizmos.DrawWireCube(center, size);
        }
    }
#endif
}
