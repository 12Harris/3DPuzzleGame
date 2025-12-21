// ============================================================================
// TIMER SYSTEM
// ============================================================================
using System;

public class Timer
{
    private float _duration;
    private float _timeRemaining;
    private bool _isRunning;
    private Action _onComplete;

    public bool IsRunning => _isRunning;
    public float Progress => 1f - (_timeRemaining / _duration);
    public float TimeRemaining => _timeRemaining;

    public Timer(float duration)
    {
        _duration = duration;
        _timeRemaining = duration;
    }

    public void Start(Action onComplete = null)
    {
        _timeRemaining = _duration;
        _isRunning = true;
        _onComplete = onComplete;
    }

    public void Stop()
    {
        _isRunning = false;
    }

    public void Reset()
    {
        _timeRemaining = _duration;
    }

    public void Tick(float deltaTime)
    {
        if (!_isRunning) return;

        _timeRemaining -= deltaTime;
        if (_timeRemaining <= 0)
        {
            _timeRemaining = 0;
            _isRunning = false;
            _onComplete?.Invoke();
        }
    }
}