using DefaultNamespace;
using Ozkaal.Core;
using UnityEngine;

public partial class WorldSymbolGroup : MonoBehaviour, IInteractable
{
    int IInteractable.Priority => 2;
        
    [SerializeField]
    private InteractableOutline outline;

    [field: SerializeField]
    public SymbolData[] SymbolDatas {get; private set;}

    private bool temp;

    void IInteractable.Interact(PlayerInteraction playerInteraction)
    {
        SymbolGroupUI.Main.Connect(playerInteraction.PlayerController.Codex, this);
        PlayerController.Instance.FreezePlayer(true);
        PlayerController.Instance.PlayerControls.SwitchToUI(true);
        for (int i = 0; i < SymbolDatas.Length; i++)
        {
            playerInteraction.PlayerController.Codex.DiscoverSymbol(SymbolDatas[i].SymbolID);
            Debug.Log($"Data : {SymbolDatas[i]},IsDiscovered: {playerInteraction.PlayerController.Codex.IsSymbolDiscovered(SymbolDatas[i].SymbolID)}");
        }
    }

    
    void IInteractable.OnEnter(PlayerInteraction playerInteraction)
    {
        outline.Show();
    }
    
    void IInteractable.OnExit(PlayerInteraction playerInteraction)
    {
        outline.Hide();
    }

}