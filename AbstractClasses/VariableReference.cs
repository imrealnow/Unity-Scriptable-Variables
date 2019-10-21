using System;
using UnityEngine;

[Serializable]
public abstract class VariableReference<T, S> : IEquatable<VariableReference<T,S>>
    where S : SharedVariable<T>
{
    [SerializeField] private T _value;
    [SerializeField] public S _variable;
    [SerializeField] public bool useConstant = true;

    public T Value
    {
        get
        {
            if (useConstant)
                return _value;
            else if (_variable != null)
                return _variable.Value;
            else
                return default(T);
        }
        set
        {
            if (useConstant)
                _value = value;
            else if (_variable != null)
                _variable.Value = value;
            else
            {
                useConstant = true;
                _value = value;
            }
        }
    }

    public bool Equals(VariableReference<T, S> other)
    {
        return Value.Equals(other.Value);
    }
}
