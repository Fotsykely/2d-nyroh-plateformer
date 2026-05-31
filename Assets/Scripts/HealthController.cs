using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private HealthData data;
    [SerializeField] private GameEvent onPlayerDied;

    public int CurrentHealth { get; private set; }

    void Start() => CurrentHealth = data.maxHealth;

    public void TakeDamage(int amount)
    {
        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        if (CurrentHealth == 0) Die();
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
