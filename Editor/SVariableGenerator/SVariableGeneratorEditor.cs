using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SVariableGeneratorEditor : EditorWindow
{
    private enum ScriptType
    {
        SharedVariable,
        SharedEvent
    }

    private static readonly string SharedVariableTemplate = @"
using UnityEngine;

[CreateAssetMenu(fileName = ""#CLASSNAME#"", menuName = ""SO/#CLASSNAME#"", order = 1)]
public class S#CLASSNAME# : SharedVariable<#CLASSNAME#> { }";

    private static readonly string SharedVariableGetterTemplate = @"
public class S#CLASSNAME#Getter : SharedVariableGetter<#CLASSNAME#, S#CLASSNAME#> { }";

    private static readonly string VariableReferenceTemplate = @"
using System;
using UnityEngine;

[Serializable]
public class #CLASSNAME#Reference : VariableReference<#CLASSNAME#, S#CLASSNAME#> { }";

    private static readonly string SharedEventTemplate = @"
using UnityEngine;

[CreateAssetMenu(fileName = ""S#CLASSNAME#Event"", menuName = ""SO/Event/S#CLASSNAME#"")]
public class S#CLASSNAME#Event : SEvent<#CLASSNAME#>{ }";

    private string className = "NewClass";
    private List<string> generatedFilePaths;
    private ScriptType currentScriptType = ScriptType.SharedVariable;

    [MenuItem("Window/SO/Shared Variable \\ Event Extension Generator")]
    static void OpenWindow()
    {
        SVariableGeneratorEditor window = GetWindow<SVariableGeneratorEditor>();
        window.titleContent = new GUIContent("Shared Variable Extender");
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Generate SharedVariable / Event Extension", EditorStyles.boldLabel);
        className = EditorGUILayout.TextField("Type Name", className);

        // Dropdown to select the script type
        currentScriptType = (ScriptType)EditorGUILayout.EnumPopup("Script Type", currentScriptType);

        if (GUILayout.Button("Generate"))
        {
            GenerateSharedVariableExtension();
        }
    }

    private void GenerateSharedVariableExtension()
    {
        generatedFilePaths = new List<string>();
        switch (currentScriptType)
        {
            case ScriptType.SharedVariable:
                GenerateFromTemplate(SharedVariableTemplate, $"Assets/S{className}.cs");
                GenerateFromTemplate(SharedVariableGetterTemplate, $"Assets/S{className}Getter.cs");
                GenerateFromTemplate(VariableReferenceTemplate, $"Assets/{className}Reference.cs");
                break;
            case ScriptType.SharedEvent:
                GenerateFromTemplate(SharedEventTemplate, $"Assets/S{className}Event.cs");
                break;
        }

        AssetDatabase.Refresh();

        // Print the paths to the console
        foreach (string path in generatedFilePaths)
        {
            Debug.Log("Generated file at: " + path);
        }

        // Highlight one of the generated files in the project hierarchy
        if (generatedFilePaths.Any())
        {
            var obj = AssetDatabase.LoadAssetAtPath<Object>(generatedFilePaths.First());
            EditorGUIUtility.PingObject(obj);
        }
    }

    private void GenerateFromTemplate(string template, string path)
    {
        string code = template.Replace("#CLASSNAME#", className);
        File.WriteAllText(path, code);
        generatedFilePaths.Add(path);
    }
}
