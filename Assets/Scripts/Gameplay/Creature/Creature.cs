using System;
using Ozkaal.Core;
using Ozkaal.Core.Datas.CreatureQuestionDatas;
using Ozkaal.Gameplay.Gameplay.Player;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class Creature
{ 
    public CreatureQuestionData CurrentCreatureQuestion { get; private set; }

    public event Action OnMaximumFriendshipReached;
    public event Action<Creature> OnCooldown;
    public event Action<Creature> OnGainFriendship;
    public event Action<Creature> OnLossFriendship;
    public event Action<Creature, Codex> OnTalk;
    public event Action<Creature, Codex> OnStopTalk;
        
        
    public bool IsAlreadyTalking {get; private set;}
        
    public bool IsInCooldown { get; private set; }
        
    private readonly CreatureQuestionData[] creatureQuestionDatas = Resources.LoadAll<CreatureQuestionData>("ScriptableObject/CreatureEntries");
        
    private int currentFriendshipAmount;
        
    private Codex currentCodex;
        
        
    public void Talk(Codex codex)
    {
        if (IsInCooldown)
        {
            OnCooldown?.Invoke(this);
            return;
        }
            
        if (IsAlreadyTalking)
        {
            StopTalking();
            PlayerController.Instance.FreezePlayer(false);
            return;
        }
        PlayerController.Instance.FreezePlayer(true);
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
        }
    }


    public void StopTalking(float waitTime = 0)
    {
        IsAlreadyTalking = false;
        OnStopTalk?.Invoke(this, currentCodex);
        _ = StartCreatureCooldown(waitTime);
    }

    private async Awaitable StartCreatureCooldown(float waitTime)
    {
        IsInCooldown = true;
        await Awaitable.WaitForSecondsAsync(waitTime);
        IsInCooldown = false;
    }

    public void AddOrRemoveFriendship(int amount)
    {
        currentFriendshipAmount += amount;
            
        if (currentFriendshipAmount >= 10)
            OnMaximumFriendshipReached?.Invoke();
                
        if (amount > 0)
            OnGainFriendship?.Invoke(this);
        else
            OnLossFriendship?.Invoke(this);
    }
}