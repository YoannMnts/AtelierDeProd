using System;
using UnityEngine;

namespace Ozkaal.Gameplay.Gameplay.Player
{
    public partial class PlayerMovement
    {
        private void OnDrawGizmos()
        {
            Vector3 center = rb.position + Vector3.up;
            Vector3 size = Vector3.one * 0.5f;
            Gizmos.DrawWireCube(center, size);
        }
    }
}