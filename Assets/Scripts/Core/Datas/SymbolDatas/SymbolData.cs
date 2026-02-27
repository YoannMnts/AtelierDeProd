using System;
using UnityEditor;
using UnityEngine;

namespace Ozkaal.Core
{
    [CreateAssetMenu(fileName = "SymbolData", menuName = "Datas/SymbolData", order = 0)]
    public class SymbolData : ScriptableObject
    {
        [field : SerializeField]
        public int CodexIndex { get; private set; }
        
        [field : SerializeField]
        public Sprite Icon { get; private set; }
        
        [field: SerializeField, HideInInspector]
        public string SymbolID { get; private set; }

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(SymbolID))
            {
                GenerateNewGuid();
            }
            
#if UNITY_EDITOR
             string[] existings = AssetDatabase.FindAssets($"t:{nameof(SymbolData)}");
             for (int i = 0; i < existings.Length; i++)
             {
                 var path = AssetDatabase.GUIDToAssetPath(existings[i]);
                 var asset = AssetDatabase.LoadAssetAtPath<SymbolData>(path);
                 if(asset != this && asset.SymbolID == SymbolID)
                     GenerateNewGuid();
             }
#endif
        }

        private void GenerateNewGuid()
        {
            SymbolID = Guid.NewGuid().ToString();
        }
    }
}