using System;
using DefaultNamespace;
using UnityEngine;

public class WorldCreature : MonoBehaviour, IInteractable
{
    [SerializeField]
    private InteractableOutline outline;
    
    private Creature currentCreature;
    
    private void Start()
    {
        currentCreature = CreatureManager.CreateCreature();
    }

    int IInteractable.Priority => 3;

    void IInteractable.Interact(PlayerInteraction playerInteraction)
    {
        currentCreature.Talk(playerInteraction.PlayerController.Codex);
    }

    void IInteractable.OnEnter(PlayerInteraction playerInteraction)
    {
        outline.Show();
    }
    
    void IInteractable.OnExit(PlayerInteraction playerInteraction)
    {
        outline.Hide();
    }

}