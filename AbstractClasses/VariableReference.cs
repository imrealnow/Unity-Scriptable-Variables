using System;
using UnityEngine;

/// <summary>
/// Class for handling a reference that can either be a constant value or a reference to a shared variable
/// </summary>
[Serializable]
public abstract class VariableReference<T, S> : IEquatable<VariableReference<T,S>>
    where S : SharedVariable<T>
{
    [SerializeField] private T _value; // the constant value
    [SerializeField] public S _variable; // the shared variable
    [SerializeField] public bool useConstant = true; // toggle to determine if a constant or shared variable should be used

    /// <summary>
    /// The value of the reference, which can be either a constant value or the value of a shared variable.
    /// </summary>
    public T Value
    {
        get
        {
            // return either the constant value or the value of the shared variable
            if (useConstant)
                return _value;
            else if (_variable != null)
                return _variable.Value;
            else
                return default(T);
        }
        set
        {
            // set either the constant value or the value of the shared variable
            if (useConstant)
                _value = value;
            else if (_variable != null)
                _variable.Value = value;
            else
            {
                // if there's no shared variable, store the value as a constant
                useConstant = true;
                _value = value;
            }
        }
    }

    /// <summary>
    /// Check if this VariableReference equals another by comparing their values.
    /// </summary>
    public bool Equals(VariableReference<T, S> other)
    {
        return Value.Equals(other.Value);
    }
}
