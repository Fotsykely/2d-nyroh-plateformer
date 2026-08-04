using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CheckpointHandler : MonoBehaviour
{
    [SerializeField] private CheckpointData data;
    [SerializeField] private GameEvent onCheckpointActivated;
    [SerializeField] private LayerMask targetLayers;

    private bool _activated;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_activated) return;
        if ((targetLayers.value & (1 << other.gameObject.layer)) == 0) return;
        _activated = true;
        data.RespawnPosition = other.attachedRigidbody != null ? other.attachedRigidbody.position : (Vector2)other.transform.position;
        data.HasCheckpoint = true;
        if (onCheckpointActivated != null) onCheckpointActivated.Raise();
    }
}
