using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Ozkaal.Gameplay.Gameplay.Player
{
    //make for POC:
    //Jump + Crouch + Gravity + renderer + CharacterController --> Rigidbody
    public class PlayerMovement : MonoBehaviour
    {
        public Vector3 Direction { get; private set; }
        public Vector3 TargetVelocity { get; private set; }
        public Vector3 CurrentVelocity { get; private set; }
        
        public bool IsCrouching { get; private set; }
        
        
        [Header("Component")]
        [SerializeField] private CharacterController controller;
        [SerializeField] private new GameObject renderer;
        
        [Header("Movement")] 
        [SerializeField] private float maxSpeed; 
        [SerializeField] private float directionAlignDamping; 
        [SerializeField] private float acceleration; 
        [SerializeField] private float deceleration;
        
        [Header("Jump")]
        [SerializeField] private float jumpForce;
        [SerializeField] private float jumpHeight;
        [SerializeField] private float jumpTime;
        [SerializeField] private float jumpCoyoteTime;
        private bool isJumpPressed;
        private bool isJumping;
        
        [Header("Crouch")]
        [SerializeField] private float crouchSpeed;
        
        [Header("Gravity")]
        [SerializeField] private Vector3 gravity;
        [SerializeField] private Vector3 groundedGravity;
        [SerializeField] private float gravityMultiplier;
        private Vector3 currentGravity;
        
        [Header("Raycasts")]
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundCheckDistance;

        private void OnValidate()
        {
            gravity = Physics.gravity * gravityMultiplier;
        }

        private void OnEnable()
        {
            //PlayerController.Instance.PlayerControls.JumpInput.performed += ApplyJumpForce;
        }

        private void OnDisable()
        {
            //PlayerController.Instance.PlayerControls.JumpInput.performed -= ApplyJumpForce;
        }

        private void Update()
        {
            ComputeDirection();
            ComputeTargetVelocity();
            ComputeGravity();
            ComputeJump();
            //Crouch();
            //Debug.Log(gravity);
        }

        private void FixedUpdate()
        {
            ApplyVelocity();
        }

        private void ApplyVelocity()
        {
            Debug.Log(TargetVelocity);
            controller.Move(TargetVelocity * Time.deltaTime);
        }

        private void ComputeTargetVelocity()
        {   
            var lastTargetVelocity = TargetVelocity;

            var wantToStop = Direction.sqrMagnitude < 0.1f;
            
            var finalTargetSpeed = wantToStop ? 0 : maxSpeed;
            var finalTargetVelocity = Direction * finalTargetSpeed;
            
            var targetVelocity = Vector3.Lerp(
                lastTargetVelocity,
                finalTargetVelocity,
                directionAlignDamping * Time.fixedDeltaTime).normalized;
            
            var lastTargetSpeed = TargetVelocity.magnitude;
            var delta = wantToStop ? -deceleration : acceleration;
            
            var targetSpeed = lastTargetSpeed + delta * Time.deltaTime;
            targetSpeed = Mathf.Clamp(targetSpeed, 0, maxSpeed);
            
            TargetVelocity = targetVelocity * targetSpeed;
        }

        private void ComputeDirection()
        {
            var direction = PlayerController.Instance.PlayerControls.GetMovementInput();
            Direction = new Vector3(direction.x, currentGravity.y, direction.y).normalized;
        }

        private void ComputeGravity()
        {
            if (controller.isGrounded)
            {
                currentGravity = -groundedGravity;
            }
            else
            {
                float previousVelocity = TargetVelocity.y;
                float newYVelocity = TargetVelocity.y + (gravity.y * Time.deltaTime);
                float nextYVelocity = (previousVelocity + newYVelocity) * .5f;
                currentGravity.y = -nextYVelocity;
            }
        }
        
        private void ComputeJump()
        {
            if (!isJumping && controller.isGrounded && isJumpPressed)
            {
                isJumping = true;
                var vector3 = CurrentVelocity;
                vector3.y = jumpForce * .5f;
                CurrentVelocity = vector3;
            }
            else if (isJumping && controller.isGrounded && !isJumpPressed)
            {
                isJumping = false;
            }
        }
        private void ApplyJumpForce(InputAction.CallbackContext context)
        {
            
        }
    }
}