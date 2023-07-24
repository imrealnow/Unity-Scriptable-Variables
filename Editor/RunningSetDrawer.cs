using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SetHandler))]
[CanEditMultipleObjects]
public class SetHandlerEditor : Editor
{
    private const float lineHeight = 20;
    private const float lineSpacing = 5;
    private const float fieldSpacing = 4;
    private const float margin = 19f;
    private const float setFieldWidth = 60;
    private const float setCountWidth = 60;
    private const float boxPadding = 2;

    public override void OnInspectorGUI()
    {
        SetHandler _target = target as SetHandler;
        EditorGUILayout.BeginVertical();
        GUIContent setLabel = new GUIContent("Running Set");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("runningSet"), setLabel);
        if(_target.runningSet != null)
            EditorGUILayout.HelpBox("Current object count: " + _target.runningSet.Count.ToString(), MessageType.Info);
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.EndVertical();
    }
}
