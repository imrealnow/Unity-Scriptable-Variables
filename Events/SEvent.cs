using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Event", menuName = "SO/Event/Default", order = 1)]
public class SEvent : ScriptableObject
{
    public Action sharedEvent;

    public void Fire()
    {
        sharedEvent?.Invoke();
    }
}

// generic definition which can store data of type T
public class SEvent<T> : ScriptableObject
{
    public Action<T> sharedEvent;

    public void Fire(T data)
    {
        sharedEvent?.Invoke(data);
    }
}