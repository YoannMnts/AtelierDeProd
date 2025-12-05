using UnityEngine;

namespace Core.Datas.SymbolDatas
{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "Interactable/SymbolData", order = 0)]
    public class SymbolData : ScriptableObject
    {
        [field : SerializeField]
        public Sprite Icon { get; private set; }
        [field : SerializeField]
        public int CodexIndex { get; private set; }
    }
}