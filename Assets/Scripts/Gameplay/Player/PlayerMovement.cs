using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Ozkaal.Gameplay.Gameplay.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] CharacterController controller;
        [SerializeField] PlayerControls controls;
        
        [Header("Settigns")]
        [SerializeField] float moveSpeed = 10f;
        [SerializeField] float RotationSpeed = 100f;
        [SerializeField] float smoothTime = 0.2f;

        private const float ZERO_F = 0f;
        private Transform mainCam;

        private float currentSpeed;
        private float velocity;

        private void Awake()
        {
            mainCam = Camera.main.transform;
        }

        private void Update()
        {
            HandleMovement();
            //UpdateAnimator();
        }

        private void HandleMovement()
        {
            var movementDirection = new Vector3(controls.Direction.x, 0, controls.Direction.y).normalized;
            //controller.Move(movementDirection * (Time.deltaTime * moveSpeed));
            
            //rotate movement direction to match camera rotation
            var adjustedDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * movementDirection;
            if (adjustedDirection.magnitude > ZERO_F)
            {
                HandleRotation(adjustedDirection);

                HandleCharacterController(adjustedDirection);

                SmoothSpeed(adjustedDirection.magnitude);
            }
            else
            {
                SmoothSpeed(ZERO_F);
            }
            
        }

        private void HandleCharacterController(Vector3 adjustedDirection)
        {
            //Move the player
            var adjustedMovement = adjustedDirection * (moveSpeed * Time.deltaTime);
            controller.Move(adjustedMovement);
        }

        private void HandleRotation(Vector3 adjustedDirection)
        {
            //Adjust rotation to match movement
            var targetRotation = Quaternion.LookRotation(adjustedDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
            transform.LookAt(transform.position + adjustedDirection);
        }

        private void SmoothSpeed(float adjustedDirection)
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, adjustedDirection, ref velocity, smoothTime);
        }
    }
}