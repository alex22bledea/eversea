using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A list of items set in the editor. Must NOT be changed during runtime
/// </summary>
public abstract class PresetListSO<T> : ScriptableObject
{
    public List<T> Items;
}
