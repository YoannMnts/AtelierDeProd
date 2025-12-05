using System;
using Core.Datas.SymbolDatas;

namespace Gameplay.Player
{
    public class CodexSymbol
    {
        public event Action<CodexSymbol> OnTranslationChanged;
        public event Action<CodexSymbol> OnDiscovered;
        public SymbolData SymbolData { get; private set; }
        public string Translation { get; private set; }
        public bool IsDiscovered { get; private set; }
        
        public CodexSymbol(SymbolData symbolData, string translation, bool isDiscovered)
        {
            SymbolData = symbolData;
            Translation = translation;
            IsDiscovered = isDiscovered;
        }

        public void SetTranslation(string translation)
        {
            Translation = translation;
            Discover();
        }

        public void Discover()
        {
            IsDiscovered = true;
        }
    }
}