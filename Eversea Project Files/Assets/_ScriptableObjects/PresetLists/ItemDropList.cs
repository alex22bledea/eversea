
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "new ItemDropList", menuName = "ScriptableObjects/Inventory/ItemDropList")]
public class ItemDropList : PresetListSO<ItemDrop>
{
    [Space(12)]
    [SerializeField] private float totalChance;

    public ItemData GetRandomItem()
    {
        if(Items.Count == 0)
        {
            Debug.LogError("Item Drop List is empty!", this);
            return null;
        }

        float cumulatice = 0;
        float rand = UnityEngine.Random.value;

        foreach (ItemDrop itemDrop in Items)
        {
            cumulatice += itemDrop.Chance;
            if (rand <= cumulatice)
                return itemDrop.Item;
        }

        Debug.LogError("ItemDrop List was not made properly, resulting in a fallback", this);
        return Items.GetRandomElement().Item;
    }

    private void OnValidate()
    {
        totalChance = 0f;

        foreach (ItemDrop itemDrop in Items)
            totalChance += itemDrop.Chance;
    }
}

[Serializable]
public struct ItemDrop
{
    [field: SerializeField] public ItemData Item { get; private set; }
    [field: SerializeField] public float Chance { get; private set; }
}