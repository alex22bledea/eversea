using UnityEngine;

[CreateAssetMenu(fileName = "new Item Data", menuName = "ScriptableObjects/Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int SizeWidth { get; private set; }
    [field: SerializeField] public int SizeHeight { get; private set; }

    [field: SerializeField] public bool[] OccupiedMatrix { get; private set; }

    [field: SerializeField] public Sprite Icon { get; private set; }
}
