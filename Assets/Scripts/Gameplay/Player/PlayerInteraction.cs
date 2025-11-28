using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Player
{
    public class PlayerInteraction : MonoBehaviour, IPlayerComponent
    {
        public PlayerController playerController { get; set; }
        
        [Header("Interact")]
        [SerializeField] private float interactRange;

        private void OnEnable()
        {
            Debug.Log(playerController);
            playerController.PlayerControls.InteractInput.performed += Interact;
        }

        private void OnDisable()
        {
            playerController.PlayerControls.InteractInput.performed -= Interact;
        }

        private void Interact(InputAction.CallbackContext context)
        {
            Debug.Log("Interact");
        }
    }
}