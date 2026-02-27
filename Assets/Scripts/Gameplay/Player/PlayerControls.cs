using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerControls", menuName = "Player/Controls")]
public class PlayerControls : ScriptableObject, PlayerInputAction.IPlayerActions
{
   public event Action<Vector2> Zoom;
   public event Action<Vector2> Move = delegate { };
   public event Action<bool> Jump = delegate { };
   public event Action<bool> Crouch = delegate { };
   public event Action Interact = delegate { };
   public event Action<Codex> OpenCodex = delegate { };
   public event Action<Codex> CloseCodex = delegate { };
       
   PlayerInputAction inputActions;
       
   bool isCodexActive;
       
   public Vector3 Direction => inputActions.Player.Move.ReadValue<Vector2>();

   private void OnEnable()
   {
      if (inputActions == null)
      {
         inputActions = new PlayerInputAction();
         inputActions.Player.SetCallbacks(this);
      }

      EnablePlayerActions();
   }

   private void OnDisable()
   {
      DisablePlayerActions();
   }

   public void DisablePlayerActions()
   {
      inputActions.Disable();
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
         Interact?.Invoke();
      }
   }

   public void OnCodex(InputAction.CallbackContext context)
   {
      if (context.performed)
      {
         isCodexActive = !isCodexActive;
         Codex currentCodex = PlayerController.Instance.Codex;
         if (isCodexActive)
            OpenCodex?.Invoke(currentCodex);
         else
            CloseCodex?.Invoke(currentCodex);
      }
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

   public void OnZoom(InputAction.CallbackContext context)
   {
      Zoom?.Invoke(context.ReadValue<Vector2>());
   }
}