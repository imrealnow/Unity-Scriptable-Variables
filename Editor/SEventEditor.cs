using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SEvent))]
public class SEventEditor : Editor
{
    SEvent _target;

    public void OnEnable()
    {
        _target = target as SEvent;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Fire"))
            _target.Fire();
    }
}
