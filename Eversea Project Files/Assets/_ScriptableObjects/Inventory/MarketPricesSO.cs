
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Market Prices", menuName = "ScriptableObjects/Inventory/MarketPrices")]
public class MarketPricesSO : ScriptableObject
{
    [field: SerializeField] public List<ItemData> ItemList {get; private set;}
    [field: SerializeField] public List<float> PriceList {get; private set;}

    public bool IsInList(ItemData itemData) => ItemList.Contains(itemData);

    public float Price(ItemData itemData) => PriceList[ItemList.IndexOf(itemData)];

}
