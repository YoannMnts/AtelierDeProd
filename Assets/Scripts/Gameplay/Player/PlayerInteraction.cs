using System;
using Ozkaal.Gameplay.Gameplay.Interaction;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ozkaal.Gameplay.Gameplay.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        public PlayerController PlayerController => PlayerController.Instance;
        
        public IInteractable CurrentInteractable { get; private set; }
        
        public event Action PlayerInteract;
        
        [Header("Interact")]
        [SerializeField] private float interactRange;

        public void Interact()
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