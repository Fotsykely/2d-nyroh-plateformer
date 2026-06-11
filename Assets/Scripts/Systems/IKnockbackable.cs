using UnityEngine;

public interface IKnockbackable
{
    void ApplyKnockback(Vector2 sourcePosition, Vector2 force, float hitstun);
}
