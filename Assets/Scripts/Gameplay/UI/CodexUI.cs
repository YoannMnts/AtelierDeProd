using System;
using System.Collections.Generic;
using Ozkaal.Core.Core.Datas.SymbolDatas;
using Ozkaal.Gameplay.Gameplay.Player;
using UnityEngine;
using UnityEngine.Pool;

namespace Ozkaal.Gameplay.Gameplay.UI
{
    public class CodexUI : MonoBehaviour
    {
        [SerializeField]
        private SymbolUI prefab;
        [SerializeField]
        private Transform root;
        [SerializeField]
        private CanvasGroup canvasGroup;
        
        private List<CodexSymbol> symbols = new List<CodexSymbol>();
        
        public Codex CurrentCodex { get; private set; }
        
        private void Start()
        {
            foreach (Transform t in root)
            {
                Destroy(t.gameObject);
            }
        }
        public void Connect(Codex codex)
        {
            if (CurrentCodex != null)
            {
                Disconnect(CurrentCodex);
            }
            CurrentCodex = codex;
            CurrentCodex.GetAllDiscoveredSymbols(symbols);
            for (int i = 0; i < symbols.Count; i++)
            {
                SymbolUI instance = Instantiate(prefab, root);
                SymbolData symbolData = symbols[i].SymbolData;
                if (codex.TryGetCodexSymbol(symbolData.SymbolID, out CodexSymbol symbol))
                {
                    instance.Connect(symbol);
                }
                Debug.Log("boucle");
            }
            Debug.Log("aaaaaa");
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        public void Disconnect(Codex codex)
        {
            if (CurrentCodex != codex)
            {
                return;
            }
            foreach (Transform t in root)
            {
                Destroy(t.gameObject);
            }
            Debug.Log("bbbbbb");
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}