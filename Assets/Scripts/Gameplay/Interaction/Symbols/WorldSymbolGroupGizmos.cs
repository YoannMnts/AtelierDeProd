using System;
using UnityEngine;

namespace Ozkaal.Gameplay.Gameplay.Interaction.Symbols
{
    public partial class WorldSymbolGroup
    {
        private GameObject firstSymbol;
        private GameObject lastSymbol;
        
        private void OnDrawGizmos()
        {
            Symbols = GetComponentsInChildren<WorldSymbol>();
            BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
            if (Symbols != null)
            {
                
                firstSymbol = Symbols[0].gameObject;
                lastSymbol = Symbols[^1].gameObject;
            }
            var groupCenter = (firstSymbol.transform.localPosition.x + lastSymbol.transform.localPosition.x) * 0.5f;
            var groupSize = Mathf.Abs(firstSymbol.transform.localPosition.x) + Mathf.Abs(lastSymbol.transform.localPosition.x) + 1f;
            boxCollider.center = new Vector3(groupCenter, 0, 0);
            boxCollider.size = new Vector3(groupSize, 1, 1);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.gameObject.name);
            if (other.CompareTag("Player"))
            {
                for (int i = 0; i < Symbols.Length; i++)
                {
                    Symbols[i].GetComponent<MeshRenderer>().material.color = Color.green;
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                for (int i = 0; i < Symbols.Length; i++)
                {
                    Symbols[i].GetComponent<MeshRenderer>().material.color = Color.grey;
                }
            }
        }
    }
}