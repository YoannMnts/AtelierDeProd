using UnityEngine;

[DefaultExecutionOrder(-5)]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
        
    [field : SerializeField]
    public PlayerMovement PlayerMovement { get; private set; }
    [field : SerializeField]
    public PlayerCamera PlayerCamera { get; private set; }
    [field : SerializeField]
    public PlayerControls PlayerControls { get; private set; }
    [field : SerializeField]
    public PlayerInteraction PlayerInteraction { get; private set; }
    public Codex Codex { get; private set; }
    
    public IPlayerComponent[] PlayerComponents { get; private set; }
        
    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        Instance = this;
        Codex = new Codex();
        PlayerComponents = GetComponentsInChildren<IPlayerComponent>();
        foreach (IPlayerComponent component in PlayerComponents)
            component.playerController = this;
    }

    public void FreezePlayer(bool isFreeze)
    {
        PlayerMovement.FreezePlayer(isFreeze);
    }
}