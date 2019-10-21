using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Cooldown))]
public class CooldownDrawer : PropertyDrawer
{
    private readonly Color greyColor = new Color(120, 120, 120);
    private readonly float elementSpacing = 2f;
    private readonly float progressBarHeight = 5f;
    private readonly float buttonWidth = 52f;

    private float labelWidth;
    private float floatWidth;

    private Color startGUIColor;
    private float startDuration;
    private float currentDuration;
    private bool isInitialised;
    private Cooldown cooldownObject;

    private void Init(SerializedProperty property)
    {
        cooldownObject = EditorHelper.GetTargetObjectOfProperty(property) as Cooldown;
        startGUIColor = GUI.color;
        startDuration = property.FindPropertyRelative("_duration").floatValue;
        isInitialised = true;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight + elementSpacing * 2 + progressBarHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (!isInitialised)
        {
            Init(property);
            return;
        }

        // get widths for elements
        CalculateWidths();

        // get duration values
        currentDuration = property.FindPropertyRelative("_duration").floatValue;
        bool durationsAreEqual = startDuration == currentDuration;

        // try to get current cooldown progress
        bool progressAvailable = cooldownObject != null;
        float progressPercent;
        Rect progressRect = new Rect();

        //make progress bar if the percentage is available
        if (progressAvailable)
        {
            progressPercent = cooldownObject.GetProgressToReset();

            progressRect = new Rect(
                position.x,
                position.y + EditorGUIUtility.singleLineHeight + elementSpacing,
                EditorGUIUtility.currentViewWidth * progressPercent,
                progressBarHeight
                );
        }
        
        // make the rects for the elements
        Rect labelRect = new Rect(position.x, position.y, labelWidth, EditorGUIUtility.singleLineHeight);
        position.x = EditorGUIUtility.currentViewWidth - buttonWidth;
        Rect resetButtonRect = new Rect(position.x, position.y, buttonWidth - elementSpacing, EditorGUIUtility.singleLineHeight);
        position.x -= buttonWidth + elementSpacing;
        Rect updateButtonRect = new Rect(position.x, position.y, buttonWidth, EditorGUIUtility.singleLineHeight);
        position.x -= floatWidth + elementSpacing;
        Rect durationRect = new Rect(position.x, position.y, floatWidth, EditorGUIUtility.singleLineHeight);

        // label and float field
        EditorGUI.LabelField(labelRect, label);
        startDuration = EditorGUI.FloatField(durationRect, startDuration);

        // contextual buttons
        GUI.enabled = !durationsAreEqual;
        if (GUI.Button(updateButtonRect, "Update"))
            property.FindPropertyRelative("_duration").floatValue = startDuration;
        if(GUI.Button(resetButtonRect, "Reset"))
            startDuration = property.FindPropertyRelative("_duration").floatValue;
        GUI.enabled = true;
        if (progressAvailable)
        {
            GUI.color = Color.green;
            GUI.Box(progressRect, GUIContent.none);
            GUI.color = startGUIColor;
        }

        if(Application.isPlaying)
            EditorUtility.SetDirty(property.serializedObject.targetObject);
    }

    private void CalculateWidths()
    {
        float viewWidth =
            EditorGUIUtility.currentViewWidth
            - (elementSpacing * 4f)
            - buttonWidth * 2;
        labelWidth = EditorGUIUtility.labelWidth;
        floatWidth = viewWidth - labelWidth - elementSpacing;
    }
}
