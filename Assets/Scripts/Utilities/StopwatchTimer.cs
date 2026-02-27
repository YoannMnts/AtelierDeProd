public class StopwatchTimer : Timer
{
    public StopwatchTimer() : base(0){}
    public override void Tick(float deltaTime)
    {
        if (IsRunning)
        {
            CurrentTime += deltaTime;
        }
    }
        
    public void Reset() => CurrentTime = 0;
        
    public float GetCurrentTime() => CurrentTime;
}