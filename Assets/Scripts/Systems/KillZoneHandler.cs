using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class KillZoneHandler : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayers;

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((targetLayers.value & (1 << other.gameObject.layer)) == 0) return;
        if (other.TryGetComponent<HealthController>(out var health)) health.Kill();
    }
}
