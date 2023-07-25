using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Cooldown))]
public class CooldownDrawer : PropertyDrawer
{
    private static readonly Color greyColor = new(0.47f, 0.47f, 0.47f);
    private static readonly Color progressBarColor = new Color(0.26f, 0.6f, 0.22f);
    private static readonly float elementSpacing = 2f;
    private static readonly float progressBarHeight = 8f;
    private static readonly float buttonWidth = 100f;

    private Cooldown cooldownObject;

    private void Init(SerializedProperty property)
    {
        cooldownObject = EditorHelper.GetTargetObjectOfProperty(property) as Cooldown;
        if (cooldownObject == null)
        {
            return;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 2 + elementSpacing * 3 + progressBarHeight; // Adjust the height to accommodate the new field
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (cooldownObject == null)
        {
            Init(property);
        }

        // Enclosing box
        GUI.Box(new Rect(position.x, position.y, position.width, position.height - progressBarHeight), GUIContent.none);

        // Adjust position within the box
        position = new Rect(position.x + 3f, position.y + 2f, position.width - 6f, position.height - 4f);

        float originalLabelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 125f; // Adjust according to your preference

        // Begin a horizontal group
        EditorGUI.BeginChangeCheck();

        // Cooldown Duration
        EditorGUI.PrefixLabel(
            new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight),
            label);
        SerializedProperty durationProperty = property.FindPropertyRelative("_duration");

        // Duration field and Use button
        Rect remainingRect = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        Rect durationRect = remainingRect;
        durationRect.width -= buttonWidth + elementSpacing;
        durationRect.height = EditorGUIUtility.singleLineHeight;

        float newDuration = EditorGUI.FloatField(durationRect, durationProperty.floatValue);

        // Check if the duration has been changed in the GUI
        if (EditorGUI.EndChangeCheck())
        {
            durationProperty.floatValue = newDuration;
        }

        // Add "Use" button
        GUI.enabled = Application.isPlaying;
        Rect buttonRect = remainingRect;
        buttonRect.x = durationRect.xMax + elementSpacing;
        buttonRect.width = buttonWidth;
        buttonRect.height = EditorGUIUtility.singleLineHeight;

        if (GUI.Button(buttonRect, "Use Cooldown") && Application.isPlaying)
        {
            cooldownObject.TryUseCooldown();
        }
        GUI.enabled = true;

        // Add remaining time label
        float remainingTime = Application.isPlaying ? cooldownObject.RemainingTime : 0f;  // Here
        EditorGUI.LabelField(
            new Rect(
                position.x + elementSpacing,
                position.y + EditorGUIUtility.singleLineHeight + elementSpacing * 2,
                position.width - elementSpacing * 2,
                EditorGUIUtility.singleLineHeight),
            "Remaining Time: " + remainingTime.ToString(),  // And here
            EditorStyles.boldLabel
        );

        // Progress bar
        if (cooldownObject != null)
        {
            DrawProgressBar(
                new Rect(
                    position.x,
                    position.y + EditorGUIUtility.singleLineHeight * 2 + elementSpacing,
                    position.width,
                    progressBarHeight),
                cooldownObject.ProgressToReset
            );
        }

        if (Application.isPlaying)
        {
            EditorUtility.SetDirty(property.serializedObject.targetObject);
        }

        EditorGUIUtility.labelWidth = originalLabelWidth; // Restore original label width
    }


    private void DrawProgressBar(Rect position, float progress)
    {
        Color originalColor = GUI.color;

        GUI.color = greyColor;
        GUI.Box(position, GUIContent.none);

        GUI.color = progressBarColor;
        float width = Mathf.Clamp(position.width * progress, 0, position.width);
        Texture2D progressTexture = new Texture2D(1, 1);
        progressTexture.SetPixel(0, 0, GUI.color);
        progressTexture.Apply();
        GUI.DrawTexture(new Rect(position.x, position.y, width, progressBarHeight), progressTexture);

        GUI.color = originalColor;
    }
}
