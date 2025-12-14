using System;
using Ozkaal.Gameplay.Gameplay.Interaction;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ozkaal.Gameplay.Gameplay.Player
{
    public class PlayerInteraction : MonoBehaviour, IPlayerComponent
    {
        public PlayerController playerController { get; set; }
        
        public IInteractable CurrentInteractable { get; private set; }
        
        public event Action PlayerInteract;
        
        [Header("Interact")]
        [SerializeField] private float interactRange;

        private void OnEnable()
        {
            playerController.PlayerControls.InteractInput.performed += Interact;
        }

        private void OnDisable()
        {
            playerController.PlayerControls.InteractInput.performed -= Interact;
        }

        private void Interact(InputAction.CallbackContext context)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, interactRange);
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out IInteractable interactable))
                {
                    CurrentInteractable = interactable;
                    interactable.Interact(this);
                    PlayerInteract?.Invoke();
                }
            }
        }
    }
}