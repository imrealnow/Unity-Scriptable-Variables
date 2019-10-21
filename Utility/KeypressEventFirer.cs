using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeypressEventFirer : MonoBehaviour
{
    public KeyCode keycode;
    public UnityEvent eventToFire;

    void Update()
    {
        if (Input.GetKeyDown(keycode) && eventToFire != null)
            eventToFire.Invoke();
    }
}
