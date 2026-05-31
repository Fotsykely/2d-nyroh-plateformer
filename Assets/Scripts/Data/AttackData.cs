using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Nyroh/AttackData")]
public class AttackData : ScriptableObject
{
    public float attackDuration = 0.3f;
    public int damage = 10;
    public Vector2 hitboxOffset = new(0.5f, 0f);
    public Vector2 hitboxSize = new(1.5f, 0.8f);

#if UNITY_EDITOR
    public Color gizmoColor = new(1f, 0.2f, 0.2f, 0.35f);
#endif
}
