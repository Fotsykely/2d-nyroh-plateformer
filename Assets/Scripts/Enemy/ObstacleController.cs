using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [SerializeField] private AttackData data;
    [SerializeField] private DamageDealer damageDealer;

    void Awake()
    {
        damageDealer.Damage = data.damage;
        damageDealer.KnockbackForce = data.knockbackForce;
        damageDealer.HitstunDuration = data.hitstunDuration;

        if (data.shape == HitboxShape.Box && TryGetComponent<BoxCollider2D>(out var box))
        {
            box.isTrigger = true;
            box.offset = data.hitboxOffset;
            box.size = data.hitboxSize;
        }
        else if (data.shape == HitboxShape.Circle && TryGetComponent<CircleCollider2D>(out var circle))
        {
            circle.isTrigger = true;
            circle.offset = data.hitboxOffset;
            circle.radius = data.hitboxRadius;
        }
        else if (data.shape == HitboxShape.Triangle && TryGetComponent<PolygonCollider2D>(out var poly))
        {
            poly.isTrigger = true;
            poly.offset = data.hitboxOffset;
            poly.SetPath(0, data.trianglePoints);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (data == null) return;
        Gizmos.color = data.gizmoColor;
        Vector3 origin = transform.position + (Vector3)data.hitboxOffset;

        if (data.shape == HitboxShape.Box)
            Gizmos.DrawCube(origin, data.hitboxSize);
        else if (data.shape == HitboxShape.Circle)
            Gizmos.DrawSphere(origin, data.hitboxRadius);
        else if (data.shape == HitboxShape.Triangle && data.trianglePoints.Length >= 3)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector3 a = origin + (Vector3)data.trianglePoints[i];
                Vector3 b = origin + (Vector3)data.trianglePoints[(i + 1) % 3];
                Gizmos.DrawLine(a, b);
            }
        }
    }
#endif
}
