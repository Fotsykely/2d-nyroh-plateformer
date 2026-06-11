using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] private AttackData[] attacks;
    [SerializeField] private BoxCollider2D boxHitbox;
    [SerializeField] private CircleCollider2D circleHitbox;
    [SerializeField] private DamageDealer damageDealer;

    private bool _isAttacking;
    private float _attackTimer;

    void Update()
    {
        if (_attackTimer > 0f)
        {
            _attackTimer -= Time.deltaTime;
            if (_attackTimer <= 0f)
            {
                DisableHitboxes();
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

        if (damageDealer != null)
        {
            damageDealer.Damage = data.damage;
            damageDealer.KnockbackForce = data.knockbackForce;
            damageDealer.HitstunDuration = data.hitstunDuration;
        }
        DisableHitboxes();

        if (data.shape == HitboxShape.Box)
        {
            boxHitbox.offset = data.hitboxOffset;
            boxHitbox.size = data.hitboxSize;
            boxHitbox.transform.localRotation = Quaternion.Euler(0f, 0f, data.hitboxRotation);
            boxHitbox.enabled = true;
        }
        else
        {
            circleHitbox.offset = data.hitboxOffset;
            circleHitbox.radius = data.hitboxRadius;
            circleHitbox.enabled = true;
        }

        _attackTimer = data.attackDuration;
    }

    private void DisableHitboxes()
    {
        boxHitbox.enabled = false;
        circleHitbox.enabled = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (attacks == null) return;
        foreach (var data in attacks)
        {
            if (data == null) continue;

            Gizmos.color = data.gizmoColor;
            Color outline = new(data.gizmoColor.r, data.gizmoColor.g, data.gizmoColor.b, 1f);

            float scaleX = transform.lossyScale.x < 0f ? -1f : 1f;
            Vector3 origin = boxHitbox != null ? boxHitbox.transform.position : transform.position;

            if (data.shape == HitboxShape.Box)
            {
                Quaternion rot = Quaternion.Euler(0f, 0f, data.hitboxRotation * scaleX);
                Vector3 center = origin + rot * new Vector3(data.hitboxOffset.x * scaleX, data.hitboxOffset.y);
                Vector3 size = new(data.hitboxSize.x, data.hitboxSize.y, 0.01f);

                Matrix4x4 oldMatrix = Gizmos.matrix;
                Gizmos.matrix = Matrix4x4.TRS(center, rot, Vector3.one);
                Gizmos.DrawCube(Vector3.zero, size);
                Gizmos.color = outline;
                Gizmos.DrawWireCube(Vector3.zero, size);
                Gizmos.matrix = oldMatrix;
            }
            else
            {
                Vector3 center = origin + new Vector3(data.hitboxOffset.x * scaleX, data.hitboxOffset.y);
                Gizmos.DrawSphere(center, data.hitboxRadius);
                Gizmos.color = outline;
                Gizmos.DrawWireSphere(center, data.hitboxRadius);
            }
        }
    }
#endif
}
