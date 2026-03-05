public interface IInteractable
{
    public int Priority { get; }
    
    public void Interact(PlayerInteraction playerInteraction);
    void OnEnter(PlayerInteraction playerInteraction);
    void OnExit(PlayerInteraction playerInteraction);
}