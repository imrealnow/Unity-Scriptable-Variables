using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Cooldown))]
public class CooldownDrawer : PropertyDrawer
{
    private static readonly Color greyColor = new Color(0.47f, 0.47f, 0.47f);
    private static readonly float elementSpacing = 2f;
    private static readonly float progressBarHeight = 5f;
    private static readonly float buttonWidth = 52f;

    private Cooldown cooldownObject;

    private void Init(SerializedProperty property)
    {
        cooldownObject = EditorHelper.GetTargetObjectOfProperty(property) as Cooldown;
        if(cooldownObject == null) {
            return;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight + elementSpacing * 2 + progressBarHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (cooldownObject == null)
        {
            Init(property);
        }

        // Get the duration values
        SerializedProperty durationProperty = property.FindPropertyRelative("_duration");
        float duration = durationProperty.floatValue;
        float newDuration = EditorGUI.FloatField(new Rect(position.x, position.y, position.width - 2 * buttonWidth, EditorGUIUtility.singleLineHeight), label, duration);

        // Buttons for updating and resetting duration
        bool durationChanged = !Mathf.Approximately(newDuration, duration);
        GUI.enabled = durationChanged;
        if (GUI.Button(new Rect(position.width - buttonWidth, position.y, buttonWidth, EditorGUIUtility.singleLineHeight), "Update"))
        {
            durationProperty.floatValue = newDuration;
        }
        GUI.enabled = true;

        // Progress bar
        if (cooldownObject != null)
        {
            DrawProgressBar(position, cooldownObject.GetProgressToReset());
        }

        if (Application.isPlaying)
        {
            EditorUtility.SetDirty(property.serializedObject.targetObject);
        }
    }

    private void DrawProgressBar(Rect position, float progress)
    {
        GUI.color = Color.green;
        GUI.Box(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + elementSpacing, position.width * progress, progressBarHeight), GUIContent.none);
        GUI.color = Color.white;
    }
}
