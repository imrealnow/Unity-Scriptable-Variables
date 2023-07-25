using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Custom editor for SharedVariableGetter objects
/// </summary>
public abstract class SVGetterEditor<T, S> : Editor
    where S : SharedVariable<T>
{
    private GameObject getterGameObject;
    private SharedVariableGetter<T, S> _target;

    private void OnEnable()
    {
        _target = (SharedVariableGetter<T, S>)target;
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
        _target.UpdateMemberInfos();

        // Exclude the script field and other fields which will be customised
        DrawPropertiesExcluding(serializedObject, new string[] { "m_Script", "fieldName", "targetScript" });

        SerializedProperty targetScriptProperty = serializedObject.FindProperty("targetScript");
        SerializedProperty fieldNameProperty = serializedObject.FindProperty("fieldName");

        // Show the components in a drop down list
        Component[] components = getterGameObject.GetComponents<Component>();
        string[] componentNames = components.Select(c => c.GetType().Name).ToArray();
        int currentComponentIndex = Array.IndexOf(components, targetScriptProperty.objectReferenceValue);
        int newComponentIndex = EditorGUILayout.Popup("Component", currentComponentIndex, componentNames);

        // If a different component has been selected, update the targetScript property
        if (newComponentIndex != currentComponentIndex)
            targetScriptProperty.objectReferenceValue = components[newComponentIndex];

        // If no component is selected, show a message and return
        if (targetScriptProperty.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("Please select a Component.", MessageType.Info);
            serializedObject.ApplyModifiedProperties();
            return;
        }

        // Get the selected component and its type
        Component selectedComponent = (Component)targetScriptProperty.objectReferenceValue;
        Type componentType = selectedComponent.GetType();

        // Get all the public fields and properties of type T
        string[] members = componentType.GetMembers(BindingFlags.Public | BindingFlags.Instance)
            .Where(m => (m is FieldInfo fi && fi.FieldType == typeof(T)) || (m is PropertyInfo pi && pi.PropertyType == typeof(T)))
            .Select(m => m.Name)
            .ToArray();

        // Show the fields/properties in a drop down list
        int currentMemberIndex = Array.IndexOf(members, fieldNameProperty.stringValue);
        int newMemberIndex = EditorGUILayout.Popup("Member Name", currentMemberIndex, members);

        // If a different field/property has been selected, update the fieldName property
        if (newMemberIndex != currentMemberIndex)
            fieldNameProperty.stringValue = members[newMemberIndex];

        // If there are no suitable fields/properties, show a warning
        if (members.Length == 0)
            EditorGUILayout.HelpBox($"The selected component '{selectedComponent.GetType().Name}' does not contain any public fields or properties of type '{typeof(T).Name}'.", MessageType.Warning);

        serializedObject.ApplyModifiedProperties();
    }
}
