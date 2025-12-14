using Ozkaal.Core.Core.Datas.SymbolDatas;
using UnityEngine;

namespace Ozkaal.Gameplay.Gameplay.Interaction.Symbols
{
    public class WorldSymbol : MonoBehaviour
    {
        [field : SerializeField]
        public SymbolData SymbolData { get; private set; }
        
    }
}