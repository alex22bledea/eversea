using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    new private List<TKey> Keys = new List<TKey>();

    [SerializeField]
    new private List<TValue> Values = new List<TValue>();


    // save the dictionary to lists
    public void OnBeforeSerialize()
    {
        // Not needed in my case
        /*
        Keys.Clear();
        Values.Clear();

        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            Keys.Add(pair.Key);
            Values.Add(pair.Value);
        }
        */
    }

    // load dictionary from lists
    public void OnAfterDeserialize()
    {
        this.Clear();

        if (Keys.Count != Values.Count)
            throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable.", Keys.Count, Values.Count));
        else
            for (int i = 0; i < Keys.Count; i++)
                this.Add(Keys[i], Values[i]);
    }
}
