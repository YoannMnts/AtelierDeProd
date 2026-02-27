using System;
using System.Collections.Generic;
using Ozkaal.Core.Datas.SymbolDatas;
using Ozkaal.Gameplay.Gameplay.Player;
using Ozkaal.Gameplay.Gameplay.UI;
using UnityEngine;
using Action = Unity.Plastic.Newtonsoft.Json.Serialization.Action;

namespace Gameplay.Creature.UI
{
    public class AnswerUI : MonoBehaviour
    {
        public event Action OnButtonClick; 
        
        private Creature currentCreature;
        
        private CreatureUI currentCreatureUI;
        
        private AnswerData answerData;

        private Dictionary<string, SymbolUI> symbols;
        public void Init(CreatureUI creatureUI, Creature creature, AnswerData answerDatas)
        {
            currentCreature = creature;
            answerData = answerDatas;
            currentCreatureUI = creatureUI;
        }
        
        public void OnClick()
        {
            Debug.Log($"Add : {answerData.GainOrLossAmount} to friendship");
            currentCreature.AddOrRemoveFriendship(answerData.GainOrLossAmount);
            currentCreature.StopTalking();
            _ = CreatureAnswered();
            OnButtonClick?.Invoke();
        }

        private async Awaitable CreatureAnswered()
        {
            try
            {
                for (int i = 0; i < answerData.CreatureAnswerDatas.Length; i++)
                {
                    SymbolUI instance = Instantiate(currentCreatureUI.SymbolPrefab, currentCreatureUI.QuestionsRoot);
                    SymbolData symbolData = answerData.CreatureAnswerDatas[i];
                    if (currentCreatureUI.Codex.TryGetCodexSymbol(symbolData.SymbolID, out CodexSymbol symbol))
                    {
                        instance.Connect(symbol);
                        symbols[symbolData.SymbolID] = instance;
                        Debug.Log("AAAAAAAAA");
                        PlayerController.Instance.Codex.DiscoverSymbol(symbolData.SymbolID);
                    }
                }
                /*
                await Awaitable.WaitForSecondsAsync(5);
                foreach (var (guid, symbolUI) in symbols)
                {
                    if (currentCreatureUI.Codex.TryGetCodexSymbol(guid, out var codexSymbol))
                    {
                        symbolUI.Disconnect(codexSymbol);
                    }
                }
                foreach (Transform t in currentCreatureUI.QuestionsRoot)
                {
                    Destroy(t.gameObject);
                }
                */
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}