using UnityEngine;
using UnityEngine.InputSystem;

namespace Ozkaal.Gameplay.Gameplay.Player
{
    [DefaultExecutionOrder(-10)]
    //CodexInput but normally it's fine
    public class PlayerControls : MonoBehaviour, IPlayerComponent
    {
        private const string DEFAULT_MAP = "Default";
        
        public PlayerController playerController { get; set; }
        
        [field: SerializeField]
        public InputActionAsset InputActionAsset { get; private set; }
        public InputAction MovementInput { get; private set; }
        public InputAction InteractInput { get; private set; }
        public InputAction CodexInput { get; private set; }
        public InputAction JumpInput { get; private set; }
        public InputAction CrouchInput { get; private set; }

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
            CodexInput = InputActionAsset.FindActionMap(DEFAULT_MAP).FindAction("Codex");
            JumpInput = InputActionAsset.FindActionMap(DEFAULT_MAP).FindAction("Jump");
            CrouchInput = InputActionAsset.FindActionMap(DEFAULT_MAP).FindAction("Crouch");
            MovementInput?.Enable();
            InteractInput?.Enable();
            CodexInput?.Enable();
            JumpInput?.Enable();
            CrouchInput?.Enable();
            InputActionAsset.FindActionMap(DEFAULT_MAP)?.Enable();
        }
        
        private void DeinitInput()
        {
            if (MovementInput != null)
            {
                MovementInput?.Disable();
                MovementInput = null;
            }
            if (InteractInput != null)
            {
                InteractInput?.Disable();
                InteractInput = null;
            }
            if (CodexInput != null)
            {
                CodexInput?.Disable();
                CodexInput = null;
            }
            if (JumpInput != null)
            {
                JumpInput?.Disable();
                JumpInput = null;
            }

            if (CrouchInput != null)
            {
                CrouchInput?.Disable();
                CrouchInput = null;
            }
            InputActionAsset.FindActionMap(DEFAULT_MAP)?.Disable();
        }
        
        public Vector2 GetMovementInput()
        {
            return MovementInput?.ReadValue<Vector2>() ?? Vector2.zero;
        }
    }
}