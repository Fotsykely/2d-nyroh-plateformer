using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private HealthData data;
    [SerializeField] private GameEvent onPlayerDied;
    [SerializeField] private GameEvent onDamageTaken;

    public bool IsInvincible { get; private set; }

    void Start() => data.CurrentHealth = data.maxHealth;

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
        if (onPlayerDied != null) onPlayerDied.Raise();
        enabled = false;
    }

}
