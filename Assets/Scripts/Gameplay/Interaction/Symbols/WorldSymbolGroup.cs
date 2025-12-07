using Ozkaal.Gameplay.Gameplay.Player;
using Ozkaal.Gameplay.Gameplay.UI;
using UnityEngine;

namespace Ozkaal.Gameplay.Gameplay.Interaction.Symbols
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