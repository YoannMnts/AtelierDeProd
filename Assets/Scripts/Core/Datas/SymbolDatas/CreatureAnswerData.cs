using UnityEngine;

namespace Ozkaal.Core.Datas.SymbolDatas
{
    [CreateAssetMenu(fileName = "CreatureSymbolGroup", menuName = "Datas/Creature/SymbolGroup", order = 0)]
    public class CreatureAnswerData : ScriptableObject
    {
        [field: SerializeField]
        public SymbolData[] SymbolDatas {get; private set;}
        
        [field: SerializeField]
        public int MinFriendshipAmount {get; private set;}
        
        [field: SerializeField]
        public int MaxFriendshipAmount {get; private set;}
        
        [field: SerializeField]
        public int GainOrLossAmount {get; private set;}
    }
}