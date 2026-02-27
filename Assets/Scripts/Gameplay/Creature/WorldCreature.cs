using System;
using UnityEngine;

public class WorldCreature : MonoBehaviour, IInteractable
{
    private Creature currentCreature;
        
    private void Start()
    {
        currentCreature = CreatureManager.CreateCreature();
    }

    private void OnEnable()
    {
        //PlayerController.Instance.PlayerControls.Escape += currentCreature.StopTalking;
    }

    private void OnDisable()
    {
        throw new NotImplementedException();
    }

    public void Interact(PlayerInteraction playerInteraction)
    {
        currentCreature.Talk(playerInteraction.PlayerController.Codex);
    }
}