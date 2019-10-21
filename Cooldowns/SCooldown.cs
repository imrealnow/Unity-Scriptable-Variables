using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class that wraps the cooldown object so it can be saved to the project folder as an asset
/// and used in multiple objects.
/// </summary>
[CreateAssetMenu(fileName = "Cooldown", menuName = "SO/Cooldown", order = 1)]
public class SCooldown : ScriptableObject
{
    public Cooldown cooldown;
    public FloatReference startDuration;

    private void OnEnable()
    {
        cooldown = new Cooldown();
        cooldown.ChangeDuration(startDuration.Value);
    }

    public float GetProgressToReset()
    {
        return cooldown.GetProgressToReset();
    }

    public float GetRemainingTime()
    {
        return cooldown.GetRemainingTime();
    }

    public void ChangeDuration(float newDuration)
    {
        cooldown.ChangeDuration(newDuration);
    }

    public bool TryUseCooldown()
    {
        return cooldown.TryUseCooldown();
    }
}
