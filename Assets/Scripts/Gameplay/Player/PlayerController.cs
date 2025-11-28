using System;
using Gameplay.Player;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [field : SerializeField]
    public PlayerMovement PlayerMovement { get; private set; }
    [field : SerializeField]
    public PlayerCamera PlayerCamera { get; private set; }
    [field : SerializeField]
    public PlayerControls PlayerControls { get; private set; }
    
    public IPlayerComponent[] PlayerComponents { get; private set; }
    
    private void Awake()
    {
        PlayerComponents = GetComponentsInChildren<IPlayerComponent>();
        foreach (IPlayerComponent component in PlayerComponents)
            component.playerController = this;
    }
}
