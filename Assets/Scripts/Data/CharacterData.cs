using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Nyroh/CharacterData")]
public class CharacterData : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float groundAccel = 80f;   // u/s² — démarrage au sol
    public float groundDecel = 90f;   // u/s² — arrêt au sol (friction)
    public float airAccel = 50f;      // contrôle aérien réduit
    public float airDecel = 30f;

    [Header("Jump")]
    public float jumpForce = 16f;
    public float coyoteTime = 0.12f;
    public float jumpBufferTime = 0.12f;
    public float riseGravityMultiplier = 3.5f;  // gravité en montée (bouton tenu)
    public float fallGravityMultiplier = 4.5f;  // chute
    public float lowJumpMultiplier = 6f;        // jump-cut : relâcher tôt = saut court
    public float maxFallSpeed = 28f;

    [Header("Apex hang")]
    public float apexThreshold = 3f;      // |vy| sous lequel on est "près du sommet"
    public float apexGravityScale = 0.5f; // gravité allégée au sommet (flottement)
}
