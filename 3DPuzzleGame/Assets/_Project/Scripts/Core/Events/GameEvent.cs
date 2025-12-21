// ============================================================================
// EVENT SYSTEM
// ============================================================================
using System;
using System.Collections.Generic;

public class GameEvent<T>
{
    private readonly List<Action<T>> _listeners = new List<Action<T>>();

    public void Subscribe(Action<T> listener)
    {
        if (!_listeners.Contains(listener))
            _listeners.Add(listener);
    }

    public void Unsubscribe(Action<T> listener)
    {
        _listeners.Remove(listener);
    }

    public void Invoke(T data)
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
            _listeners[i]?.Invoke(data);
    }
}