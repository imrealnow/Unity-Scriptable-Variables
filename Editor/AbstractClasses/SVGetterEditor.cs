using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public abstract class SVGetterEditor<T, S> : Editor
    where S : SharedVariable<T>
{
    private GameObject getterGameObject;
    private SharedVariableGetter<T, S> _target;

    private void OnEnable()
    {
        if (target == null)
        {
            Debug.LogError("Editor target is null!");
            return;
        }

        _target = (target as SharedVariableGetter<T, S>);
        if (_target == null)
        {
            Debug.LogError("Editor target cannot be cast to SharedVariableGetter!");
            return;
        }

        getterGameObject = _target.gameObject;
        if (getterGameObject == null)
        {
            Debug.LogError("Getter GameObject is null!");
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        _target.UpdateFieldInfo();

        // Draw default inspector properties
        DrawPropertiesExcluding(serializedObject, new string[] { "m_Script", "fieldName", "targetScript" });

        var targetScriptProperty = serializedObject.FindProperty("targetScript");
        var fieldNameProperty = serializedObject.FindProperty("fieldName");

        EditorGUILayout.Space();

        string[] componentNames = getterGameObject.GetComponents<Component>()
            .Select(c => c.GetType().Name)
            .ToArray();


        int currentIndex = -1; // Default to -1 indicating no selection
        if (targetScriptProperty.objectReferenceValue != null)
        {
            currentIndex = Array.IndexOf(componentNames, targetScriptProperty.objectReferenceValue.GetType().Name);
        }

        int newIndex = EditorGUILayout.Popup("Component", currentIndex, componentNames);

        if (newIndex != currentIndex)
        {
            targetScriptProperty.objectReferenceValue = getterGameObject.GetComponents<Component>()[newIndex];
        }

        if (targetScriptProperty.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("Please select a Component.", MessageType.Info);
            serializedObject.ApplyModifiedProperties();
            return;
        }

        // Get selected component
        Component selectedComponent = (Component)targetScriptProperty.objectReferenceValue;
        Type componentType = selectedComponent.GetType();

        // Get all the public fields and properties of the component type that match the type T
        List<string> fieldsAndProperties = componentType
            .GetMembers(BindingFlags.Public | BindingFlags.Instance)
            .Where(member => member.MemberType == MemberTypes.Field && ((FieldInfo)member).FieldType == typeof(T) ||
                             member.MemberType == MemberTypes.Property && ((PropertyInfo)member).PropertyType == typeof(T))
            .Select(member => member.Name)
            .ToList();


        int currentSelectedIndex = fieldsAndProperties.IndexOf(fieldNameProperty.stringValue);
        if (currentSelectedIndex == -1) currentSelectedIndex = 0;

        if (fieldsAndProperties.Any())
        {
            // Allow to choose a field/property
            int newSelectedIndex = EditorGUILayout.Popup("Member Name", currentSelectedIndex, fieldsAndProperties.ToArray());

            // If selection has been made, update the field name
            if (newSelectedIndex >= 0)
            {
                fieldNameProperty.stringValue = fieldsAndProperties[newSelectedIndex];
            }
        }
        else
        {
            EditorGUILayout.HelpBox($"The selected component '{targetScriptProperty.objectReferenceValue.GetType().Name}' does not contain any public fields or properties of type {typeof(T).Name}.", MessageType.Warning);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
