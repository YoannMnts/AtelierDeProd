using System;
using Ozkaal.Gameplay.Gameplay.Player;
using Ozkaal.Gameplay.Gameplay.UI;
using UnityEngine;

namespace Ozkaal.Gameplay.Gameplay.Interaction.Symbols
{
    public partial class WorldSymbolGroup : MonoBehaviour, IInteractable
    {
        public WorldSymbol[] Symbols {get; private set;}

        private bool temp;
        public void Interact(PlayerInteraction playerInteraction)
        {
            temp = !temp;
            if (temp)
            {
                SymbolGroupUI.Main.Connect(playerInteraction.playerController.Codex, this);
            }
            else
            {
                SymbolGroupUI.Main.Disconnect(playerInteraction.playerController.Codex, this);
            }
            for (int i = 0; i < Symbols.Length; i++)
            {
                playerInteraction.playerController.Codex.DiscoverSymbol(Symbols[i].SymbolData.SymbolID);
                Debug.Log($"Data : {Symbols[i].SymbolData},IsDiscovered: {playerInteraction.playerController.Codex.IsSymbolDiscovered(Symbols[i].SymbolData.SymbolID)}");
                
            }
        }

        private void Start()
        {
            Symbols = GetComponentsInChildren<WorldSymbol>();
        }
    }
}