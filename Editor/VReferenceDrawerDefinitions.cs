using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BoolReference))]
public class BoolReferenceDrawer : VariableReferenceDrawer<bool, SBool> { }

[CustomPropertyDrawer(typeof(FloatReference))]
public class FloatReferenceDrawer : VariableReferenceDrawer<float, SFloat> { }

[CustomPropertyDrawer(typeof(IntReference))]
public class IntReferenceDrawer : VariableReferenceDrawer<int, SInt> { }

[CustomPropertyDrawer(typeof(DoubleReference))]
public class DoubleReferenceDrawer : VariableReferenceDrawer<double, SDouble> { }

[CustomPropertyDrawer(typeof(StringReference))]
public class StringReferenceDrawer : VariableReferenceDrawer<string, SString> { }

[CustomPropertyDrawer(typeof(Vector2Reference))]
public class Vector2ReferenceDrawer : VariableReferenceDrawer<Vector2, SVector2> { }

[CustomPropertyDrawer(typeof(Vector3Reference))]
public class Vector3ReferenceDrawer : VariableReferenceDrawer<Vector3, SVector3> { }

[CustomPropertyDrawer(typeof(GameObjectReference))]
public class GameObjectReferenceDrawer : VariableReferenceDrawer<GameObject, SGameObject> { }

