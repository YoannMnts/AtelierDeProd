using Ozkaal.Core.Datas.SymbolDatas;
using Unity.Plastic.Newtonsoft.Json.Serialization;
using UnityEngine;

namespace Gameplay.Creature.UI
{
    public class AnswerUI : MonoBehaviour
    {
        public event Action OnButtonClick; 
        
        private Creature currentCreature;
        
        private AnswerData answerData;

        public void Init(Creature creature, AnswerData answerData)
        {
            currentCreature = creature;
            this.answerData = answerData;
        }
        
        public void OnClick()
        {
            Debug.Log($"Add : {answerData.GainOrLossAmount} to friendship");
            currentCreature.AddOrRemoveFriendship(answerData.GainOrLossAmount);
            OnButtonClick?.Invoke();
            currentCreature.StopTalking();
        }
    }
}