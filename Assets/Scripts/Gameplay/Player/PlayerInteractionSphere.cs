using System;
using Ozkaal.Gameplay.Gameplay.Interaction.Symbols;
using UnityEngine;

namespace Ozkaal.Gameplay.Gameplay.Player
{
    public partial class PlayerInteraction
    {
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.gameObject.name);
            if (other.CompareTag("Symbol"))
            {
                var components = other.GetComponentsInChildren<WorldSymbol>();
                for (int i = 0; i < components.Length; i++)
                {
                    components[i].gameObject.GetComponent<Material>().color = Color.green;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Symbol"))
            {
                var components = other.GetComponentsInChildren<WorldSymbol>();
                for (int i = 0; i < components.Length; i++)
                {
                    components[i].gameObject.GetComponent<Material>().color = Color.grey;
                }
            }
        }
    }
}