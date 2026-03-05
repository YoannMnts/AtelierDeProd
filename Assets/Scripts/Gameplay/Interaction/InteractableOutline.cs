using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class InteractableOutline : MonoBehaviour
    {
        private const int OUTLINE_RENDERING_LAYER = 1;
        
        private MeshRenderer[] meshRenderers;


        private void Awake()
        {
            meshRenderers = GetComponentsInChildren<MeshRenderer>();
        }

        public void Show()
        {
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                meshRenderer.renderingLayerMask |= 1u << OUTLINE_RENDERING_LAYER;
            }
        }

        public void Hide()
        {
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                meshRenderer.renderingLayerMask &= ~(1u << OUTLINE_RENDERING_LAYER);
            }
        }
    }
}