using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInputAction;


namespace Ozkaal.Gameplay.Gameplay.Player
{
    [DefaultExecutionOrder(-10)]
    [CreateAssetMenu(fileName = "PlayerControls", menuName = "Player/Controls")]
    public class PlayerControls : ScriptableObject, IPlayerActions
    {
       public event Action<Vector2> Move = delegate { };
       public event Action<bool> Jump = delegate { };
       
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
           //aaaaa
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
           //aaaaa
        }
    }
}