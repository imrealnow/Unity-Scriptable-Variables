using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SEventsToUnityEvents : MonoBehaviour
{
    public List<EventPair> eventPairs = new List<EventPair>();

    void OnEnable()
    {
        if (eventPairs.Count > 0)
        {
            for (int i = 0; i < eventPairs.Count; i++)
            {
                eventPairs[i].sharedEvent.sharedEvent += eventPairs[i].eventHandler;
            }
        }
    }

    void OnDisable()
    {
        if (eventPairs.Count > 0)
        {
            for (int i = 0; i < eventPairs.Count; i++)
            {
                eventPairs[i].sharedEvent.sharedEvent -= eventPairs[i].eventHandler;
            }
        }
    }
}

[System.Serializable]
public class EventPair
{
    public SEvent sharedEvent;
    public UnityEvent unityEvent;
    public Action eventHandler;

    public EventPair()
    {
        this.eventHandler = () => { if (this.unityEvent != null) { this.unityEvent.Invoke(); } };
    }
}
