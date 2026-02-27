using System;
using System.Collections.Generic;
using Ozkaal.Core.Datas.SymbolDatas;
using Ozkaal.Gameplay.Gameplay.Player;
using Ozkaal.Gameplay.Gameplay.UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Gameplay.Creature.UI
{
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
            creature.OnTalk += OnCreatureTalk;
            creature.OnStopTalk += OnCreatureStopTalk;
        }

        private void Disconnect(Creature creature)
        {
            creature.OnTalk -= OnCreatureTalk;
            creature.OnStopTalk -= OnCreatureStopTalk;
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
            for (int i = 0; i < creature.CurrentCreatureQuestion.Answers.Length; i++)
            {
                AnswerUI answerInstance = Instantiate(answerPrefab, answersRoot);
                AnswerData answerData = creature.CurrentCreatureQuestion.Answers[i];
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
        }
    }
}