using System;
using System.Collections.Generic;
using Core.Datas.SymbolDatas;
using Gameplay.Interaction.Symbols;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay.UI
{
    public class SymbolGroupUI : MonoBehaviour
    {

        public static SymbolGroupUI Main => UIManager.instance.SymbolGroupUI;
        
        public Codex CurrentCodex { get; private set; }
        public WorldSymbolGroup CurrentGroup { get; private set; }
        [SerializeField]
        private Transform root;
        [SerializeField]
        private SymbolUI prefab;
        private Dictionary<string, SymbolUI> symbols;

        private void Awake()
        {
            symbols = new Dictionary<string, SymbolUI>();
        }

        private void Start()
        {
            foreach (Transform t in root)
            {
                Destroy(t.gameObject);
            }
        }

        public void Connect(Codex codex, WorldSymbolGroup group)
        {
            if (CurrentCodex != null)
            {
                Disconnect(CurrentCodex, CurrentGroup);
            }
            CurrentCodex = codex;
            CurrentGroup = group;
            for (int i = 0; i < group.Symbols.Length; i++)
            {
                SymbolUI instance = Instantiate(prefab, root);
                SymbolData symbolData = group.Symbols[i].SymbolData;
                if (codex.TryGetCodexSymbol(symbolData.SymbolID, out CodexSymbol symbol))
                {
                    instance.Connect(symbol);
                    symbols[symbolData.SymbolID] = instance;
                }
            }
            
        }
        
        public void Disconnect(Codex codex, WorldSymbolGroup group)
        {
            if (CurrentCodex != codex)
            {
                return;
            }
            foreach (var (guid, symbolUI) in symbols)
            {
                if (codex.TryGetCodexSymbol(guid, out var codexSymbol))
                {
                    symbolUI.Disconnect(codexSymbol);
                }
            }
            foreach (Transform t in root)
            {
                Destroy(t.gameObject);
            }
        }
    }
}