using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SManagerHandler is responsible for controlling the lifecycle of SManager instances and
/// invoking their Update methods each frame.
/// </summary>
public class SManagerHandler : MonoBehaviour
{
    /// <summary>
    /// List of SManager instances controlled by this SManagerHandler.
    /// </summary>
    [SerializeField] private List<SManager> managers = new List<SManager>();

    /// <summary>
    /// Event invoked each frame for updating all SManager instances.
    /// </summary>
    private event Action managersUpdate;

    private void Start()
    {
        IterateManagers(manager => manager.OnEnabled());
    }

    private void OnEnable()
    {
        IterateManagers(manager =>
        {
            manager.AssignHandler(this);
            manager.enabled = true;
            managersUpdate += manager.Update;
        });
    }

    private void OnDisable()
    {
        IterateManagers(manager =>
        {
            manager.OnDisabled();
            manager.enabled = false;
            managersUpdate -= manager.Update;
        });
    }

    void FixedUpdate()
    {
        managersUpdate?.Invoke();
    }

    /// <summary>
    /// Resets all managers by disabling and re-enabling them.
    /// </summary>
    public void Reset()
    {
        OnDisable();
        OnEnable();
    }

    /// <summary>
    /// Helper method to perform an action on all SManager instances.
    /// </summary>
    /// <param name="action">The action to perform on each manager.</param>
    private void IterateManagers(Action<SManager> action)
    {
        if (managers.Count > 0)
        {
            foreach (var manager in managers)
            {
                action(manager);
            }
        }
    }
}
