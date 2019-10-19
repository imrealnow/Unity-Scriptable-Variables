using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SBoolGetter))]
[CanEditMultipleObjects]
public class SBoolGetterEditor : SVGetterEditor<bool, SBool> { }

[CustomEditor(typeof(SFloatGetter))]
[CanEditMultipleObjects]
public class SFloatGetterEditor : SVGetterEditor<float, SFloat> { }

[CustomEditor(typeof(SIntGetter))]
[CanEditMultipleObjects]
public class SIntGetterEditor : SVGetterEditor<int, SInt> { }

[CustomEditor(typeof(SDoubleGetter))]
[CanEditMultipleObjects]
public class SDoubleGetterEditor : SVGetterEditor<double, SDouble> { }

[CustomEditor(typeof(SStringGetter))]
[CanEditMultipleObjects]
public class SStringGetterEditor : SVGetterEditor<string, SString> { }

[CustomEditor(typeof(SVector2Getter))]
[CanEditMultipleObjects]
public class SVector2GetterEditor : SVGetterEditor<Vector2, SVector2> { }

[CustomEditor(typeof(SVector3Getter))]
[CanEditMultipleObjects]
public class SVector3GetterEditor : SVGetterEditor<Vector3, SVector3> { }

[CustomEditor(typeof(SGameObjectGetter))]
[CanEditMultipleObjects]
public class SGameObjectGetterEditor : SVGetterEditor<GameObject, SGameObject> { }

