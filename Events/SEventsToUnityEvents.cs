using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A class to map SEvent to UnityEvent and invoke the UnityEvent when the corresponding SEvent fires.
/// </summary>
public class SEventsToUnityEvents : MonoBehaviour
{
    [Tooltip("List of SEvent-UnityEvent pairs")]
    public List<EventPair> eventPairs = new List<EventPair>();

    void OnEnable()
    {
        eventPairs.ForEach(ep => ep.sharedEvent.sharedEvent += ep.eventHandler);
    }

    void OnDisable()
    {
        eventPairs.ForEach(ep => ep.sharedEvent.sharedEvent -= ep.eventHandler);
    }
}

/// <summary>
/// A class representing a pair of corresponding SEvent and UnityEvent.
/// </summary>
[System.Serializable]
public class EventPair
{
    [Tooltip("The shared event")]
    public SEvent sharedEvent;
    
    [Tooltip("The Unity event to invoke when the shared event fires")]
    public UnityEvent unityEvent;
    
    public Action eventHandler;

    public EventPair()
    {
        this.eventHandler = () => unityEvent?.Invoke();
    }
}