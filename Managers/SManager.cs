using UnityEngine;

public abstract class SManager : ScriptableObject
{
    [HideInInspector] public bool enabled;

    protected SManagerHandler handler;

    public void AssignHandler(SManagerHandler handler)
    {
        this.handler = handler;
    }

    public virtual void OnEnabled() { ManagerRegistry.Instance.RegisterManager(this); }

    public virtual void OnDisabled() { ManagerRegistry.Instance.DeregisterManager(this); }

    public virtual void Update() { }
}
