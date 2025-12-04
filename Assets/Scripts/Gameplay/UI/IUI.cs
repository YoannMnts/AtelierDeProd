using Unity.VisualScripting;

namespace Gameplay.UI
{
    public interface IUI
    {
        public UIManager UIManager { get; set; }
        public void Check();
        public void Show();
        public void Hide();
    }
}