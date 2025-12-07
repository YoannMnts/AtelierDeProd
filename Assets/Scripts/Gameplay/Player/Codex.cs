using System.Collections.Generic;
using Ozkaal.Core.Core.Datas.SymbolDatas;
using UnityEngine;
using UnityEngine.Pool;

namespace Ozkaal.Gameplay.Gameplay.Player
{
    public class Codex
    {
        private Dictionary<string, CodexSymbol> symbols = new ();

        public Codex()
        {
            var entries = Resources.LoadAll<SymbolData>("ScriptableObject/CodexEntries");
            for (int i = 0; i < entries.Length; i++)
            {
                symbols.Add(entries[i].SymbolID, new CodexSymbol(entries[i], string.Empty, false));
            }
        }

        public void SetTranslation(string guid, string translation)
        {
            if (symbols.TryGetValue(guid, out CodexSymbol symbol))
            {
                symbol.SetTranslation(translation);
            }
        }

        public void DiscoverSymbol(string guid)
        {
            if (symbols.TryGetValue(guid, out CodexSymbol symbol))
            {
                symbol.Discover();
            }
        }

        public bool IsSymbolDiscovered(string guid)
        {
            if (symbols.TryGetValue(guid, out CodexSymbol symbol))
            {
                return symbol.IsDiscovered;
            }
            return false;
        }
        
        public bool TryGetCodexSymbol(string guid, out CodexSymbol symbol) => symbols.TryGetValue(guid, out symbol);
    }
}