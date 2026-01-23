using System;
using Ozkaal.Gameplay.Gameplay.Interaction;
using Ozkaal.Gameplay.Gameplay.Player;
using Ozkaal.Gameplay.Gameplay.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Creature
{
    public class WorldCreature : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private int numberOfSentence = 3;
        
        [SerializeField]
        private Transform symbolGroupRoot;
        
        private Creature currentCreature;
        
        private void Start()
        {
            currentCreature = new Creature(numberOfSentence);
        }

        public void Interact(PlayerInteraction playerInteraction)
        {
            currentCreature.Talk();
        }
    }
}