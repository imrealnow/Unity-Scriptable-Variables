using System;
using UnityEngine;

/// <summary>
/// SharedVariable is a generic ScriptableObject that can hold any type of variable. 
/// </summary>
/// <typeparam name="T">The type of the variable.</typeparam>
[System.Serializable]
public class SharedVariable<T> : ScriptableObject
{
    [SerializeField]
    private bool resetValueOnLoad = false;

    [SerializeField]
    private T _variable;
    private T _placeholderValue;

    [SerializeField]
    [TextArea]
    private string description;

    /// <summary>
    /// Event that gets triggered when the value of the variable changes.
    /// </summary>
    public event Action variableChanged;

    /// <summary>
    /// Gets or sets the value of the variable. 
    /// If resetValueOnLoad is true, it uses a placeholder value that can be reset.
    /// </summary>
    public T Value
    {
        get
        {
            if (resetValueOnLoad)
                return _placeholderValue;
            else
                return _variable;
        }
        set
        {
            if (resetValueOnLoad)
                _placeholderValue = value;
            else if (!_variable.Equals(value))
            {
                _variable = value;
                if (variableChanged != null)
                    variableChanged.Invoke();
            }
        }
    }

    /// <summary>
    /// When enabled, checks if the value should be reset and if so assigns it to placeholder.
    /// </summary>
    private void OnEnable()
    {
        if (resetValueOnLoad)
            _placeholderValue = _variable;
    }
}
