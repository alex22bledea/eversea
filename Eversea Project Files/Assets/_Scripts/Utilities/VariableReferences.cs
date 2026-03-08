using System;
using UnityEngine;

[Serializable]
public class Vector3Reference
{
    [SerializeField] private bool UseConstant = true;
    [SerializeField] private Vector3 ConstantValue;
    [SerializeField] private Vector3VariableSO Variable;

    public Vector3 Value{
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }

    public Vector3Reference(Vector3 value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public static implicit operator Vector3(Vector3Reference reference)
    {
        return reference.Value;
    }
}
