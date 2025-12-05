using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerController : MonoBehaviour
    {
        [field : SerializeField]
        public PlayerMovement PlayerMovement { get; private set; }
        [field : SerializeField]
        public PlayerCamera PlayerCamera { get; private set; }
        [field : SerializeField]
        public PlayerControls PlayerControls { get; private set; }
        public Codex Codex { get; private set; }
    
        public IPlayerComponent[] PlayerComponents { get; private set; }
    
        private void Awake()
        {
            Codex = new Codex();
            PlayerComponents = GetComponentsInChildren<IPlayerComponent>();
            foreach (IPlayerComponent component in PlayerComponents)
                component.playerController = this;
        }
    }
}
