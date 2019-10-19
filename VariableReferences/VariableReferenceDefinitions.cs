using System;
using UnityEngine;

[Serializable]
public class BoolReference : VariableReference<bool, SBool> { }

[Serializable]
public class FloatReference : VariableReference<float, SFloat> { }

[Serializable]
public class IntReference : VariableReference<int, SInt> { }

[Serializable]
public class DoubleReference : VariableReference<double, SDouble> { }

[Serializable]
public class StringReference : VariableReference<string, SString> { }

[Serializable]
public class Vector2Reference : VariableReference<Vector2, SVector2> { }

[Serializable]
public class Vector3Reference : VariableReference<Vector3, SVector3> { }

[Serializable]
public class GameObjectReference : VariableReference<GameObject, SGameObject> { }

