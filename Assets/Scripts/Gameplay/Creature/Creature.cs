using System;
using System.Collections.Generic;
using Ozkaal.Gameplay.Gameplay.UI;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Gameplay.Creature
{
    public class Creature
    {
        public Creature(int sentencesNumber)
        {
            this.numberOfSentence = sentencesNumber;
        }
        public event Action OnGainFriendship;
        public event Action OnLossFriendship;
        public event Action<List<CreatureSentencesData>> OnTalk;
        
        private readonly CreatureSentencesData[] creatureSentencesDatas = Resources.LoadAll<CreatureSentencesData>("ScriptableObject/CreatureEntries");
        
        private readonly int numberOfSentence;
        
        private int currentFriendshipAmount;
        
        private List<CreatureSentencesData> currentCreatureSentences = new();
        
        public void Talk()
        {
            List<CreatureSentencesData> validCreatureSentencesDatas = ListPool<CreatureSentencesData>.Get();
            try
            {
                for (int i = 0; i < creatureSentencesDatas.Length; i++)
                {
                    int min = creatureSentencesDatas[i].MinFriendshipAmount;
                    int max = creatureSentencesDatas[i].MaxFriendshipAmount;
                    
                    if (currentFriendshipAmount >= min && currentFriendshipAmount <= max)
                    {
                        validCreatureSentencesDatas.Add(creatureSentencesDatas[i]);
                    }
                }
                
                for (int i = 0; i < numberOfSentence; i++)
                {
                    int randomIndex = Random.Range(0, validCreatureSentencesDatas.Count);
                    currentCreatureSentences.Add(validCreatureSentencesDatas[randomIndex]);
                    
                    //Debug pour la demo
                    Debug.Log($"Answer {randomIndex}, Number of symbol: {validCreatureSentencesDatas[randomIndex].SymbolDatas.Length}");
                    for (int j = 0; j < validCreatureSentencesDatas[randomIndex].SymbolDatas.Length; j++)
                    {
                        Debug.Log($"{validCreatureSentencesDatas[randomIndex].SymbolDatas[j].name}");
                    }
                }
                OnTalk?.Invoke(currentCreatureSentences);
            }
            finally
            {
                Debug.Log("OnTalk finished");
                ListPool<CreatureSentencesData>.Release(validCreatureSentencesDatas);
            }
        }

        public void AddOrRemoveFriendship(int amount)
        {
            currentFriendshipAmount += amount;
            if (amount > 0)
                OnGainFriendship?.Invoke();
            else
                OnLossFriendship?.Invoke();
        }
    }
}