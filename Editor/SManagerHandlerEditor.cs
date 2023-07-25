using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SManagerHandler))]
public class SManagerHandlerEditor : Editor
{
    private List<bool> foldouts = new List<bool>();

    public override void OnInspectorGUI()
    {
        SManagerHandler handler = (SManagerHandler)target;

        for (int i = 0; i < handler.Managers.Count; i++)
        {
            if (foldouts.Count <= i)
            {
                foldouts.Add(false);
            }

            EditorGUILayout.BeginVertical("box"); // Start box

            EditorGUILayout.BeginHorizontal();
            foldouts[i] = EditorGUILayout.Foldout(foldouts[i], handler.Managers[i].name);

            // find SManager in project files
            if (GUILayout.Button(EditorGUIUtility.IconContent("d_ViewToolZoom"), GUILayout.Width(30)))
            {
                EditorGUIUtility.PingObject(handler.Managers[i]);
            }

            // remove SManager
            if (GUILayout.Button(EditorGUIUtility.IconContent("d_TreeEditor.Trash"), GUILayout.Width(30)))
            {
                // Removes manager from the list
                handler.RemoveManagerAt(i);
                DestroyImmediate(handler.Managers[i], true);
                foldouts.RemoveAt(i);
                --i;
                continue;
            }
            EditorGUILayout.EndHorizontal(); // End horizontal layout

            if (foldouts[i])
            {
                Editor sManagerEditor = CreateEditor(handler.Managers[i]);
                sManagerEditor.OnInspectorGUI();
            }

            EditorGUILayout.EndVertical(); // End box
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Add SManager"))
        {
            var menu = new GenericMenu();

            foreach (var type in GetUninstantiatedSubclassesOfSManager(handler))
            {
                menu.AddItem(new GUIContent(type.Name), false, () =>
                {
                    // Create a new instance of SManager
                    var sManager = ScriptableObject.CreateInstance(type) as SManager;

                    // Open save file dialog
                    string path = EditorUtility.SaveFilePanel("Save Scriptable Object", "Assets/", type.Name, "asset");

                    if (!string.IsNullOrEmpty(path))
                    {
                        // Unity's path is relative to the project folder, so remove the project's absolute path.
                        path = path.Replace(Application.dataPath, "");

                        // Ensure the path starts with "Assets/"
                        if (!path.StartsWith("/")) path = "/" + path;
                        path = "Assets" + path;

                        // Create an asset at the specified path
                        AssetDatabase.CreateAsset(sManager, path);
                        AssetDatabase.SaveAssets();

                        handler.AddManager(sManager);
                    }
                });
            }

            menu.ShowAsContext();
        }
    }

    private List<Type> GetUninstantiatedSubclassesOfSManager(SManagerHandler handler)
    {
        var subclasses = new List<Type>();
        var instantiatedTypes = new HashSet<Type>(handler.Managers.Select(m => m.GetType()));

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(SManager)) && !type.IsAbstract && !instantiatedTypes.Contains(type))
                {
                    subclasses.Add(type);
                }
            }
        }
        return subclasses;
    }
}
