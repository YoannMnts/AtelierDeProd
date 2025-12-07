namespace Ozkaal.Gameplay.Gameplay.UI
{
    public interface IUIElement
    {
        public UIManager UIManager { get; set; }
        public void Check();
        public void Show();
        public void Hide();
    }
}