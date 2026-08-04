using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    [SerializeField] private CheckpointData checkpointData;
    [SerializeField] private GameEvent onPlayerRespawned;

    private Rigidbody2D _rb;
    private HealthController _health;
    private KnockbackHandler _knockback;
    private PlayerController _playerController;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _health = GetComponent<HealthController>();
        _knockback = GetComponent<KnockbackHandler>();
        _playerController = GetComponent<PlayerController>();
    }

    void Start()
    {
        if (!checkpointData.HasCheckpoint) checkpointData.RespawnPosition = _rb.position;
    }

    public void Respawn() => RespawnRoutine().Forget();

    private async UniTaskVoid RespawnRoutine()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(checkpointData.respawnDelay), cancellationToken: this.GetCancellationTokenOnDestroy());
        _rb.position = checkpointData.RespawnPosition;
        _rb.linearVelocity = Vector2.zero;
        _health.Revive();
        _knockback.ResetHitstun();
        _playerController.enabled = true;
        if (onPlayerRespawned != null) onPlayerRespawned.Raise();
    }
}
