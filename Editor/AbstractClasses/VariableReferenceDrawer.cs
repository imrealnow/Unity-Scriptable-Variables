using UnityEditor;
using UnityEngine;

public abstract class VariableReferenceDrawer<T, S> : PropertyDrawer
    where S : SharedVariable<T>
{
    private readonly float lineHeight = 20;
    private readonly float lineSpacing = 6;
    private readonly float buttonWidth = 100;
    private readonly float foldoutWidth = 14;
    private readonly float boxPadding = 2;

    private bool foldout = true;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return foldout ? (lineHeight + lineSpacing) * 2 : lineHeight + lineSpacing * 2;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        bool useConstant = property.FindPropertyRelative("useConstant").boolValue;
        float editorWidth = EditorGUIUtility.currentViewWidth;

        Color startGUIColor = GUI.color;
        Color transparentColor = startGUIColor;
        transparentColor.a = 0;

        Rect boxRect;
        Rect foldoutRect = new Rect(position.x + lineSpacing * 3, position.y + lineSpacing, foldoutWidth, lineHeight);
        Rect labelRect = new Rect(
                position.x + lineSpacing + foldoutWidth,
                position.y + lineSpacing,
                editorWidth - buttonWidth - foldoutWidth - lineSpacing * 4,
                lineHeight
            );
        Rect buttonRect = new Rect(
                editorWidth - buttonWidth - lineSpacing,
                position.y + lineSpacing,
                buttonWidth,
                lineHeight
            );
        Rect foldoutButtonRect = new Rect(
                position.x,
                position.y + lineSpacing,
                editorWidth - buttonWidth - foldoutWidth - lineSpacing * 3,
                lineHeight
            );

        if (foldout)
        {
            boxRect = new Rect(
                position.x - boxPadding,
                position.y + lineSpacing - boxPadding,
                position.width + boxPadding * 2,
                lineHeight * 2 + boxPadding * 3
            );
        }
        else
        {
            boxRect = new Rect(
                position.x - boxPadding,
                position.y + lineSpacing - boxPadding,
                position.width + boxPadding * 2,
                lineHeight + boxPadding * 2
            );
        }

        GUI.Box(boxRect, GUIContent.none);
        EditorGUI.Foldout(foldoutRect, foldout, GUIContent.none);
        EditorGUI.LabelField(labelRect, label);

        // invisible foldout button
        GUI.color = transparentColor;
        if (GUI.Button(foldoutButtonRect, ""))
            foldout = !foldout;
        GUI.color = startGUIColor;

        if (GUI.Button(buttonRect, useConstant ? "Constant" : "Variable"))
        {
            useConstant = !useConstant;
            property.FindPropertyRelative("useConstant").boolValue = useConstant;
        }

        if (!foldout)
            return;

        Rect propertyRect = new Rect(position.x + lineSpacing, position.y + lineHeight + lineSpacing, position.width - lineSpacing, lineHeight);
        EditorGUI.PropertyField(propertyRect, property.FindPropertyRelative(useConstant ? "_value" : "_variable"), GUIContent.none);
    }
}
