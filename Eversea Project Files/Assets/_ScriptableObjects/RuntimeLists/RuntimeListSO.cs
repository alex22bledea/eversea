using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A list of items that only exist during runtime
/// </summary>
public abstract class RuntimeListSO<T> : ScriptableObject
{
    public List<T> Items;

    private void OnEnable()
    {
        Items = new List<T>();
    }

    private void OnDisable()
    {
        Items.Clear();
        Items = null;
    }

    public void Add(T item)
    {
        if(!Items.Contains(item))
            Items.Add(item);
    }

    public void Remove(T item)
    {
        if(Items.Contains(item))
            Items.Remove(item);
    }
}
