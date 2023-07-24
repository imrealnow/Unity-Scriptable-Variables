using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RunningSetHandler))]
[CanEditMultipleObjects]
public class SetHandlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SetHandler _target = target as SetHandler;

        // Running Set field
        SerializedProperty runningSetProperty = serializedObject.FindProperty("runningSet");
        EditorGUILayout.PropertyField(runningSetProperty, new GUIContent("Running Set"));

        // If runningSet is not null, display its current count
        if(_target.runningSet != null)
            EditorGUILayout.HelpBox("Current object count: " + _target.runningSet.Count.ToString(), MessageType.Info);

        // Apply the changes to the serializedObject
        serializedObject.ApplyModifiedProperties();
    }
}
