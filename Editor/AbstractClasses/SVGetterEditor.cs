using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public abstract class SVGetterEditor<T, S> : Editor
    where S : SharedVariable<T>
{
    private GameObject getterGameObject;
    private SharedVariableGetter<T, S> _target;
    private bool isOut = true;
    private List<string> options;
    private List<int> selectedIndexes = new List<int>();
    private char[] split = { '.' };

    private void OnEnable()
    {
        _target = (target as SharedVariableGetter<T, S>);
        getterGameObject = _target.gameObject;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.indentLevel = 0;
        EditorGUILayout.BeginHorizontal();
        isOut = EditorGUILayout.Foldout(isOut, "Target Variable");
        EditorGUILayout.Space(40f);
        if (serializedObject.FindProperty("_Value") != null)
        {
            EditorGUILayout.LabelField(serializedObject.FindProperty("fieldName").stringValue);
        }
        EditorGUILayout.EndHorizontal();

        if (!isOut)
            return;

        EditorGUI.indentLevel = 1;

        selectedIndexes.Clear();

        // get components attached to the gameobject
        List<string> componentNames = new List<string>();
        componentNames.Add("");
        foreach (Component t in getterGameObject.GetComponents<Component>())
        {
            componentNames.Add(t.GetType().ToString());
        }

        // if the target script is already set, set the value to its name
        object targetScript = serializedObject.FindProperty("targetScript").objectReferenceValue;
        selectedIndexes.Add(0);
        if (targetScript != null)
            selectedIndexes[0] = componentNames.IndexOf(targetScript.GetType().ToString());
        if (selectedIndexes[0] == -1) selectedIndexes[0] = 0;

        // make the gui control for the target script
        selectedIndexes[0] = EditorGUILayout.Popup("Component", selectedIndexes[0], componentNames.ToArray());
        if (selectedIndexes[0] != 0)
        {
            // set the value to the selected component
            _target.targetScript = getterGameObject.GetComponents<Component>()[selectedIndexes[0] - 1];
            EditorUtility.SetDirty(target);
            targetScript = getterGameObject.GetComponents<Component>()[selectedIndexes[0] - 1];
        }

        if (targetScript == null)
            return;

        string fieldName = "";
        string[] splitstring = serializedObject.FindProperty("fieldName").stringValue.Split(split);

        Type currentType = targetScript.GetType();
        for (int i = 0; i < splitstring.Length; i++)
        {
            selectedIndexes.Add(0);

            options = new List<string>();
            options.Add("");
            MemberInfo[] members = currentType.GetMembers(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (MemberInfo member in members)
            {
                if (member.MemberType == MemberTypes.Field ||
                    member.MemberType == MemberTypes.Property)
                { 
                    object[] atributes = member.GetCustomAttributes(typeof(ObsoleteAttribute), true);
                    if (atributes.Length == 0)
                    {
                        options.Add(member.Name);
                    }
                }
            }

            if (options.Count > 1)
            {
                selectedIndexes[i + 1] = options.IndexOf(splitstring[i]);
                if (selectedIndexes[i + 1] == -1) selectedIndexes[i + 1] = 0;
                selectedIndexes[i + 1] = EditorGUILayout.Popup("Member Name", selectedIndexes[i + 1],
                    options.ToArray());
                if (selectedIndexes[i + 1] != 0)
                {
                    if (i != 0) fieldName += ".";
                    fieldName += options[selectedIndexes[i + 1]];
                }
                MemberInfo[] info = currentType.GetMember(
                    options[selectedIndexes[i + 1]], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (info.Length > 0)
                {
                    if (info[0].MemberType == MemberTypes.Field)
                    {
                        currentType = ((FieldInfo)info[0]).FieldType;
                    }
                    else if (info[0].MemberType == MemberTypes.Property)
                    {
                        currentType = ((PropertyInfo)info[0]).PropertyType;
                    }
                }
            }
            else
            {
                EditorGUILayout.LabelField("No watchable data");
            }
        }

        if (currentType != typeof(T))
        {
            if (fieldName.Length > 0 && fieldName[fieldName.Length - 1] != '.')
            {
                fieldName += ".";
            }
        }
        else
        {
            EditorGUILayout.LabelField("Success! '" + fieldName + "' is the right type.");
        }
        _target.fieldName = fieldName;
    }
}
