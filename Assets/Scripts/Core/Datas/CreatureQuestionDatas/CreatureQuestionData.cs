using Ozkaal.Core.Datas.SymbolDatas;
using UnityEngine;

namespace Ozkaal.Core.Datas.CreatureQuestionDatas
{
    [CreateAssetMenu(fileName = "CreatureQuestion", menuName = "Datas/Creature/Question", order = 0)]
    public class CreatureQuestionData : ScriptableObject
    {
        [field: SerializeField]
        public AnswerData[] Answers { get; private set; }
        
        [field: SerializeField]
        public SymbolData[] Question { get; private set; }
        
        [field: SerializeField]
        public int MinFriendshipAmount {get; private set;}
        
        [field: SerializeField]
        public int MaxFriendshipAmount {get; private set;}
    }
}