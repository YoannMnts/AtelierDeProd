using System;
using Ozkaal.Gameplay.Gameplay.Interaction;
using Ozkaal.Gameplay.Gameplay.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Creature
{
    public class WorldCreature : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private int numberOfSymbolGroup = 3;
        
        [SerializeField]
        private Transform symbolGroupRoot;
        
        private Creature currentCreature;

        private void Start()
        {
            currentCreature = new Creature(numberOfSymbolGroup, symbolGroupRoot);
        }

        public void Interact(PlayerInteraction playerInteraction)
        {
            currentCreature.Talk();
        }
    }
}