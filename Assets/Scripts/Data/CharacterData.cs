using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Nyroh/CharacterData")]
public class CharacterData : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Jump")]
    public float jumpForce = 12f;
    public float coyoteTime = 0.15f;
    public float jumpBufferTime = 0.1f;
    public float fallGravityMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float maxFallSpeed = 20f;
}
