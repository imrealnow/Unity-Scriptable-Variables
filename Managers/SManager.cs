using UnityEngine;

/// <summary>
/// SManager is an abstract base class for all manager scripts in your ScriptableObject architecture.
/// It provides common functionality for managing game logic that needs to persist across different scenes.
/// </summary>
public abstract class SManager : ScriptableObject
{
    /// <summary>
    /// Boolean value indicating whether the manager is currently enabled or not.
    /// </summary>
    [HideInInspector] public bool enabled;

    /// <summary>
    /// Reference to the SManagerHandler object which calls the Update method on this SManager every frame.
    /// </summary>
    protected SManagerHandler handler;

    /// <summary>
    /// Assigns an SManagerHandler to this SManager. Useful for assigning a handler that can call the Update method.
    /// </summary>
    /// <param name="handler">The SManagerHandler to be assigned.</param>
    public void AssignHandler(SManagerHandler handler)
    {
        this.handler = handler;
    }

    /// <summary>
    /// Method called when the manager is enabled. By default, registers the manager to the ManagerRegistry.
    /// Override to add additional behavior when the manager is enabled.
    /// </summary>
    public virtual void OnEnabled() { ManagerRegistry.Instance.RegisterManager(this); }

    /// <summary>
    /// Method called when the manager is disabled. By default, deregisters the manager from the ManagerRegistry.
    /// Override to add additional behavior when the manager is disabled.
    /// </summary>
    public virtual void OnDisabled() { ManagerRegistry.Instance.DeregisterManager(this); }

    /// <summary>
    /// Method called every frame by the SManagerHandler if the manager is enabled. Override to add custom update logic.
    /// </summary>
    public virtual void Update() { }
}
