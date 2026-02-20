using System;
using Ozkaal.Core.Datas.SymbolDatas;
using Ozkaal.Gameplay.Gameplay.Player;
using Ozkaal.Gameplay.Gameplay.UI;
using UnityEngine;
using Random = System.Random;

namespace Ozkaal.Gameplay.Gameplay.Interaction.Symbols
{
    public partial class WorldSymbolGroup : MonoBehaviour, IInteractable
    {
        public WorldSymbolGroup(SymbolData[] symbolDatas)
        {
            SymbolDatas = symbolDatas;
        }
        
        [field: SerializeField]
        public SymbolData[] SymbolDatas {get; private set;}

        private bool temp;
        public void Interact(PlayerInteraction playerInteraction)
        {
            temp = !temp;
            if (temp)
            {
                SymbolGroupUI.Main.Connect(playerInteraction.PlayerController.Codex, this);
                PlayerController.Instance.FreezePLayer(true);
            }
            else
            {
                SymbolGroupUI.Main.Disconnect(playerInteraction.PlayerController.Codex, this);
                PlayerController.Instance.FreezePLayer(false);
            }
            for (int i = 0; i < SymbolDatas.Length; i++)
            {
                playerInteraction.PlayerController.Codex.DiscoverSymbol(SymbolDatas[i].SymbolID);
                Debug.Log($"Data : {SymbolDatas[i]},IsDiscovered: {playerInteraction.PlayerController.Codex.IsSymbolDiscovered(SymbolDatas[i].SymbolID)}");
            }
        }

    }
}