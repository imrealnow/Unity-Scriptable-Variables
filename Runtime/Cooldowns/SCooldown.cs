using UnityEngine;

/// <summary>
/// A ScriptableObject that encapsulates a Cooldown instance, allowing it to be saved to the project folder as an asset.
/// This can be used in multiple objects.
/// </summary>
[CreateAssetMenu(fileName = "Cooldown", menuName = "SO/Cooldown", order = 1)]
public class SCooldown : ScriptableObject
{
    /// <summary>
    /// The starting duration for the cooldown, set by a FloatReference.
    /// </summary>
    public FloatReference initialCooldownTime;

    /// <summary>
    /// The Cooldown instance that this ScriptableObject encapsulates.
    /// </summary>
    public Cooldown cooldown;

    private void OnEnable()
    {
        cooldown = new Cooldown(0f);
        cooldown.Duration = initialCooldownTime.Value;
    }

    /// <summary>
    /// Gets the progress to the Cooldown's reset, expressed as a value between 0 and 1.
    /// </summary>
    public float ProgressToReset => cooldown.ProgressToReset;

    /// <summary>
    /// Gets the remaining time before the Cooldown is reset.
    /// </summary>
    public float RemainingTime => cooldown.RemainingTime;

    /// <summary>
    /// Changes the duration of the Cooldown and restarts it.
    /// </summary>
    /// <param name="newDuration">The new duration to set the Cooldown to.</param>
    public void ChangeDuration(float newDuration)
    {
        cooldown.Duration = newDuration;
    }

    /// <summary>
    /// Tries to use the Cooldown and restart it if it's finished.
    /// </summary>
    /// <returns>Returns true if the Cooldown was available and restarted; false otherwise.</returns>
    public bool TryUseCooldown() => cooldown.TryUseCooldown();
}
