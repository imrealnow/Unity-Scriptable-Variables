using System;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Class to get a value from another component's field or property and store it in a SharedVariable
/// </summary>
public abstract class SharedVariableGetter<T, S> : MonoBehaviour
    where S : SharedVariable<T>
{
    [Tooltip("SharedVariable to store the value in")]
    public S sharedVariable;

    [Tooltip("Whether to update the SharedVariable with the value every frame")]
    public bool setOnUpdate;

    [Tooltip("Whether to set the SharedVariable with the value on Start")]
    public bool setOnStart;

    [SerializeField, HideInInspector]
    private Component targetScript = null;

    [SerializeField, HideInInspector]
    private string fieldName = "";

    // Cache of the MemberInfos for the field or property to get the value from
    private MemberInfo[] memberInfos;

    public T Value
    {
        get
        {
            UpdateMemberInfos();

            // Traverse the memberInfos to get the final value
            object currentValue = targetScript;
            foreach (MemberInfo memberInfo in memberInfos)
            {
                currentValue = GetValueFromMemberInfo(currentValue, memberInfo);
                if (currentValue == null)
                    return default;

                // Try to convert the current value to T and return it if successful
                if (currentValue is T value)
                    return value;
            }

            Debug.LogError($"Could not convert value from {fieldName} to {typeof(T)}");
            return default;
        }
    }

    private void Start()
    {
        if (setOnStart)
            UpdateSharedVariable();
    }

    private void Update()
    {
        if (setOnUpdate)
            UpdateSharedVariable();
    }

    private void UpdateSharedVariable()
    {
        if (sharedVariable != null)
            sharedVariable.Value = Value;
    }

    private void UpdateMemberInfos()
    {
        // Split the fieldName into parts, and get the MemberInfo for each part
        string[] parts = fieldName.Split('.');
        memberInfos = new MemberInfo[parts.Length];
        Type type = targetScript.GetType();
        for (int i = 0; i < parts.Length; i++)
        {
            memberInfos[i] = type.GetMember(parts[i], BindingFlags.Public | BindingFlags.Instance).FirstOrDefault();
            if (memberInfos[i] == null)
                return;

            // Update type to the type of the next member
            type = GetMemberInfoType(memberInfos[i]);
        }
    }

    private static Type GetMemberInfoType(MemberInfo memberInfo)
    {
        switch (memberInfo)
        {
            case FieldInfo fieldInfo:
                return fieldInfo.FieldType;
            case PropertyInfo propertyInfo:
                return propertyInfo.PropertyType;
            default:
                return null;
        }
    }

    private static object GetValueFromMemberInfo(object obj, MemberInfo memberInfo)
    {
        switch (memberInfo)
        {
            case FieldInfo fieldInfo:
                return fieldInfo.GetValue(obj);
            case PropertyInfo propertyInfo:
                return propertyInfo.GetValue(obj);
            default:
                return null;
        }
    }
}
