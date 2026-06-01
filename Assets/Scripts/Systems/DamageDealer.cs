using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayers;

    public int Damage { get; set; }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((targetLayers.value & (1 << other.gameObject.layer)) == 0) return;
        if (other.TryGetComponent<HealthController>(out var health) && !health.IsInvincible)
            health.TakeDamage(Damage);
    }
}
