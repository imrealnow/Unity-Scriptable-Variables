using UnityEngine;

/// <summary>
/// RunningSetHandler is a MonoBehaviour that adds or removes a GameObject from a RunningSet when the GameObject is enabled or disabled.
/// </summary>
public class RunningSetHandler : MonoBehaviour
{
    /// <summary>
    /// Reference to the RunningSet this GameObject should be added to and removed from.
    /// </summary>
    [SerializeField] private RunningSet runningSet;
    public RunningSet RunningSet => runningSet;

    /// <summary>
    /// Adds the GameObject to the RunningSet when the GameObject is enabled.
    /// </summary>
    private void OnEnable()
    {
        runningSet?.AddToSet(gameObject);
    }

    /// <summary>
    /// Removes the GameObject from the RunningSet when the GameObject is disabled.
    /// </summary>
    private void OnDisable()
    {
        runningSet?.RemoveFromSet(gameObject);
    }
}
