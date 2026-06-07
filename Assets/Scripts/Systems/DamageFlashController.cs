using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DamageFlashController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float flashDuration = 0.1f;

    private Material _normalMaterial;

    void Awake() => _normalMaterial = spriteRenderer.material;

    public void OnDamageTaken() => Flash().Forget();

    private async UniTaskVoid Flash()
    {
        spriteRenderer.material = flashMaterial;
        await UniTask.Delay(
            TimeSpan.FromSeconds(flashDuration),
            cancellationToken: this.GetCancellationTokenOnDestroy());
        spriteRenderer.material = _normalMaterial;
    }
}
