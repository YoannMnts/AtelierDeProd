using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerInteraction : MonoBehaviour
{
    public PlayerController PlayerController => PlayerController.Instance;
    public IInteractable CurrentInteractable { get; private set; }
    
    private Collider[] buffer = new Collider[16];
    public event Action PlayerInteract;
        
    [Header("Interact")]
    [SerializeField]
    private float interactRange;

    
    private void FixedUpdate()
    {
        int size = Physics.OverlapSphereNonAlloc(transform.position, interactRange, buffer);
        
        IInteractable nextInteractable = null;
        for (int i = 0; i < size; i++)
        {
            if (!buffer[i].TryGetComponent(out IInteractable interactable)) 
                continue;
            
            if(nextInteractable == null || interactable.Priority > nextInteractable.Priority)
                nextInteractable = interactable;
        }

        if (CurrentInteractable != nextInteractable)
        {
            if (CurrentInteractable != null)
                CurrentInteractable.OnExit(this);
            
            if (nextInteractable != null)
                nextInteractable.OnEnter(this);
            
            CurrentInteractable = nextInteractable;
        }
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
        if (CurrentInteractable == null) 
            return;
        
        CurrentInteractable.Interact(this);
        PlayerInteract?.Invoke();
    }
}