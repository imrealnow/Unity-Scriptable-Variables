using UnityEngine;

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

    public delegate void OnVariableChanged();
    public OnVariableChanged variableChanged;

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

    private void OnEnable()
    {
        if (resetValueOnLoad)
            _placeholderValue = _variable;
    }
}
