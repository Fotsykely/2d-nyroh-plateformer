using UnityEngine;

public abstract class EnemyData : ScriptableObject
{
    [Header("Contact Attack")]
    public int contactDamage = 1;

    [Header("Knockback infligé au joueur")]
    public Vector2 knockbackForce = new Vector2(8f, 6f);
    public float hitstunDuration = 0.15f;
}
