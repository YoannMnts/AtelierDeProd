using System;
using Gameplay.Interaction;
using Gameplay.Interaction.Symbols;
using Gameplay.Player;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay.UI
{
    public class SymbolUI : MonoBehaviour, IUIElement
    {
        [HideInInspector] 
        public UIManager UIManager { get; set; }
        public CodexSymbol CodexSymbol { get; private set; }

        public void Connect(CodexSymbol symbol)
        {
            if (CodexSymbol != null)
            {
                Disconnect(CodexSymbol);
            }
            CodexSymbol = symbol;
            CodexSymbol.OnTranslationChanged += Check;
        }


        public void Disconnect(CodexSymbol symbol)
        {
            if (CodexSymbol == symbol)
            {
                CodexSymbol.OnTranslationChanged -= Check;
                CodexSymbol = null;
            }
        }
        
        public void Check()
        {
            throw new NotImplementedException();
        }

        public void Show()
        {
            UIManager.canvasGroup.alpha = 1;
        }

        public void Hide()
        {
            UIManager.canvasGroup.alpha = 0;
        }
        private void Check(CodexSymbol obj)
        {
            throw new NotImplementedException();
        }
    }
}
