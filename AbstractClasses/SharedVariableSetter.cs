using UnityEngine;
using System.Collections;
using System;
public abstract class SharedVariableSetter<T, S> : MonoBehaviour
    where S : SharedVariable<T>
{
    public T _value;
    public S _sharedVariable;

    public bool setOnStart;

    void Start()
    {
        if (setOnStart && _sharedVariable != null)
            _sharedVariable.Value = _value;
    }

    public void SetValue(T value)
    {
        if (_sharedVariable == null)
            return;

        _sharedVariable.Value = value;
    }

    public void CopyOtherSharedVariable(S sharedVariable)
    {
        if (_sharedVariable == null)
            return;

        _sharedVariable.Value = sharedVariable.Value;
    }
}

