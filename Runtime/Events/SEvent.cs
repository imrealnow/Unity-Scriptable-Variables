using System;
using UnityEngine;

/// <summary>
/// Scriptable object representing an event that can be invoked at runtime.
/// </summary>
[CreateAssetMenu(fileName = "Event", menuName = "SO/Event/Default", order = 1)]
public class SEvent : ScriptableObject
{
    /// <summary>
    /// The delegate representing the event to be fired.
    /// </summary>
    public Action sharedEvent;

    /// <summary>
    /// Invoke the event.
    /// </summary>
    public void Fire()
    {
        sharedEvent?.Invoke();
    }
}

/// <summary>
/// Generic scriptable object representing an event that can be invoked at runtime.
/// The event carries a payload of the specified type.
/// </summary>
/// <typeparam name="T">The type of the event's data payload.</typeparam>
public class SEvent<T> : ScriptableObject
{
    /// <summary>
    /// The delegate representing the event to be fired. It takes one argument of type T.
    /// </summary>
    public Action<T> sharedEvent;

    /// <summary>
    /// Invoke the event and pass the data.
    /// </summary>
    /// <param name="data">The data payload to pass with the event.</param>
    public void Fire(T data)
    {
        sharedEvent?.Invoke(data);
    }
}
