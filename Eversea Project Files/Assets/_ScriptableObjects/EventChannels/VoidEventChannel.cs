using System;
using UnityEngine;

[CreateAssetMenu(fileName = "new Void Event Channel", menuName = "Events/Void Event Channel", order = 0)]
public class VoidEventChannel : ScriptableObject
{
    public event Action OnEventRaised;

    public void RaiseEvent() => OnEventRaised?.Invoke();
}
