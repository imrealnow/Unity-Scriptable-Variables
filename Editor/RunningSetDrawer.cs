using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RunningSetHandler))]
[CanEditMultipleObjects]
public class SetHandlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RunningSetHandler _target = target as RunningSetHandler;

        // Running Set field
        SerializedProperty runningSetProperty = serializedObject.FindProperty("runningSet");
        EditorGUILayout.PropertyField(runningSetProperty, new GUIContent("Running Set"));

        // If runningSet is not null, display its current count
        if (_target.RunningSet != null)
            EditorGUILayout.HelpBox("Current object count: " + _target.RunningSet.Count.ToString(), MessageType.Info);

        // Apply the changes to the serializedObject
        serializedObject.ApplyModifiedProperties();
    }
}
