using Ozkaal.Gameplay.Gameplay.Interaction;
using Ozkaal.Gameplay.Gameplay.Player;
using UnityEngine;

public class WorldCreature : MonoBehaviour, IInteractable
{
    private Creature currentCreature;
        
    private void Start()
    {
        currentCreature = CreatureManager.CreateCreature();
    }

    public void Interact(PlayerInteraction playerInteraction)
    {
        currentCreature.Talk(playerInteraction.PlayerController.Codex);
    }
}