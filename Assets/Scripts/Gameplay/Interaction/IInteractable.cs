using System;
using Gameplay.Player;
using Gameplay.UI;
using UnityEngine;

namespace Gameplay.Interaction
{
    public interface IInteractable
    {
        public void Interact(PlayerInteraction playerInteraction);
    }
}