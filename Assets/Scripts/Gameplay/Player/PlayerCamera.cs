using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerCamera : MonoBehaviour, IPlayerComponent
    {
        public PlayerController playerController { get; set; }
    }
}