using System;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay.UI
{
    public class UIManager : MonoBehaviour
    {
        [field : SerializeField] 
        public PlayerInteraction PlayerInteraction { get; private set; }

        private void Awake()
        {
            var childsUI = GetComponentsInChildren<IUI>();
            foreach (var ui in childsUI)
            {
                ui.UIManager = this;
            }
        }
    }
}