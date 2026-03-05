using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public PlayerController PlayerController => PlayerController.Instance;
        
    public IInteractable CurrentInteractable { get; private set; }
    
    private Collider[] buffer = new Collider[16];
    
    public event Action PlayerInteract;
        
    [Header("Interact")]
    [SerializeField] private float interactRange;

    private int size;

    private void FixedUpdate()
    {
        size = Physics.OverlapSphereNonAlloc(transform.position, interactRange, buffer);
    }
    
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
        for (int i = 0; i < size; i++)
        {
            if (!buffer[i].TryGetComponent(out IInteractable interactable)) continue;
            CurrentInteractable = interactable;
            interactable.Interact(this);
            PlayerInteract?.Invoke();
        }
        
        
    }
}