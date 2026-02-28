using System;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public LayerMask GroundLayer => groundLayer;
        
        
    [SerializeField] private float groundRadius;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundLayer;
        
    public bool IsGrounded { get; private set; }
    public Vector3 GroundNormal { get; private set; }

    private void FixedUpdate()
    {
        /*
        var origin = transform.position;
        Physics.SphereCast(origin, groundDistance, Vector3.down, out RaycastHit hit, groundDistance,groundLayer);
        IsGrounded = hit.normal.y >= 0.75f;
        */
        var origin = transform.position;
        var hits = new RaycastHit[3];
        Physics.SphereCastNonAlloc(origin, groundDistance, Vector3.down, hits ,groundDistance , groundLayer);
        foreach (RaycastHit hit in hits)
        {
            IsGrounded = hit.normal.y >= 0.75f;
            GroundNormal = IsGrounded ? hit.normal : Vector3.zero;
            if (IsGrounded)
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var origin = transform.position;
        var hits = new RaycastHit[3];
        Physics.SphereCastNonAlloc(origin, groundDistance, Vector3.down, hits ,groundDistance , groundLayer);
        foreach (RaycastHit hit in hits)
        {
            IsGrounded = hit.normal.y >= 0.75f;
            GroundNormal = IsGrounded ? hit.normal : Vector3.zero;
            Gizmos.DrawRay(hit.point, hit.normal);
            if (IsGrounded)
                break;
        }
    }
}