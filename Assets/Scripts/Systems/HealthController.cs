using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private HealthData data;
    [SerializeField] private GameEvent onPlayerDied;
    [SerializeField] private GameEvent onDamageTaken;

    public int CurrentHealth { get; private set; }
    public int MaxHealth => data.maxHealth;
    public bool IsInvincible { get; private set; }

    void Start() => CurrentHealth = data.maxHealth;

    public void TakeDamage(int amount)
    {
        if (IsInvincible) return;
        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        if (onDamageTaken != null) onDamageTaken.Raise();
        if (CurrentHealth == 0) { Die(); return; }
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
