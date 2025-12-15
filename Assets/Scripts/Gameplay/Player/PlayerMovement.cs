using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Ozkaal.Gameplay.Gameplay.Player
{
    //make for POC:
    //Jump + Crouch + Gravity + renderer + CharacterController --> Rigidbody
    public class PlayerMovement : MonoBehaviour, IPlayerComponent
    {
        public PlayerController playerController { get; set; }
        
        public Vector3 Direction { get; private set; }
        public Vector3 TargetVelocity { get; private set; }
        public Vector3 CurrentVelocity { get; private set; }
        public Vector3 GroundNormal { get; private set; }
        
        public bool IsJumping { get; private set; }
        public bool IsGrounded { get; private set; }
        public bool IsCrouching { get; private set; }
        
        [Header("Component")]
        [SerializeField] private Rigidbody rb;

        [SerializeField] private new GameObject renderer;
        
        [Header("Movement")] 
        [SerializeField] private float maxSpeed; 
        [SerializeField] private float directionAlignDamping; 
        [SerializeField] private float acceleration; 
        [SerializeField] private float deceleration;
        
        [Header("Jump")]
        [SerializeField] private float jumpForce;
        [SerializeField] private float jumpCoyoteTime;
        
        [Header("Crouch")]
        [SerializeField] private float crouchSpeed;
        
        
        [Header("Gravity")]
        [SerializeField] private float gravityMultiplier;
        
        [Header("Raycasts")]
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundCheckDistance;
        
        [Header("Colliders")]
        [SerializeField] private GameObject normalCollider;
        [SerializeField] private GameObject crouchCollider;

        private void OnEnable()
        {
            //playerController.PlayerControls.JumpInput.performed += ApplyJumpForce;
        }

        private void OnDisable()
        {
            //playerController.PlayerControls.JumpInput.performed -= ApplyJumpForce;
        }

        private void Update()
        {
            ComputeDirection();
            ComputeTargetVelocity();
            ComputeGravity();
            ComputeJump();
            Crouch();
        }

        private void FixedUpdate()
        {
            CheckGround();
            ApplyVelocity();
        }

        private void ApplyVelocity()
        {
            Vector3 finalTargetVelocity = Vector3.ProjectOnPlane(TargetVelocity, GroundNormal);
            Vector3 verticalVelocity = Vector3.Project(CurrentVelocity, GroundNormal);
            
            var dot = Vector3.Dot(verticalVelocity.normalized, Physics.gravity);
            if (IsGrounded && dot >= 0)
                verticalVelocity = Vector3.zero;
            CurrentVelocity = finalTargetVelocity + verticalVelocity;
            rb.linearVelocity = CurrentVelocity;
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
            
            TargetVelocity = targetVelocity * targetSpeed + Physics.gravity;
        }

        private void ComputeDirection()
        {
            var direction = playerController.PlayerControls.GetMovementInput();
            Direction = new Vector3(direction.x, 0.0f, direction.y).normalized;
        }

        private void CheckGround()
        {
            Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundCheckDistance, groundLayer);
            if (hit.collider)
            {
                IsGrounded = true;
                GroundNormal = hit.normal;
            }
            else
            {
                IsGrounded = false;
                GroundNormal = -Physics.gravity.normalized;
            }
        }

        private void ComputeGravity()
        {
            Vector3 worldGravity = Physics.gravity;
            
            Vector3 currentGravity = worldGravity * gravityMultiplier;
            CurrentVelocity += currentGravity;
            CurrentVelocity = Vector3.ClampMagnitude(CurrentVelocity, maxSpeed);
        }
        
        private void ComputeJump()
        {
            var wantToJump = playerController.PlayerControls.JumpInput.IsPressed();
            if (IsGrounded && !IsJumping)
            {
                IsJumping = false;
            }
            if (wantToJump && IsGrounded)
            {
                var temp = CurrentVelocity;
                temp.y = jumpForce;
                CurrentVelocity = temp;
            }

        }
        private void ApplyJumpForce(InputAction.CallbackContext context)
        {
            IsJumping = true;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Force);
        }

        private void Crouch()
        {
            if (playerController.PlayerControls.CrouchInput.IsPressed())
            {
                IsCrouching = true;
                normalCollider.SetActive(false);
                crouchCollider.SetActive(true);
                renderer.transform.localScale = new Vector3(1, 0.5f, 1);
            }
            else
            {
                IsCrouching = false;
                normalCollider.SetActive(true);
                crouchCollider.SetActive(false);
                renderer.transform.localScale = new Vector3(1, 1, 1);
            }
        }


        private void OnDrawGizmos()
        {
            Vector3 rayDirection = Physics.gravity.normalized * groundCheckDistance;
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(rb.position, rayDirection);
        }
    }
}