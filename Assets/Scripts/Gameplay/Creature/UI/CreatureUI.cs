using System;
using System.Collections.Generic;
using Ozkaal.Core.Datas.SymbolDatas;
using Ozkaal.Gameplay.Gameplay.Player;
using Ozkaal.Gameplay.Gameplay.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Creature.UI
{
    public class CreatureUI : MonoBehaviour
    {
        [SerializeField] 
        private Transform questionsRoot;
        
        [SerializeField] 
        private Transform answersRoot;
        
        [SerializeField] 
        private Transform answerPrefab;
        
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
            for (int i = 0; i < creature.CurrentCreatureQuestion.Question.Length; i++)
            {
                SymbolUI instance = Instantiate(symbolPrefab, questionsRoot);
                SymbolData symbolData = creature.CurrentCreatureQuestion.Question[i];
                if (codex.TryGetCodexSymbol(symbolData.SymbolID, out CodexSymbol symbol))
                {
                    instance.Connect(symbol);
                    symbols[symbolData.SymbolID] = instance;
                }
            }
            for (int i = 0; i < creature.CurrentCreatureQuestion.Answers.Length; i++)
            {
                Transform answerInstance = Instantiate(answerPrefab, answersRoot);
                for (int j = 0; j < creature.CurrentCreatureQuestion.Answers[i].SymbolDatas.Length; j++)
                {
                    SymbolUI instance = Instantiate(symbolPrefab, answerInstance);
                    SymbolData symbolData = creature.CurrentCreatureQuestion.Answers[i].SymbolDatas[j];
                    if (codex.TryGetCodexSymbol(symbolData.SymbolID, out CodexSymbol symbol))
                    {
                        instance.Connect(symbol);
                        symbols[symbolData.SymbolID] = instance;
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