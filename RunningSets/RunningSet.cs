using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RunningSet", menuName = "SO/RunningSet", order = 1)]
public class RunningSet : ScriptableObject
{
    public List<GameObject> set = new List<GameObject>();

    public int Count { get { return set.Count; } }

    private void OnDisable()
    {
        set.Clear();
    }

    public void AddToSet(GameObject objToAdd)
    {
        if (!set.Contains(objToAdd))
            set.Add(objToAdd);
    }

    public bool RemoveFromSet(GameObject objToRemove)
    {
        if (set.Contains(objToRemove))
            return set.Remove(objToRemove);

        return false;
    }

    public List<GameObject> GetSet()
    {
        if (set.Count > 0)
            return set;

        return null;
    }
}
