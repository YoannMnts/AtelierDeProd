using System.Collections.Generic;
using DefaultNamespace;
using Ozkaal.Core;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class CreatureUI : MonoBehaviour
{
    public Transform QuestionsRoot => questionsRoot;
    public SymbolUI SymbolPrefab => symbolPrefab;
        
    public Codex Codex { get; private set; }

    [SerializeField] 
    private Transform questionsRoot;
        
    [SerializeField] 
    private Transform answersRoot;
        
    [SerializeField] 
    private AnswerUI answerPrefab;
        
    [SerializeField]
    private SymbolUI symbolPrefab;
    
    [SerializeField]
    private Image fillImage;
    
    [SerializeField]
    private CanvasGroup fillCanvasGroup;
        
    private Dictionary<string, SymbolUI> symbols;

    private void Awake()
    {
        symbols = new();
        foreach (Transform t in answersRoot)
        {
            Destroy(t.gameObject);
        }

        foreach (Transform t in questionsRoot)
        {
            Destroy(t.gameObject);
        }
        fillImage.fillAmount = 0;
        fillCanvasGroup.alpha = 0;
        fillCanvasGroup.interactable = false;
        fillCanvasGroup.blocksRaycasts = false;
    }

    private void OnEnable()
    {
        foreach (Creature creature in CreatureManager.Creatures) 
            Connect(creature);
        
        CreatureManager.OnCreatureCreated += Connect;
        CreatureManager.OnCreatureDestroyed += Disconnect;
    }

    private void OnDisable()
    {
        CreatureManager.OnCreatureCreated -= Connect;
        CreatureManager.OnCreatureDestroyed -= Disconnect;
            
        foreach (Creature creature in CreatureManager.Creatures)
            Disconnect(creature);
    }

    private void Connect(Creature creature)
    {
        Debug.Log($"Connecting to creature");
        creature.OnTalk += OnCreatureTalk;
        creature.OnStopTalk += OnCreatureStopTalk;
        creature.OnGainOrLossFriendship += AddOrRemoveFillImage;
        PlayerController.Instance.PlayerControls.Escape += creature.EarlyStopTalking;
    }

    private void Disconnect(Creature creature)
    {
        creature.OnTalk -= OnCreatureTalk;
        creature.OnStopTalk -= OnCreatureStopTalk;
        creature.OnGainOrLossFriendship -= AddOrRemoveFillImage;
        PlayerController.Instance.PlayerControls.Escape -= creature.EarlyStopTalking;
    }

    private void OnCreatureTalk(Creature creature,Codex codex)
    {
        Codex = codex;
        for (int i = 0; i < creature.CurrentCreatureQuestion.Question.Length; i++)
        {
            SymbolUI instance = Instantiate(symbolPrefab, questionsRoot);
            SymbolData symbolData = creature.CurrentCreatureQuestion.Question[i];
            if (codex.TryGetCodexSymbol(symbolData.SymbolID, out CodexSymbol symbol))
            {
                instance.Connect(symbol);
                symbols[symbolData.SymbolID] = instance;
                PlayerController.Instance.Codex.DiscoverSymbol(symbolData.SymbolID);
            }
        }
        using (ListPool<int>.Get(out var validIndex))
        {
            for (int i = 0; i < creature.CurrentCreatureQuestion.Answers.Length; i++)
            {
                AnswerUI answerInstance = Instantiate(answerPrefab, answersRoot);

                int lenght = creature.CurrentCreatureQuestion.Answers.Length;
                GetValidRandomIndex(lenght, validIndex);


                AnswerData answerData = creature.CurrentCreatureQuestion.Answers[validIndex[i]];
                answerInstance.Init(this, creature, answerData);

                Transform symbolRoot = answerInstance.transform;
                for (int j = 0; j < answerData.AnswerDatas.Length; j++)
                {
                    SymbolUI instance = Instantiate(symbolPrefab, symbolRoot);
                    SymbolData symbolData = answerData.AnswerDatas[j];
                    if (codex.TryGetCodexSymbol(symbolData.SymbolID, out CodexSymbol symbol))
                    {
                        instance.Connect(symbol);
                        symbols[symbolData.SymbolID] = instance;
                        PlayerController.Instance.Codex.DiscoverSymbol(symbolData.SymbolID);
                    }
                }
            }
        }
        fillCanvasGroup.alpha = 1;
        fillCanvasGroup.interactable = true;
        fillCanvasGroup.blocksRaycasts = true;
    }

    private static void GetValidRandomIndex(int lenght, List<int> validIndex)
    {
        int randomIndex = Random.Range(0, lenght);
        if (!validIndex.Contains(randomIndex))
        {
            validIndex.Add(randomIndex);
        }
        else
        {
            GetValidRandomIndex(lenght, validIndex);
        }
    }

    private void OnCreatureStopTalk(Creature creature, Codex codex)
    {
        foreach (var (guid, symbolUI) in symbols)
        {
            if (codex.TryGetCodexSymbol(guid, out var codexSymbol))
            {
                symbolUI.Disconnect(codexSymbol);
            }
        }
        foreach (Transform t in answersRoot)
        {
            Destroy(t.gameObject);
        }

        foreach (Transform t in questionsRoot)
        {
            Destroy(t.gameObject);
        }
        fillCanvasGroup.alpha = 0;
        fillCanvasGroup.interactable = false;
        fillCanvasGroup.blocksRaycasts = false;
    }

    private void AddOrRemoveFillImage(int amount)
    {
        Debug.Log($"Adding amount {amount}");
        float fillAmount = amount / 11f;
        Debug.Log($"Adding fill amount {fillAmount}");
        fillImage.fillAmount = Mathf.Clamp01(fillImage.fillAmount + fillAmount);
    }
}