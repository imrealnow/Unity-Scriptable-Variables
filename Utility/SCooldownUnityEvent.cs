using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SCooldownUnityEvent : MonoBehaviour
{
    public SCooldown cooldown;
    public UnityEvent eventToInvoke;

    public void InvokeEvent()
    {
        if (eventToInvoke != null)
            if (cooldown.TryUseCooldown())
                eventToInvoke.Invoke();
    }
}
