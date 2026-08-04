using UnityEngine;

[CreateAssetMenu(fileName = "CheckpointData", menuName = "Nyroh/CheckpointData")]
public class CheckpointData : ScriptableObject
{
    public float respawnDelay = 1f;
    [System.NonSerialized] public Vector2 RespawnPosition;
    [System.NonSerialized] public bool HasCheckpoint;
}
