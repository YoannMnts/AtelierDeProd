using System;

namespace Utilities
{
    public abstract class Timer
    {
        public float Progress => CurrentTime / initialTime;
        public void Resume() => IsRunning = true;
        public void Pause() => IsRunning = false;
        
        protected float initialTime;
        protected float CurrentTime { get; set; }
        public bool IsRunning { get; private set; }
        
        
        public Action OnTimerStart;
        public Action OnTimerStop;

        protected Timer(float value)
        {
            initialTime = value;
            IsRunning = false;
        }

        public void Start()
        {
            CurrentTime = initialTime;
            if (!IsRunning)
            {
                IsRunning = true;
                OnTimerStart?.Invoke();
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                OnTimerStop?.Invoke();
            }
        }
        
        public abstract void Tick(float deltaTime);
    }
}