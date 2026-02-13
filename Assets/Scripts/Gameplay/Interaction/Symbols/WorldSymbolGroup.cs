using System;
using Ozkaal.Gameplay.Gameplay.Player;
using Ozkaal.Gameplay.Gameplay.UI;
using UnityEngine;
using Random = System.Random;

namespace Ozkaal.Gameplay.Gameplay.Interaction.Symbols
{
    public partial class WorldSymbolGroup : MonoBehaviour, IInteractable
    {
        public WorldSymbolGroup(WorldSymbol[] symbols)
        {
            Symbols = symbols;
        }
        public WorldSymbol[] Symbols {get; private set;}

        private bool temp;
        private void Start()
        {
            Symbols = GetComponentsInChildren<WorldSymbol>();
        }
        public void Interact(PlayerInteraction playerInteraction)
        {
            temp = !temp;
            if (temp)
            {
                SymbolGroupUI.Main.Connect(playerInteraction.PlayerController.Codex, this);
            }
            else
            {
                SymbolGroupUI.Main.Disconnect(playerInteraction.PlayerController.Codex, this);
            }
            for (int i = 0; i < Symbols.Length; i++)
            {
                playerInteraction.PlayerController.Codex.DiscoverSymbol(Symbols[i].SymbolData.SymbolID);
                Debug.Log($"Data : {Symbols[i].SymbolData},IsDiscovered: {playerInteraction.PlayerController.Codex.IsSymbolDiscovered(Symbols[i].SymbolData.SymbolID)}");
                
            }
        }

    }
}