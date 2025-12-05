using System;
using Gameplay.Player;
using Gameplay.UI;
using UnityEngine;

namespace Gameplay.Interaction.Symbols
{
    public class WorldSymbolGroup : MonoBehaviour, IInteractable
    {
        public WorldSymbol[] Symbols {get; private set;}
        public void Interact(PlayerInteraction playerInteraction)
        {
            SymbolGroupUI.Main.Connect(playerInteraction.playerController.Codex, this);
        }

        private void Start()
        {
            Symbols = GetComponentsInChildren<WorldSymbol>();
        }
    }
}