using UnityEngine;
using UnityEngine.Events;

public abstract class ReferenceEqualityChecker<T, S, R> : MonoBehaviour
    where S : SharedVariable<T>
    where R : VariableReference<T, S>
{
    public R referenceA, referenceB;
    public UnityEvent whenTrue;

    public bool CompareReferences(R referenceA, R referenceB)
    {
        return referenceA.Equals(referenceB);
    }

    public void CompareReferences()
    {
        if (whenTrue != null && referenceA != null && referenceB != null)
        {
            if (CompareReferences(referenceA, referenceB))
                whenTrue.Invoke();
        }
    }
}


