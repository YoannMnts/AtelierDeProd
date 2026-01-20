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
            List<WorldCreatureSymbolGroup> validCreatureSymbolGroups = ListPool<WorldCreatureSymbolGroup>.Get();
            try
            {
                for (int i = 0; i < creatureSymbolGroupDatas.Length; i++)
                {
                    int min = creatureSymbolGroupDatas[i].MinFriendshipAmount;
                    int max = creatureSymbolGroupDatas[i].MaxFriendshipAmount;
                    
                    if (currentFriendshipAmount > min && currentFriendshipAmount < max)
                    {
                        WorldCreatureSymbolGroup validCreatureSymbolGroup = new(creatureSymbolGroupDatas[i]);
                        validCreatureSymbolGroups.Add(validCreatureSymbolGroup);
                    }
                }

                for (int i = 0; i < numberOfSentence; i++)
                {
                    int randomIndex = Random.Range(0, validCreatureSymbolGroups.Count);
                    Debug.Log($"I talk : {validCreatureSymbolGroups[randomIndex]}");
                    //instantiate the sentence
                }
                OnTalk?.Invoke();
            }
            finally
            {
                ListPool<WorldCreatureSymbolGroup>.Release(validCreatureSymbolGroups);
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