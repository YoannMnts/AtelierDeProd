using System;
using System.Collections.Generic;
using Ozkaal.Core.Core.Datas.SymbolDatas;
using Ozkaal.Gameplay.Gameplay.Player;
using UnityEngine;
using UnityEngine.Pool;

namespace Ozkaal.Gameplay.Gameplay.UI
{
    //Make for POC :
    //Connect + Disconnect + CurrentCodex properties
    public class CodexUI : MonoBehaviour
    {
        [SerializeField]
        private SymbolUI prefab;
        [SerializeField]
        private Transform root;
        [SerializeField]
        private CanvasGroup canvasGroup;
        
        public Codex CurrentCodex { get; private set; }
        
        private void Start()
        {
            foreach (Transform t in root)
            {
                Destroy(t.gameObject);
            }
        }
        public void Connect(Codex codex,Dictionary<string, CodexSymbol> symbols)
        {
            if (CurrentCodex != null)
            {
                Disconnect(CurrentCodex);
            }
            CurrentCodex = codex;
            foreach ((string guid, CodexSymbol codexSymbol) in symbols)
            {
                if (!CurrentCodex.IsSymbolDiscovered(guid))
                {
                    continue;
                }
                SymbolUI instance = Instantiate(prefab, root);
                instance.Connect(codexSymbol);
            }
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
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}