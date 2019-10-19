using UnityEngine;

public class SetHandler : MonoBehaviour
{
    public RunningSet runningSet;

    private void OnEnable()
    {
        if (runningSet != null)
            runningSet.AddToSet((GameObject)gameObject);
    }

    private void OnDisable()
    {
        if (runningSet != null)
            runningSet.RemoveFromSet((GameObject)gameObject);
    }
}
