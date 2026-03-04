using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerAnimation : MonoBehaviour
    {
        private PlayerController Player => PlayerController.Instance;
        
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Jump = Animator.StringToHash("Jumping");
        private static readonly int IsCrouch = Animator.StringToHash("IsCrouch");

        [SerializeField]
        private Animator animator;

        private void OnEnable()
        {
            Player.PlayerMovement.OnMoving += SetSpeed;
            Player.PlayerMovement.OnCrouching += SetIsCrouch;
            Player.PlayerMovement.OnJumping += TriggerJump;
        }

        private void SetSpeed(float speed)
        {
            animator.SetFloat(Speed, speed);
        }

        private void TriggerJump()
        {
            animator.SetTrigger(Jump);
        }

        private void SetIsCrouch(bool isCrouch)
        {
            animator.SetBool(IsCrouch, isCrouch);
        }
    }
}