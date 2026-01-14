using System;
using UnityEngine;

namespace Ozkaal.Gameplay.Gameplay.Player
{
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] private float groundDistance;
        [SerializeField] private LayerMask groundLayer;
        
        public bool IsGrounded { get; private set; }

        private void Update()
        {
            IsGrounded = Physics.SphereCast(transform.position, groundDistance, Vector3.down, out _, groundDistance,groundLayer);
        }
    }
}