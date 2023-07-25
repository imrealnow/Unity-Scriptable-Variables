using UnityEditor;
using UnityEngine;

public abstract class VariableReferenceDrawer<T, S> : PropertyDrawer where S : SharedVariable<T>
{
    private const string buttonLabelConstant = "Const";
    private const string buttonLabelVariable = "Var";
    private readonly float lineSpacing = 6;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty useConstantProperty = property.FindPropertyRelative("useConstant");
        bool useConstant = useConstantProperty.boolValue;
        SerializedProperty selectedProperty = property.FindPropertyRelative(useConstant ? "_value" : "_variable");

        // Get the height of the property field
        float propertyHeight = EditorGUI.GetPropertyHeight(selectedProperty, true);

        return EditorGUIUtility.singleLineHeight + lineSpacing * 4 + propertyHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty useConstantProperty = property.FindPropertyRelative("useConstant");
        bool useConstant = useConstantProperty.boolValue;

        // Compute button width based on the size of the text
        var buttonLabel = useConstant ? buttonLabelConstant : buttonLabelVariable;
        var buttonWidth = GUI.skin.button.CalcSize(new GUIContent(buttonLabel)).x + lineSpacing;

        // Create layout rectangles
        Rect labelRect = new Rect(position.x + lineSpacing, position.y + lineSpacing, position.width - buttonWidth - lineSpacing * 3, EditorGUIUtility.singleLineHeight);
        Rect buttonRect = new Rect(position.x + position.width - buttonWidth - lineSpacing, position.y + lineSpacing, buttonWidth, EditorGUIUtility.singleLineHeight);

        // Draw the label and the button
        EditorGUI.LabelField(labelRect, label);
        if (GUI.Button(buttonRect, buttonLabel))
        {
            useConstant = !useConstant;
            useConstantProperty.boolValue = useConstant;
        }

        // Draw the selected property
        SerializedProperty selectedProperty = property.FindPropertyRelative(useConstant ? "_value" : "_variable");
        Rect propertyRect = new Rect(position.x + lineSpacing, position.y + EditorGUIUtility.singleLineHeight + lineSpacing * 2, position.width - lineSpacing * 2, position.height - EditorGUIUtility.singleLineHeight - lineSpacing * 4);
        EditorGUI.PropertyField(propertyRect, selectedProperty, new GUIContent(""));

        // Draw the box encompassing everything
        Rect boxRect = new Rect(position.x, position.y, position.width, position.height - lineSpacing);
        GUI.Box(boxRect, GUIContent.none);
    }
}
