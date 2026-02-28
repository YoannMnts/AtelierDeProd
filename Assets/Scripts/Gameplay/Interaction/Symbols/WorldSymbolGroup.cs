using Ozkaal.Core;
using UnityEngine;

public partial class WorldSymbolGroup : MonoBehaviour, IInteractable
{
    public WorldSymbolGroup(SymbolData[] symbolDatas)
    {
        SymbolDatas = symbolDatas;
    }
        
    [field: SerializeField]
    public SymbolData[] SymbolDatas {get; private set;}

    private bool temp;
    public void Interact(PlayerInteraction playerInteraction)
    {
        temp = !temp;
        if (temp)
        {
            SymbolGroupUI.Main.Connect(playerInteraction.PlayerController.Codex, this);
            PlayerController.Instance.FreezePlayer(true);
            PlayerController.Instance.PlayerControls.SwitchToUI(true);
        }
        else
        {
            SymbolGroupUI.Main.Disconnect(playerInteraction.PlayerController.Codex);
            PlayerController.Instance.FreezePlayer(false);
            PlayerController.Instance.PlayerControls.SwitchToUI(false);
        }
        for (int i = 0; i < SymbolDatas.Length; i++)
        {
            playerInteraction.PlayerController.Codex.DiscoverSymbol(SymbolDatas[i].SymbolID);
            Debug.Log($"Data : {SymbolDatas[i]},IsDiscovered: {playerInteraction.PlayerController.Codex.IsSymbolDiscovered(SymbolDatas[i].SymbolID)}");
        }
    }

}