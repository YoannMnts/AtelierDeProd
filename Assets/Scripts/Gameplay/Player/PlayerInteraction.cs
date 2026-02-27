using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public PlayerController PlayerController => PlayerController.Instance;
        
    public IInteractable CurrentInteractable { get; private set; }
        
    public event Action PlayerInteract;
        
    [Header("Interact")]
    [SerializeField] private float interactRange;

    private void OnEnable()
    {
        PlayerController.PlayerControls.Interact += TryInteract;
    }

    private void OnDisable()
    {
        PlayerController.PlayerControls.Interact -= TryInteract;
    }

    private void TryInteract()
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