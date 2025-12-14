using UnityEngine;

namespace Ozkaal.Gameplay.Gameplay.UI
{
    //Make for POC :
    //CodexUI SerializeField
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance { get; private set; }
        
        [field: SerializeField]
        public SymbolGroupUI SymbolGroupUI { get; private set; }
        
        [field: SerializeField]
        public CodexUI CodexUI { get; private set; }
        
        [field: SerializeField]
        public CanvasGroup canvasGroup { get; private set; }
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
            var childsUI = GetComponentsInChildren<IUIElement>();
            foreach (var ui in childsUI)
            {
                ui.UIManager = this;
            }
        }
    }
}