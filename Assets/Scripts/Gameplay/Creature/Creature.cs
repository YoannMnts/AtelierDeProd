using System;
using System.Collections.Generic;
using Ozkaal.Core.Datas.SymbolDatas;
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
        
        public readonly List<CreatureAnswerData> currentCreatureAnswers = new();
        
        public event Action<Creature> OnGainFriendship;
        public event Action<Creature> OnLossFriendship;
        public event Action<Creature> OnTalk;
        public event Action<Creature> OnStopTalk;
        
        
        public bool IsAlreadyTalking {get; private set;}
        
        
        private readonly CreatureAnswerData[] creatureAnswerDatas = Resources.LoadAll<CreatureAnswerData>("ScriptableObject/CreatureEntries");
        
        private readonly int numberOfAnswer;
        
        
        private int currentFriendshipAmount;
        
        
        public void Talk()
        {
            using (ListPool<CreatureAnswerData>.Get(out var validCreatureAnswerDatas))
            {
                for (int i = 0; i < creatureAnswerDatas.Length; i++)
                {
                    int min = creatureAnswerDatas[i].MinFriendshipAmount;
                    int max = creatureAnswerDatas[i].MaxFriendshipAmount;
                    
                    if (currentFriendshipAmount >= min && currentFriendshipAmount <= max)
                    {
                        validCreatureAnswerDatas.Add(creatureAnswerDatas[i]);
                    }
                }
                
                int maxCount = Mathf.Min(numberOfAnswer, validCreatureAnswerDatas.Count);
                for (int i = 0; i < maxCount; i++)
                {
                    int randomIndex = Random.Range(0, validCreatureAnswerDatas.Count);
                    currentCreatureAnswers.Add(validCreatureAnswerDatas[randomIndex]);
                    
                    //Debug pour la demo
                    Debug.Log($"Answer {randomIndex}, Number of symbol: {validCreatureAnswerDatas[randomIndex].SymbolDatas.Length}");
                    for (int j = 0; j < validCreatureAnswerDatas[randomIndex].SymbolDatas.Length; j++)
                    {
                        Debug.Log($"{validCreatureAnswerDatas[randomIndex].SymbolDatas[j].name}");
                    }
                }
                IsAlreadyTalking = true;
                OnTalk?.Invoke(this);
                Debug.Log("OnTalk finished");
            }
        }

        public void StopTalking()
        {
            IsAlreadyTalking = false;
            OnStopTalk?.Invoke(this);
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