using System;
using UnityEngine;

public abstract class GenericEventChannelSO<T> : ScriptableObject
{
    [Tooltip("The action to perform. Listeners subscribe to this Action")]
    public event Action<T> OnEventRaised;
    
    public void RaiseEvent(T parameter) => OnEventRaised?.Invoke(parameter);
}
