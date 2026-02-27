using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public LayerMask GroundLayer => groundLayer;
        
        
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundLayer;
        
    public bool IsGrounded { get; private set; }
    public Vector3 GroundNormal { get; private set; }

    private void FixedUpdate()
    {
        Physics.SphereCast(transform.position, groundDistance, Vector3.down, out RaycastHit hit, groundDistance,groundLayer);
        IsGrounded = hit.normal.y >= 0.75f;
        GroundNormal = IsGrounded ? hit.normal : Vector3.zero;
    }
}