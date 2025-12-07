using UnityEngine;

namespace Ozkaal.Gameplay.Gameplay.Player
{
    public class PlayerCamera : MonoBehaviour, IPlayerComponent
    {
        public PlayerController playerController { get; set; }
    }
}