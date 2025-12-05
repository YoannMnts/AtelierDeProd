using System;
using UnityEngine;

namespace Core.Datas.SymbolDatas
{
    [CreateAssetMenu(fileName = "SymbolData", menuName = "Interactable/SymbolData", order = 0)]
    public class SymbolData : ScriptableObject
    {
        [field : SerializeField]
        public GameObject Prefab { get; private set; }
        [field : SerializeField]
        public Sprite Icon { get; private set; }
        public string SymbolID { get; private set; }

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(SymbolID))
            {
                SymbolID = Guid.NewGuid().ToString();
            }
        }
    }
}