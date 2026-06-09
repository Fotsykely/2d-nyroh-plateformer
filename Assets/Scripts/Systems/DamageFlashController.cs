using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DamageFlashController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Material flashMaterial;
    [SerializeField] private HealthData data;

    private Material _normalMaterial;

    void Awake() => _normalMaterial = spriteRenderer.material;

    public void OnDamageTaken() => Flash().Forget();

    private async UniTaskVoid Flash()
    {
        var token = this.GetCancellationTokenOnDestroy();
        float elapsed = 0f;
        bool on = false;
        while (elapsed < data.iframeDuration)
        {
            on = !on;
            spriteRenderer.material = on ? flashMaterial : _normalMaterial;
            await UniTask.Delay(TimeSpan.FromSeconds(data.blinkInterval), cancellationToken: token);
            elapsed += data.blinkInterval;
        }
        spriteRenderer.material = _normalMaterial;
    }
}
