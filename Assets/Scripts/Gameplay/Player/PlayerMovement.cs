using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Utilities;

namespace Ozkaal.Gameplay.Gameplay.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        /* ───────────────────────── REFERENCES ───────────────────────── */

        [Header("References")]

        [Tooltip("Rigidbody used to move the player using physics (velocity-based movement and gravity).")]
        [SerializeField] private Rigidbody rb;

        [Tooltip("Checks whether the player is currently touching the ground (used to allow jumping).")]
        [SerializeField] private GroundChecker groundChecker;

        [Tooltip("Input wrapper that provides input.")]
        [SerializeField] private PlayerControls input;


        /* ───────────────────────── MOVEMENT SETTINGS ───────────────────────── */

        [Header("Movement Settings")]

        [Tooltip("Horizontal movement speed applied to the player.")]
        [SerializeField] private float moveSpeed = 10f;

        [Tooltip("Rotation speed used to rotate the player toward movement direction.")]
        [SerializeField] private float rotationSpeed = 100f;

        [Tooltip("Smoothing time used to interpolate movement speed (for animation blending).")]
        [SerializeField] private float smoothTime = 0.2f;


        /* ───────────────────────── JUMP SETTINGS ───────────────────────── */

        [Header("Jump Settings")]

        [Tooltip("Total duration of the jump phase (used by the jump timer).")]
        [SerializeField] private float jumpDuration = 0.5f;

        [Tooltip("Cooldown time after landing before another jump is allowed.")]
        [SerializeField] private float jumpCooldown = 0f;

        [Tooltip("Maximum height the jump should reach (in world units).")]
        [SerializeField] private float jumpMaxHeight = 2f;

        [Tooltip("Multiplier applied to gravity when the player is falling.")]
        [SerializeField] private float gravityMultiplier = 3f;

        
        /* ───────────────────────── Colliders ───────────────────────── */

        [Header("Colliders references")] 
        
        [Tooltip("Default collider used when the player is in normal movement state.")]
        [SerializeField] private CapsuleCollider defaultCollider;
        
        [Tooltip("Crouch collider used when the player is in crouch movement state.")]
        [SerializeField] private CapsuleCollider crouchCollider;

        
        /* ───────────────────────── INTERNAL STATE ───────────────────────── */

        private const float ZERO_F = 0f;

        // Smoothed speed value (usually for animation blending)
        private float currentSpeed;

        // Velocity reference used by SmoothDamp
        private float velocity;

        // Vertical velocity applied to the Rigidbody for jumping/falling
        private float jumpVelocity;

        // Raw movement direction from input
        private Vector3 movement;

        // Collection used to update all timers in one loop
        private List<Timer> timers;

        // Controls the active jump phase
        private CountdownTimer jumpTimer;

        // Controls the cooldown between jumps
        private CountdownTimer jumpCountdownTimer;

        private CapsuleCollider currentCollider;

        /* ───────────────────────── UNITY LIFECYCLE ───────────────────────── */

        private void Awake()
        {
            // Prevent physics from rotating the player
            rb.freezeRotation = true;

            // Create jump timer (controls jump duration)
            jumpTimer = new CountdownTimer(jumpDuration);

            // Create cooldown timer (prevents jump spamming)
            jumpCountdownTimer = new CountdownTimer(jumpCooldown);

            // Store timers in a list so they can all be ticked together
            timers = new(2) { jumpTimer, jumpCountdownTimer };

            // When the jump timer ends, automatically start the cooldown timer
            jumpTimer.OnTimerStop += () => jumpCountdownTimer.Start();
        }

        private void Start()
        {
            // Enable player input actions
            input.EnablePlayerActions();
            currentCollider = defaultCollider;
            crouchCollider.enabled = false;
        }

        private void OnEnable()
        {
            // Subscribe to jump input
            input.Jump += OnJump;
            input.Crouch += OnCrouch;
        }

        private void OnDisable()
        {
            // Unsubscribe from jump input
            input.Jump -= OnJump;
            input.Crouch -= OnCrouch;
        }


        /* ───────────────────────── INPUT HANDLING ───────────────────────── */

        private void OnJump(bool performed)
        {
            // Start jump only if:
            // - Button pressed
            // - Not already jumping
            // - Not in cooldown
            // - Player is grounded
            if (performed && !jumpTimer.IsRunning && !jumpCountdownTimer.IsRunning && groundChecker.IsGrounded)
            {
                jumpTimer.Start();
            }
            // Optional early jump cancel
            else if (!performed && jumpTimer.IsRunning)
            {
                // jumpTimer.Stop();
            }
        }

        private void OnCrouch(bool wantToCrouch)
        {
            if (wantToCrouch && groundChecker.IsGrounded)
            {
                ActivateColliders(crouchCollider);
            }
            else if (!wantToCrouch || !groundChecker.IsGrounded)
            {
                ActivateColliders(defaultCollider);
            }
        }

        private void ActivateColliders(CapsuleCollider newCollider)
        {
            currentCollider.enabled = false;
            currentCollider = newCollider;
            currentCollider.enabled = true;
        }

        /* ───────────────────────── UPDATE LOOP ───────────────────────── */

        private void Update()
        {
            // Convert 2D input into world-space movement direction
            movement = new Vector3(input.Direction.x, 0f, input.Direction.y);

            // Update all timers
            HandleTimers();
        }

        private void FixedUpdate()
        {
            // Handle horizontal movement & rotation
            HandleMovement();

            // Handle vertical movement (jump & gravity)
            HandleJump();
        }


        /* ───────────────────────── TIMER MANAGEMENT ───────────────────────── */

        private void HandleTimers()
        {
            // Tick all timers using deltaTime
            foreach (Timer timer in timers)
            {
                timer.Tick(Time.deltaTime);
            }
        }


        /* ───────────────────────── JUMP LOGIC ───────────────────────── */

        private void HandleJump()
        {
            // If grounded and not jumping, reset vertical velocity
            if (!jumpTimer.IsRunning && groundChecker.IsGrounded)
            {
                jumpVelocity = ZERO_F;
                jumpTimer.Stop();
                return;
            }

            // While jump timer is running, calculate upward velocity
            if (jumpTimer.IsRunning)
            {
                // Physics-based jump velocity with progressive reduction and using physics equations v = sqrt(2gh)
                jumpVelocity =
                    Mathf.Sqrt(2f * jumpMaxHeight * Mathf.Abs(Physics.gravity.y))
                    * (1f - jumpTimer.Progress);
            }
            else
            {
                // Apply gravity when not actively jumping
                jumpVelocity += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
            }

            // Apply final vertical velocity to Rigidbody
            rb.linearVelocity = new Vector3(
                rb.linearVelocity.x,
                jumpVelocity,
                rb.linearVelocity.z
            );
        }


        /* ───────────────────────── MOVEMENT LOGIC ───────────────────────── */

        private void HandleMovement()
        {
            var adjustedDirection = movement;

            if (adjustedDirection.magnitude > ZERO_F)
            {
                HandleRotation(adjustedDirection);
                HandleHorizontalMovement(adjustedDirection);
                SmoothSpeed(adjustedDirection.magnitude);
            }
            else
            {
                // Stop horizontal movement when no input
                SmoothSpeed(ZERO_F);

                rb.linearVelocity = new Vector3(
                    ZERO_F,
                    rb.linearVelocity.y,
                    ZERO_F
                );
            }
        }

        private void HandleHorizontalMovement(Vector3 adjustedDirection)
        {
            // Apply horizontal velocity
            Vector3 applyVelocity = adjustedDirection * (moveSpeed * Time.fixedDeltaTime);
            rb.linearVelocity = applyVelocity;
        }

        private void HandleRotation(Vector3 adjustedDirection)
        {
            // Rotate the player toward movement direction
            var targetRotation = Quaternion.LookRotation(adjustedDirection);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            // Ensure forward vector matches movement
            transform.LookAt(transform.position + adjustedDirection);
        }

        private void SmoothSpeed(float targetSpeed)
        {
            // Smooth speed changes for animation blending
            currentSpeed = Mathf.SmoothDamp(
                currentSpeed,
                targetSpeed,
                ref velocity,
                smoothTime
            );
        }
    }
}
