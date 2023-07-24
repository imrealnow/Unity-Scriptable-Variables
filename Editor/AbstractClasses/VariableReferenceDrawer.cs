using UnityEditor;
using UnityEngine;

public abstract class VariableReferenceDrawer<T, S> : PropertyDrawer
    where S : SharedVariable<T>
{
    private readonly float lineHeight = 20;
    private readonly float lineSpacing = 6;
    private bool foldout = true;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return foldout ? (lineHeight + lineSpacing) * 3 : lineHeight + lineSpacing * 2;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty useConstantProperty = property.FindPropertyRelative("useConstant");
        bool useConstant = useConstantProperty.boolValue;
        
        // Create layout rectangles
        Rect boxRect = new Rect(position.x, position.y + lineSpacing, position.width, foldout ? lineHeight * 2 : lineHeight);
        Rect foldoutRect = new Rect(position.x + lineSpacing, position.y + lineSpacing, lineHeight, lineHeight);
        Rect labelRect = new Rect(position.x + lineHeight + lineSpacing * 2, position.y + lineSpacing, position.width - lineHeight * 3 - lineSpacing * 4, lineHeight);
        Rect buttonRect = new Rect(position.width - lineHeight * 2, position.y + lineSpacing, lineHeight * 2, lineHeight);
        Rect propertyRect = new Rect(position.x + lineSpacing, position.y + lineHeight + lineSpacing * 2, position.width - lineSpacing * 2, lineHeight);
        
        EditorGUI.LabelField(labelRect, label);

        // Button to toggle between constant and variable
        if (GUI.Button(buttonRect, useConstant ? "Const" : "Var"))
        {
            useConstant = !useConstant;
            useConstantProperty.boolValue = useConstant;
        }

        // Foldout for extra properties
        foldout = EditorGUI.Foldout(foldoutRect, foldout, GUIContent.none);

        if (!foldout)
            return;

        // Draw the selected property
        SerializedProperty selectedProperty = property.FindPropertyRelative(useConstant ? "_value" : "_variable");
        EditorGUI.PropertyField(propertyRect, selectedProperty, GUIContent.none);
    }
}
