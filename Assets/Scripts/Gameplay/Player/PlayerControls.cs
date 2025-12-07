using UnityEngine;
using UnityEngine.InputSystem;

namespace Ozkaal.Gameplay.Gameplay.Player
{
    public class PlayerControls : MonoBehaviour, IPlayerComponent
    {
        private const string DEFAULT_MAP = "Default";
        
        public PlayerController playerController { get; set; }
        
        [field: SerializeField]
        public InputActionAsset InputActionAsset { get; private set; }
        public InputAction MovementInput { get; private set; }
        public InputAction InteractInput { get; private set; }

        private void OnEnable()
        {
            InitInput();
        }

        private void OnDisable()
        {
            DeinitInput();
        }

        private void InitInput()
        {
            MovementInput = InputActionAsset.FindActionMap(DEFAULT_MAP).FindAction("Move");
            InteractInput = InputActionAsset.FindActionMap(DEFAULT_MAP).FindAction("Interact");
            MovementInput?.Enable();
            InteractInput?.Enable();
            InputActionAsset.FindActionMap(DEFAULT_MAP)?.Enable();
        }
        
        private void DeinitInput()
        {
            if (MovementInput != null)
            {
                MovementInput?.Disable();
                MovementInput = null;
            }
            InputActionAsset.FindActionMap(DEFAULT_MAP)?.Disable();
        }
        
        public Vector2 GetMovementInput()
        {
            return MovementInput?.ReadValue<Vector2>() ?? Vector2.zero;
        }
    }
}