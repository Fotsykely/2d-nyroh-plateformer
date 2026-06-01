using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private HealthData data;
    [SerializeField] private GameEvent onPlayerDied;

    public int CurrentHealth { get; private set; }
    public bool IsInvincible { get; private set; }

    void Start() => CurrentHealth = data.maxHealth;

    public void TakeDamage(int amount)
    {
        if (IsInvincible) return;
        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
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
        onPlayerDied.Raise();
        enabled = false;
    }

    // TODO: Replace with proper health UI
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 30), $"HP : {CurrentHealth} / {data.maxHealth}");
    }
}
