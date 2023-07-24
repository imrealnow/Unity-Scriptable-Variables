using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

/// <summary>
/// A RunningSet is a ScriptableObject that holds a collection of GameObjects which gets updated at runtime.
/// </summary>
[CreateAssetMenu(fileName = "RunningSet", menuName = "SO/RunningSet", order = 1)]
public class RunningSet : ScriptableObject
{
    /// <summary>
    /// The set of GameObjects in the running set.
    /// </summary>
    private Set<GameObject> set = new HashSet<GameObject>();

    /// <summary>
    /// A read-only version of the set.
    /// </summary>
    public ReadOnlyCollection<GameObject> Set => new List<GameObject>(set).AsReadOnly();

    /// <summary>
    /// The number of GameObjects in the running set.
    /// </summary>
    public int Count => set.Count;

    /// <summary>
    /// Clears the set when the scriptable object is disabled.
    /// </summary>
    private void OnDisable()
    {
        set.Clear();
    }

    /// <summary>
    /// Adds a GameObject to the running set.
    /// </summary>
    /// <param name="objToAdd">GameObject to be added.</param>
    public void AddToSet(GameObject objToAdd)
    {
        set.Add(objToAdd);
    }

    /// <summary>
    /// Removes a GameObject from the running set.
    /// </summary>
    /// <param name="objToRemove">GameObject to be removed.</param>
    /// <returns>True if the GameObject was present and removed; otherwise, false.</returns>
    public bool RemoveFromSet(GameObject objToRemove)
    {
        return set.Remove(objToRemove);
    }
}
