using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Ozkaal.Gameplay.Gameplay.Player
{
    public class PlayerMovement : MonoBehaviour, IPlayerComponent
    {
        public PlayerController playerController { get; set; }
        
        public Vector3 Direction { get; private set; }
        public Vector3 TargetVelocity { get; private set; }
        public Vector3 CurrentVelocity { get; private set; }
        public bool IsJumping { get; private set; }
        
        [Header("Components")]
        [field: SerializeField]
        public CharacterController CharacterController { get; private set; }
        
        [Header("Movement")] 
        [SerializeField] private float maxSpeed; 
        [SerializeField] private float directionAlignDamping; 
        [SerializeField] private float acceleration; 
        [SerializeField] private float deceleration;
        
        [Header("Jump")]
        [SerializeField] private float jumpForce;

        private void OnEnable()
        {
            playerController.PlayerControls.JumpInput.performed += ApplyJumpForce;
        }

        private void OnDisable()
        {
            playerController.PlayerControls.JumpInput.performed -= ApplyJumpForce;
        }

        private void Update()
        {
            IsJumping = !CharacterController.isGrounded;
            ComputeDirection();
            ComputeTargetVelocity();
        }

        private void FixedUpdate()
        {
            ApplyVelocity();
        }

        private void ApplyVelocity()
        {
            CharacterController.Move(TargetVelocity * Time.deltaTime);
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
        
        private void ApplyJumpForce(InputAction.CallbackContext context)
        {
            if (CharacterController.isGrounded && !IsJumping)
            {
                IsJumping = true;
                CharacterController.Move(Vector3.up * jumpForce);
            }
        }
    }
}