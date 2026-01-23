using Ozkaal.Core.Core.Datas.SymbolDatas;
using UnityEngine;

namespace Gameplay.Creature
{
    [CreateAssetMenu(fileName = "CreatureSymbolGroup", menuName = "Datas/Creature/SymbolGroup", order = 0)]
    public class CreatureSentencesData : ScriptableObject
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