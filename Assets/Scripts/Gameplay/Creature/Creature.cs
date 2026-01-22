using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Gameplay.Creature
{
    public class Creature
    {
        public Creature(int NumberOfSentence, Transform SentencesRoot)
        {
            creatureSymbolGroupDatas = Resources.LoadAll<CreatureSymbolGroupData>("ScriptableObject/CreatureEntries");
            numberOfSentence = NumberOfSentence;
            sentencesRoot = SentencesRoot;
        }
        
        public event Action OnGainFriendship;
        public event Action OnLossFriendship;
        public event Action OnTalk;
        
        private readonly CreatureSymbolGroupData[] creatureSymbolGroupDatas;
        
        private readonly int numberOfSentence;
        
        private readonly Transform sentencesRoot;
        
        private int currentFriendshipAmount;
        
        public void Talk()
        {
            List<CreatureSymbolGroupData> validCreatureSymbolGroupDatas = ListPool<CreatureSymbolGroupData>.Get();
            try
            {
                for (int i = 0; i < creatureSymbolGroupDatas.Length; i++)
                {
                    int min = creatureSymbolGroupDatas[i].MinFriendshipAmount;
                    int max = creatureSymbolGroupDatas[i].MaxFriendshipAmount;
                    
                    if (currentFriendshipAmount >= min && currentFriendshipAmount <= max)
                    {
                        validCreatureSymbolGroupDatas.Add(creatureSymbolGroupDatas[i]);
                    }
                }
                
                for (int i = 0; i < numberOfSentence; i++)
                {
                    int randomIndex = Random.Range(0, validCreatureSymbolGroupDatas.Count);
                    Debug.Log($"Answer {randomIndex}, Number of symbol: {validCreatureSymbolGroupDatas[randomIndex].SymbolDatas.Length}");
                    for (int j = 0; j < validCreatureSymbolGroupDatas[randomIndex].SymbolDatas.Length; j++)
                    {
                        Debug.Log($"{validCreatureSymbolGroupDatas[randomIndex].SymbolDatas[j].name}");
                    }
                    //instantiate the sentence
                }
                OnTalk?.Invoke();
            }
            finally
            {
                Debug.Log("OnTalk finished");
                ListPool<CreatureSymbolGroupData>.Release(validCreatureSymbolGroupDatas);
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