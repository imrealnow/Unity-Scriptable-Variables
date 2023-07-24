using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SManagerHandler : MonoBehaviour
{
    public List<SManager> managers = new List<SManager>();

    public delegate void GOCallback();
    public GOCallback managersUpdate;

    private void Start()
    {
        if(managers.Count > 0)
        {
            for (int i = 0; i < managers.Count; i++)
            {
                managers[i].OnEnabled();
            }
        }
    }

    private void OnEnable()
    {
        if(managers.Count > 0)
        {
            for(int i = 0; i < managers.Count; i++)
            {
                if (managers[i] != null)
                {
                    managers[i].AssignHandler(this);
                    managers[i].enabled = true;
                    managersUpdate += managers[i].Update;
                }
            }
        }
    }

    private void OnDisable()
    {
        if (managers.Count > 0)
        {
            for (int i = managers.Count - 1; i > -1; i--)
            {
                managers[i].OnDisabled();
                managers[i].enabled = false;
                managersUpdate -= managers[i].Update;
            }
        }
    }

    void FixedUpdate()
    {
        if (managersUpdate != null)
            managersUpdate.Invoke();
    }

    public void Reset()
    {
        OnDisable();
        OnEnable();
    }
}
