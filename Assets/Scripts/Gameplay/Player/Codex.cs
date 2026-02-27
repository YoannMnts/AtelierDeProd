using System.Collections.Generic;
using Ozkaal.Core;
using UnityEngine;

//Make for POC :
//Properties for CodexSymbol Dictionary + ConnectToUI + temp
public class Codex
{
    public bool TryGetCodexSymbol(string guid, out CodexSymbol symbol) => symbols.TryGetValue(guid, out symbol);
        
        
    private readonly Dictionary<string, CodexSymbol> symbols;
        
    public Codex()
    {
        symbols = new Dictionary<string, CodexSymbol>();
        var entries = Resources.LoadAll<SymbolData>("ScriptableObject/CodexEntries");
        for (int i = 0; i < entries.Length; i++)
        {
            SymbolData entry = entries[i];
            CodexSymbol codexSymbol = new CodexSymbol(entry, string.Empty, false);
            symbols.Add(entry.SymbolID, codexSymbol);
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
}