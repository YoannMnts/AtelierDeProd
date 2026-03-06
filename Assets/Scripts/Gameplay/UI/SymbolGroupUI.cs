using System;
using System.Collections.Generic;
using Ozkaal.Core;
using UnityEngine;

public class SymbolGroupUI : MonoBehaviour
{
    public static SymbolGroupUI Main => UIManager.instance.SymbolGroupUI;
        
    public Codex CurrentCodex { get; private set; }
    public WorldSymbolGroup CurrentGroup { get; private set; }
        
        
    [SerializeField]
    private Transform root;
        
    [SerializeField]
    private SymbolUI prefab;
        
        
    private Dictionary<string, SymbolUI> symbols;
    
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        symbols = new Dictionary<string, SymbolUI>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        foreach (Transform t in root)
        {
            Destroy(t.gameObject);
        }
        HideCanvas();
    }

    private void OnEnable()
    {
        PlayerController.Instance.PlayerControls.Escape += EarlyDisconnect;
    }

    private void OnDisable()
    {
        PlayerController.Instance.PlayerControls.Escape -= EarlyDisconnect;
    }

    public void Connect(Codex codex, WorldSymbolGroup group)
    {
        if (CurrentCodex != null)
        {
            Disconnect(CurrentCodex);
        }

        ShowCanvas();
        CurrentCodex = codex;
        CurrentGroup = group;
        for (int i = 0; i < group.SymbolDatas.Length; i++)
        {
            SymbolUI instance = Instantiate(prefab, root);
            SymbolData symbolData = group.SymbolDatas[i];
            if (codex.TryGetCodexSymbol(symbolData.SymbolID, out CodexSymbol symbol))
            {
                instance.Connect(symbol);
                symbols[symbolData.SymbolID] = instance;
            }
        }
        PlayerController.Instance.PlayerControls.SwitchToUI(true);
        Cursor.visible = true;
    }

    private void ShowCanvas()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }


    private void EarlyDisconnect()
    {
        if (CurrentCodex is null)
        {
            return;
        }
        Disconnect(CurrentCodex);
    }
    public void Disconnect(Codex codex)
    {
        if (CurrentCodex != codex)
        {
            return;
        }
        foreach (var (guid, symbolUI) in symbols)
        {
            if (codex.TryGetCodexSymbol(guid, out var codexSymbol))
            {
                symbolUI.Disconnect(codexSymbol);
            }
        }
        foreach (Transform t in root)
        {
            Destroy(t.gameObject);
        }
        HideCanvas();
        PlayerController.Instance.PlayerControls.SwitchToUI(false);
        Cursor.visible = false;
    }

    private void HideCanvas()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}