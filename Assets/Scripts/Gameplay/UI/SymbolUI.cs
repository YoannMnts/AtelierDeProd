using System;
using Ozkaal.Gameplay.Gameplay.Player;
using TMPro;
using UnityEngine;

namespace Ozkaal.Gameplay.Gameplay.UI
{
    public class SymbolUI : MonoBehaviour, IUIElement
    {
        [HideInInspector] 
        public UIManager UIManager { get; set; }
        public CodexSymbol CodexSymbol { get; private set; }
        
        [SerializeField]
        private TMP_InputField inputField;
        
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
        private void Check(CodexSymbol obj)
        {
            inputField.text = obj.Translation;
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

        public void OnEndEdit(string translation)
        {
            CodexSymbol.SetTranslation(translation);
            Debug.Log(translation);
        }
        
    }
}
