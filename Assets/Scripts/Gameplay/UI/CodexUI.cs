using Ozkaal.Gameplay.Gameplay.Player;
using UnityEngine;

namespace Ozkaal.Gameplay.Gameplay.UI
{
    public class CodexUI
    {
        [SerializeField]
        private SymbolUI prefab;
        [SerializeField]
        private Transform root;
        
        public Codex CurrentCodex { get; private set; }
        
        public void Connect(Codex codex)
        {
            
        }
    }
}