using System;
using System.Collections.Generic;
using Ozkaal.Core.Datas.CreatureQuestionDatas;
using Ozkaal.Core.Datas.SymbolDatas;
using Ozkaal.Gameplay.Gameplay.Player;
using Ozkaal.Gameplay.Gameplay.UI;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Gameplay.Creature
{
    public class Creature
    { 
        public Creature(int answerNumber)
        {
            this.numberOfAnswer = answerNumber;
        }

        public CreatureQuestionData CurrentCreatureQuestion { get; private set; }
        
        public event Action<Creature> OnGainFriendship;
        public event Action<Creature> OnLossFriendship;
        public event Action<Creature, Codex> OnTalk;
        public event Action<Creature, Codex> OnStopTalk;
        
        
        public bool IsAlreadyTalking {get; private set;}
        
        
        private readonly CreatureQuestionData[] creatureQuestionDatas = Resources.LoadAll<CreatureQuestionData>("ScriptableObject/CreatureEntries");
        
        private readonly int numberOfAnswer;
        
        private int currentFriendshipAmount;
        
        private Codex currentCodex;
        
        
        public void Talk(Codex codex)
        {
            if (IsAlreadyTalking)
            {
                StopTalking();
                return;
            }
            currentCodex = codex;
            using (ListPool<CreatureQuestionData>.Get(out var validCreatureQuestionDatas))
            {
                for (int i = 0; i < creatureQuestionDatas.Length; i++)
                {
                    int min = creatureQuestionDatas[i].MinFriendshipAmount;
                    int max = creatureQuestionDatas[i].MaxFriendshipAmount;
                    
                    if (currentFriendshipAmount >= min && currentFriendshipAmount <= max)
                    {
                        validCreatureQuestionDatas.Add(creatureQuestionDatas[i]);
                    }
                }
                int randomIndex = Random.Range(0, validCreatureQuestionDatas.Count);
                CurrentCreatureQuestion = validCreatureQuestionDatas[randomIndex];
                
                IsAlreadyTalking = true;
                OnTalk?.Invoke(this, currentCodex);
                Debug.Log("OnTalk finished");
            }
        }

        public void StopTalking()
        {
            IsAlreadyTalking = false;
            OnStopTalk?.Invoke(this, currentCodex);
        }

        public void AddOrRemoveFriendship(int amount)
        {
            currentFriendshipAmount += amount;
            if (amount > 0)
                OnGainFriendship?.Invoke(this);
            else
                OnLossFriendship?.Invoke(this);
        }
    }
}