using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public ReadOnlyCollection<SManager> Managers => managers.AsReadOnly();

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

    /// <summary>
    /// Initialises and adds manager to handler.
    /// </summary>
    /// <param name="manager">Manager to add</param>
    public void AddManager(SManager manager)
    {
        manager.AssignHandler(this);
        manager.enabled |= true;
        managersUpdate += manager.Update;
        managers.Add(manager);
    }

    /// <summary>
    /// Disable, clean up and remove manager from handler.
    /// </summary>
    /// <param name="manager">Manager to remove</param>
    /// <returns>Returns false if manager did not exist, true if successfully removed</returns>
    public bool RemoveManager(SManager manager)
    {
        if (!managers.Contains(manager)) return false;
        manager.OnDisabled();
        manager.enabled = false;
        managersUpdate -= manager.Update;
        managers.Remove(manager);
        return true;
    }

    /// <summary>
    /// Removes manager at index in list
    /// </summary>
    /// <param name="index">Index of manager to remove</param>
    public void RemoveManagerAt(int index)
    {
        if (index >= managers.Count) return;
        RemoveManager(managers[index]);
    }
}
