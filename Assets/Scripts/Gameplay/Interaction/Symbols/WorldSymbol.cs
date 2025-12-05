using System;
using Core.Datas.SymbolDatas;
using Gameplay.Player;
using Gameplay.UI;
using UnityEngine;

namespace Gameplay.Interaction.Symbols
{
    public class WorldSymbol : MonoBehaviour
    {
        [field : SerializeField]
        public SymbolData SymbolData { get; private set; }
        
    }
}