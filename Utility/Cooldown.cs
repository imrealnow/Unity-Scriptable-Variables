using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Class that can act as a standalone cooldown
/// </summary>
[Serializable]
public class Cooldown
{
    [SerializeField] private float _duration;
    [SerializeField] private float _startTime;
    [SerializeField] private bool _initialised;


    private float EndTime
    {
        get { return _startTime + _duration; }
    }

    public float Duration
    {
        get { return _duration; }
    }

    /// <summary>
    /// Changes the duration of the cooldown while maintaining the current progress
    /// towards resetting it.
    /// </summary>
    /// <param name="newDuration">The new duration to set the cooldown to</param>
    public void ChangeDuration(float newDuration)
    {
        _duration = Mathf.Abs(newDuration);
        _startTime = Time.time;
    }

    /// <summary>
    /// Returns the amount of time left before the cooldown is able to be fired again
    /// </summary>
    /// <returns>The amount of time left before the cooldown is able to be fired again</returns>
    public float GetRemainingTime()
    {
        return GetProgressToReset() >= 1 ? 0 : EndTime - Time.time;
    }

    public void ReduceCooldown(float percent)
    {
        _startTime += Duration * Mathf.Clamp(percent, 0, 1);
    }

    /// <summary>
    /// Try to use and restart the cooldown if it's available.
    /// </summary>
    /// <returns>Returns true if the cooldown was available and restarted</returns>
    public bool TryUseCooldown()
    {
        if (Time.time <= EndTime)
            return false;

        _startTime = Time.time;
        return true;
    }

    /// <summary>
    /// Get's the percentage of time left before the cooldown resets
    /// </summary>
    public float GetProgressToReset()
    {
        if (_initialised)
        {
            float percentage = 1 - (EndTime - Time.time) / _duration;
            return percentage;
        }
        else
            return default;
    }

    public Cooldown()
    {
        _initialised = true;
    }

    public Cooldown(float duration)
    {
        _initialised = true;
        _duration = Mathf.Abs(duration);
        _startTime = Time.time - duration;
    }
}
