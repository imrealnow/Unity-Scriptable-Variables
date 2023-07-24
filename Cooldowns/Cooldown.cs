[Serializable]
public class Cooldown
{
    [SerializeField] private float _duration;
    [SerializeField] private float _startTime;
    [SerializeField] private bool _initialised;

    /// <summary>
    /// Gets the time at which the cooldown will end.
    /// </summary>
    private float EndTime => _startTime + _duration;

    /// <summary>
    /// Gets or sets the duration of the cooldown.
    /// Setting the duration will restart the cooldown.
    /// </summary>
    public float Duration 
    { 
        get => _duration;
        set 
        {
            _duration = Mathf.Abs(value);
            _startTime = Time.time;
        }
    }

    /// <summary>
    /// Gets the remaining time before the cooldown is reset.
    /// </summary>
    public float RemainingTime => !_initialised || GetProgressToReset() >= 1 ? 0 : EndTime - Time.time;

    /// <summary>
    /// Gets the progress to the cooldown reset, expressed as a value between 0 and 1.
    /// </summary>
    public float ProgressToReset => !_initialised ? default : 1 - (EndTime - Time.time) / _duration;

    /// <summary>
    /// Reduces the remaining cooldown duration by a percentage of the total duration.
    /// </summary>
    /// <param name="percent">The percentage to reduce the cooldown by, expressed as a value between 0 and 1.</param>
    public void ReduceCooldown(float percent)
    {
        _startTime += Duration * Mathf.Clamp(percent, 0, 1);
    }

    /// <summary>
    /// Tries to use the cooldown and restart it if it's finished.
    /// </summary>
    /// <returns>Returns true if the cooldown was available and restarted; false otherwise.</returns>
    public bool TryUseCooldown()
    {
        if (Time.time <= EndTime)
            return false;

        _startTime = Time.time;
        return true;
    }

    /// <summary>
    /// Default constructor. Initializes a new instance of the <see cref="Cooldown"/> class.
    /// </summary>
    public Cooldown()
    {
        _initialised = true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Cooldown"/> class with a specific duration.
    /// </summary>
    /// <param name="duration">The duration of the cooldown.</param>
    public Cooldown(float duration)
    {
        _initialised = true;
        Duration = duration;
        _startTime = Time.time - duration;
    }
}
