using System;
using Ozkaal.Core.Core.Datas.SymbolDatas;
using Ozkaal.Gameplay.Gameplay.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ozkaal.Gameplay.Gameplay.UI
{
    //Make for POC :
    //CreateUI method
    public class SymbolUI : MonoBehaviour
    {
        public CodexSymbol CodexSymbol { get; private set; }
        
        [SerializeField]
        private TMP_InputField inputField;
        [SerializeField]
        private Image iconImage;

        public void Connect(CodexSymbol symbol)
        {
            if (CodexSymbol != null)
            {
                Disconnect(CodexSymbol);
            }
            CodexSymbol = symbol;
            CodexSymbol.OnTranslationChanged += Check;
            UpdateUI(CodexSymbol);
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

        private void UpdateUI(CodexSymbol codexSymbol)
        {
            iconImage.sprite = codexSymbol.SymbolData.Icon;
            inputField.text = codexSymbol.Translation;
        }
        
        public void OnEndEdit(string translation)
        {
            CodexSymbol.SetTranslation(translation);
            Debug.Log(translation);
        }
        
    }
}
