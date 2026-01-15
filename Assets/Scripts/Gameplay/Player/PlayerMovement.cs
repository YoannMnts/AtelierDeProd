using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Utilities;

namespace Ozkaal.Gameplay.Gameplay.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody rb;
        [SerializeField] private GroundChecker groundChecker;
        [SerializeField] private PlayerControls input;
        [SerializeField] private SphereCollider normalCollider;
        
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float rotationSpeed = 100f;
        [SerializeField] private float smoothTime = 0.2f;

        [Header("Jump Settings")]
        [SerializeField] private float launchPoint = .9f;
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private float jumpDuration = 0.5f;
        [SerializeField] private float jumpCooldown = 0f;
        [SerializeField] private float jumpMaxHeight = 2f;
        [SerializeField] private float gravityMultiplier = 3f;
        
        
        private const float ZERO_F = 0f;
        
        private float currentSpeed; //smooth speed for animator
        private float velocity;
        private float jumpVelocity;
        
        Vector3 movement;

        private List<Timer> timers;
        private CountdownTimer jumpTimer;
        private CountdownTimer jumpCountdownTimer;

        //private Transform mainCam;
        

        private void Awake()
        {
            //mainCam = Camera.main.transform;
            rb.freezeRotation = true;
            
            //Setup timers
            jumpTimer = new CountdownTimer(jumpDuration);
            jumpCountdownTimer = new CountdownTimer(jumpCooldown);
            timers = new(2) { jumpTimer, jumpCountdownTimer };

            //jumpTimer.OnTimerStop += () => jumpCountdownTimer.Start();   comprend pas
        }

        void Start() => input.EnablePlayerActions();

        private void OnEnable()
        {
            input.Jump += OnJump;
        }

        private void OnDisable()
        {
            input.Jump -= OnJump;
        }

        private void OnJump(bool performed)
        {
            if (performed && !jumpTimer.IsRunning && !jumpCountdownTimer.IsRunning && groundChecker.IsGrounded)
                jumpTimer.Start();
            else if (!performed && jumpTimer.IsRunning) ;
                //jumpTimer.Stop();
        }

        private void Update()
        {
            movement = new Vector3(input.Direction.x, 0, input.Direction.y);
            //Debug.Log($"Jump Timer: {jumpTimer.IsFinished}");
            //Debug.Log($"Jump Countdown: {jumpCountdownTimer.IsFinished}");
            HandleTimers();
            //UpdateAnimator();
        }


        private void FixedUpdate()
        {
            HandleJump();
            HandleMovement();
        }

        private void HandleTimers()
        { 
            foreach (Timer timer in timers)
            {
                timer.Tick(Time.deltaTime);
            }
        }
        private void HandleJump()
        {
            //if not jumping and grounded
            if (!jumpTimer.IsRunning && groundChecker.IsGrounded)
            {
                jumpVelocity = ZERO_F;
                jumpTimer.Stop();
                return;
            }

            //if jumping or falling, calculate velocity
            if (jumpTimer.IsRunning)
            {
                /*
                //Progress point for initial burst of velocity
                if (jumpTimer.Progress > launchPoint)
                {
                    //calculate the velocity required to reach the jump height using physics equations v = sqrt(2gh)
                    jumpVelocity = Mathf.Sqrt(2 * jumpMaxHeight * Mathf.Abs(Physics.gravity.y));
                }
                else
                {
                    //gradually apply less velocity as the jump progresses
                    jumpVelocity += (1- jumpTimer.Progress) * jumpForce * Time.fixedDeltaTime;
                }*/
                jumpVelocity = Mathf.Sqrt(2 * jumpMaxHeight * Mathf.Abs(Physics.gravity.y)) * (1- jumpTimer.Progress);
            }
            else
            {
                //Gravity takes over
                jumpVelocity += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
            }
            
            //apply velocity
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpVelocity, rb.linearVelocity.z);
        }

        private void HandleMovement()
        {
            //rotate movement direction to match camera rotation
            var adjustedDirection = movement; //Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) *
            if (adjustedDirection.magnitude > ZERO_F)
            {
                HandleRotation(adjustedDirection);

                HandleHorizontalMovement(adjustedDirection);

                SmoothSpeed(adjustedDirection.magnitude);
            }
            else
            {
                SmoothSpeed(ZERO_F);
                
                rb.linearVelocity = new Vector3(ZERO_F, rb.linearVelocity.y, ZERO_F);
            }
        }

        private void HandleHorizontalMovement(Vector3 adjustedDirection)
        {
            //Move the player
            Vector3 applyVelocity = adjustedDirection * (moveSpeed * Time.fixedDeltaTime);
            rb.linearVelocity = applyVelocity;
        }

        private void HandleRotation(Vector3 adjustedDirection)
        {
            //Adjust rotation to match movement
            var targetRotation = Quaternion.LookRotation(adjustedDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.LookAt(transform.position + adjustedDirection);
        }

        private void SmoothSpeed(float adjustedDirection)
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, adjustedDirection, ref velocity, smoothTime);
        }
    }
}