using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Event", menuName = "SO/Event", order = 1)]
public class SEvent : ScriptableObject
{
    public Action sharedEvent;

    public void Fire()
    {
        if (sharedEvent != null)
            sharedEvent.Invoke();
    }
}