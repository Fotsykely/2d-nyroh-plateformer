using UnityEngine;

[CreateAssetMenu(fileName = "HealthData", menuName = "Nyroh/HealthData")]
public class HealthData : ScriptableObject
{
    public int maxHealth = 3;
    public float iframeDuration = 0.8f;
}
