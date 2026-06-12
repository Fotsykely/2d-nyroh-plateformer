using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class HealthController : MonoBehaviour
{
    [SerializeField] private HealthData data;
    [FormerlySerializedAs("onPlayerDied")] [SerializeField] private GameEvent onDied;
    [SerializeField] private GameEvent onDamageTaken;
    [SerializeField] private bool destroyOnDeath;
    [SerializeField] private float deathDelay = 0.3f;

    public bool IsInvincible { get; private set; }

    void Awake() => data.CurrentHealth = data.maxHealth;

    public void TakeDamage(int amount)
    {
        if (IsInvincible) return;
        data.CurrentHealth = Mathf.Max(0, data.CurrentHealth - amount);
        if (onDamageTaken != null) onDamageTaken.Raise();
        if (data.CurrentHealth == 0) { Die(); return; }
        StartIframes().Forget();
    }

    private async UniTaskVoid StartIframes()
    {
        IsInvincible = true;
        await UniTask.Delay(
            TimeSpan.FromSeconds(data.iframeDuration),
            cancellationToken: this.GetCancellationTokenOnDestroy());
        IsInvincible = false;
    }

    private void Die()
    {
        if (onDied != null) onDied.Raise();
        if (destroyOnDeath)
        {
            var rb = GetComponent<Rigidbody2D>();
            if (rb != null) { rb.linearVelocity = Vector2.zero; rb.simulated = false; }
            DestroyAfterDelay().Forget();
        }
        else
        {
            enabled = false;
        }
    }

    private async UniTaskVoid DestroyAfterDelay()
    {
        await UniTask.Delay(
            TimeSpan.FromSeconds(deathDelay),
            cancellationToken: this.GetCancellationTokenOnDestroy());
        Destroy(gameObject);
    }
}
