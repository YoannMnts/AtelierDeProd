using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInputAction;


namespace Ozkaal.Gameplay.Gameplay.Player
{
    [CreateAssetMenu(fileName = "PlayerControls", menuName = "Player/Controls")]
    public class PlayerControls : ScriptableObject, IPlayerActions
    {
       public event Action<Vector2> Move = delegate { };
       public event Action<bool> Jump = delegate { };
       public event Action<bool> Crouch = delegate { };
       
       PlayerInputAction inputActions;
       
       public Vector3 Direction => inputActions.Player.Move.ReadValue<Vector2>();

       private void OnEnable()
       {
          if (inputActions == null)
          {
             inputActions = new PlayerInputAction();
             inputActions.Player.SetCallbacks(this);
          }
          inputActions.Enable();
       }

       public void EnablePlayerActions()
       {
          inputActions.Enable();
       }

       public void OnMove(InputAction.CallbackContext context)
        {
           Move?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
           if (context.performed)
           {
              PlayerController.Instance.PlayerInteraction.Interact();
           }
        }

        public void OnCodex(InputAction.CallbackContext context)
        {
           //aaaaa
        }

        public void OnJump(InputAction.CallbackContext context)
        {
           switch (context.phase)
           {
              case InputActionPhase.Started:
                 Jump?.Invoke(true);
                 break;
              case InputActionPhase.Canceled:
                 Jump?.Invoke(false);
                 break;
           }
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
           switch (context.phase)
           {
              case InputActionPhase.Started:
                 Crouch?.Invoke(true);
                 break;
              case InputActionPhase.Canceled:
                 Crouch?.Invoke(false);
                 break;
           }
        }
    }
}