using System;
using System.Reflection;
using UnityEngine;

public abstract class SharedVariableGetter<T, S> : MonoBehaviour
    where S : SharedVariable<T>
{
    public S sharedVariable;
    public bool setOnUpdate;
    public bool setOnStart;

    [SerializeField, HideInInspector] private Component targetScript = null;
    public Component TargetScript => targetScript;

    [SerializeField, HideInInspector] private string fieldName = "";
    public string FieldName => fieldName;

    string[] splitFieldName;
    MemberInfo[] memberInfo;

    char[] split = { '.' };

    public void UpdateFieldInfo()
    {
        if (TargetScript == null)
        {
            memberInfo = new MemberInfo[0];
            return;
        }
        splitFieldName = FieldName.Split(split);
        if (splitFieldName.Length < 1) return;
        memberInfo = new MemberInfo[splitFieldName.Length];
        var type = TargetScript.GetType();
        for (int i = 0; i < memberInfo.Length; i++)
        {
            MemberInfo[] tempArray = type.GetMember(splitFieldName[i], BindingFlags.Public | BindingFlags.Instance);
            if (tempArray.Length < 1) return;
            memberInfo[i] = tempArray[0];
            if (tempArray[0].MemberType == MemberTypes.Property)
            {
                type = ((PropertyInfo)tempArray[0]).PropertyType;
            }
            else if (tempArray[0].MemberType == MemberTypes.Field)
            {
                type = ((FieldInfo)tempArray[0]).FieldType;
            }
            else
            {
                return;
            }
        }
    }

    [SerializeField, HideInInspector] T _Value;

    public T Value
    {
        get
        {
            _Value = default(T);
            UpdateFieldInfo();
            object currentTarget = TargetScript;
            for (int i = 0; i < memberInfo.Length; i++)
            {
                if (memberInfo[i] == null) return default(T);
                if (memberInfo[i].MemberType == MemberTypes.Field)
                {
                    currentTarget = ((FieldInfo)memberInfo[i]).GetValue(currentTarget);
                }
                else if (memberInfo[i].MemberType == MemberTypes.Property)
                {
                    currentTarget = ((PropertyInfo)memberInfo[i]).GetValue(currentTarget, null);
                }
                else
                {
                    return default(T);
                }
                if (currentTarget == null) break;
            }
            try
            {
                _Value = (T)Convert.ChangeType(currentTarget, typeof(T));
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

            return _Value;
        }
    }

    private void Start()
    {
        if (setOnStart && sharedVariable != null)
            sharedVariable.Value = Value;
    }

    private void Update()
    {
        if (!setOnUpdate || sharedVariable == null)
            return;

        sharedVariable.Value = Value;
    }
}
