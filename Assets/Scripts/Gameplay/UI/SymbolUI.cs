using System;
using Gameplay.Interaction;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay.UI
{
    public class SymbolUI : MonoBehaviour, IUI
    {
        public UIManager UIManager { get; set; }

        private void OnEnable()
        {
            UIManager.PlayerInteraction.OnInteract -= Hide;
            UIManager.PlayerInteraction.OnInteract += Show;
        }

        private void OnDisable()
        {
            UIManager.PlayerInteraction.OnInteract -= Show;
            UIManager.PlayerInteraction.OnInteract += Hide;
        }

        public void Show()
        {
            throw new System.NotImplementedException();
        }

        public void Hide()
        {
            throw new System.NotImplementedException();
        }
    }
}