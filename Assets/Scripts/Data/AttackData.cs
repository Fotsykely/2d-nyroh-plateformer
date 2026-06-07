using UnityEngine;

public enum HitboxShape { Box, Circle, Triangle }

[CreateAssetMenu(fileName = "AttackData", menuName = "Nyroh/AttackData")]
public class AttackData : ScriptableObject
{
    public float attackDuration = 0.3f;
    public int damage = 10;

    public HitboxShape shape = HitboxShape.Box;
    public Vector2 hitboxOffset = new(0.5f, 0f);
    public float hitboxRotation = 0f;

    [Tooltip("Box: largeur/hauteur — Circle: ignoré (utiliser hitboxRadius)")]
    public Vector2 hitboxSize = new(1.5f, 0.8f);

    [Tooltip("Utilisé uniquement si shape = Circle")]
    public float hitboxRadius = 0.5f;

    [Tooltip("Sommets du triangle (espace local) — uniquement si shape = Triangle")]
    public Vector2[] trianglePoints = { new(0f, 0.5f), new(-0.5f, -0.5f), new(0.5f, -0.5f) };

#if UNITY_EDITOR
    public Color gizmoColor = new(1f, 0.2f, 0.2f, 0.35f);
#endif
}
