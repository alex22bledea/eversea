using UnityEngine;

public abstract class VariableSO<T> : ScriptableObject
{

#if UNITY_EDITOR
    [Multiline, SerializeField]
    private string DeveloperDescription = "";
#endif

    [field: SerializeField] public T Value { get; private set; }

    public void SetValue(T value) => Value = value;

    // public void SetValue(VariableSO<T> varSO) => Value = varSO.Value;
}
