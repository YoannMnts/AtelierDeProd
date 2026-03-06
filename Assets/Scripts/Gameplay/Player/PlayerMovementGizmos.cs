using UnityEngine;

public partial class PlayerMovement
{
    private void OnDrawGizmos()
    {
        Vector3 center = rb.position + crouchCheckCenter;
        Vector3 size = crouchCheckHalfSize * 2;
        Gizmos.DrawWireCube(center, size);
    }
}