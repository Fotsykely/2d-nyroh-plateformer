using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float radius = 0.1f;

    public bool IsGrounded => Physics2D.OverlapCircle(transform.position, radius, groundLayer);
}
