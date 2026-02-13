using UnityEngine;

namespace Ozkaal.Core.Datas.SymbolDatas
{
    [CreateAssetMenu(fileName = "AnswerData", menuName = "Datas/Answer", order = 0)]
    public class AnswerData : ScriptableObject
    {
        [field: SerializeField]
        public SymbolData[] SymbolDatas {get; private set;}
        
        [field: SerializeField]
        public int GainOrLossAmount {get; private set;}
    }
}