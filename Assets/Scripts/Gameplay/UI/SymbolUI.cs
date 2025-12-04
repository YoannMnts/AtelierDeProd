using System;
using Gameplay.Interaction;
using Gameplay.Interaction.Symbols;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay.UI
{
    public class SymbolUI : MonoBehaviour, IUI
    {
        public UIManager UIManager { get; set; }

        private void OnEnable()
        {
            UIManager.PlayerInteraction.PlayerInteract += Check;
        }

        private void OnDisable()
        {
            UIManager.PlayerInteraction.PlayerInteract -= Check;
        }

        public void Check()
        {
            if (UIManager.PlayerInteraction.CurrentInteractable is Symbol)
            {
                Show();
            }
        }
        
        public void Show()
        {
            Debug.Log("Symbol UI Show");
        }

        public void Hide()
        {
            throw new System.NotImplementedException();
        }
    }
}