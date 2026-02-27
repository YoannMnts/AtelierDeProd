using UnityEngine;

namespace Ozkaal.Core
{
    [CreateAssetMenu(fileName = "AnswerData", menuName = "Datas/Answer", order = 0)]
    public class AnswerData : ScriptableObject
    {
        [field: SerializeField]
        public SymbolData[] AnswerDatas {get; private set;}
        
        [field: SerializeField]
        public SymbolData[] CreatureAnswerDatas {get; private set;}
        
        [field: SerializeField]
        public int GainOrLossAmount {get; private set;}
    }
}