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

        private Codex currentCodex;

        private void OnEnable()
        {
            PlayerController.Instance.PlayerControls.OpenCodex += Connect;
            PlayerController.Instance.PlayerControls.CloseCodex += Disconnect;
        }

        private void OnDisable()
        {
            PlayerController.Instance.PlayerControls.OpenCodex -= Connect;
            PlayerController.Instance.PlayerControls.CloseCodex -= Disconnect;
        }

        private void Start()
        {
            foreach (Transform t in root)
            {
                Destroy(t.gameObject);
            }
        }
        public void Connect(Codex codex)
        {
            Debug.Log($"Connecting to {codex}");
            if (currentCodex != null)
            {
                Disconnect(currentCodex);
            }
            currentCodex = codex;
            var entries = Resources.LoadAll<SymbolData>("ScriptableObject/CodexEntries");
            for (int i = 0; i < entries.Length; i++)
            {
                string guid = entries[i].SymbolID;
                if (!currentCodex.IsSymbolDiscovered(guid))
                    continue;
                SymbolUI instance = Instantiate(prefab, root);
                currentCodex.TryGetCodexSymbol(guid, out CodexSymbol codexSymbol);
                instance.Connect(codexSymbol);
            }
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        public void Disconnect(Codex codex)
        {
            if (currentCodex != codex)
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