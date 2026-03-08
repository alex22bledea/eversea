
using UnityEngine;

public class ReferenceVariableSO<T> : VariableSO<T> where T : class
{
    private void OnDisable()
    {
        SetValue(default(T));
    }
}
