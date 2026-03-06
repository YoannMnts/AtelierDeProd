using System;
using System.Collections.Generic;
using Ozkaal.Core;
using UnityEngine;
using UnityEngine.UI;

public class AnswerUI : MonoBehaviour
{
    public event Action OnButtonClick; 
        
    private Creature currentCreature;
        
    private CreatureUI currentCreatureUI;
        
    private AnswerData answerData;

    private Dictionary<string, SymbolUI> symbols;
        
    private Button button;
        
    private bool isBadAnswer;

    private void Awake()
    {
        symbols = new();
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnClick);
    }

    public void Init(CreatureUI creatureUI, Creature creature, AnswerData answerData)
    {
        currentCreatureUI = creatureUI;
        currentCreature = creature;
        this.answerData = answerData;
        isBadAnswer = answerData.GainOrLossAmount <= 0;
    }

    private void OnClick()
    {
        currentCreature.AddOrRemoveFriendship(answerData.GainOrLossAmount);
        Debug.Log($"IsBadAnswer: {isBadAnswer}");
        if (isBadAnswer)
            currentCreature.StopTalking(30);
        else
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
                    PlayerController.Instance.Codex.DiscoverSymbol(symbolData.SymbolID);
                }
            }
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
                
            //A enlever !!!!!!!!!!
            PlayerController.Instance.PlayerControls.SwitchToUI(false);
            PlayerController.Instance.FreezePlayer(false);
            currentCreatureUI.ShowOrHideCanvas(true);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;

        }
    }
}