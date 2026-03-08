
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [field: SerializeField] public ItemData ItemData { get; private set; }

    private Vector2Int gridPosition;
    private int rotationCount;
    private RectTransform rectTransform;

    public Vector2Int GetCurrentGridPosition() => gridPosition;

    public void SetCurrentGridPosition(Vector2Int newGridPosition) => gridPosition = newGridPosition;

    public int GetRotationCount() => rotationCount;

    public void RotateOnce()
    {
        rotationCount++;
        rotationCount %= 4;
        rectTransform.rotation = Quaternion.Euler(0, 0, 90 * (4 - rotationCount));
    }

    public void SetRotation(int value)
    {
        value %= 4;
        rotationCount = value;
        rectTransform.rotation = Quaternion.Euler(0, 0, 90 * (4 - rotationCount));
    }

    public bool IsOccupied(int w, int h)
    {
        switch (rotationCount)
        {
            case 0:
                return ItemData.OccupiedMatrix[w * ItemData.SizeHeight + h];
            case 1:
                return ItemData.OccupiedMatrix[(ItemData.SizeWidth - h - 1) * ItemData.SizeHeight + w];
            case 2:
                return ItemData.OccupiedMatrix[(ItemData.SizeWidth - w - 1) * ItemData.SizeHeight + ItemData.SizeHeight - h - 1];
            case 3:
                return ItemData.OccupiedMatrix[h * ItemData.SizeHeight + ItemData.SizeHeight - w - 1];
        }

        Debug.LogError("width and height do not match queried item");
        return false;
    }

    public int GetCurrentWidth() => rotationCount % 2 == 0 ? ItemData.SizeWidth : ItemData.SizeHeight;

    public int GetCurrentHeight() => rotationCount % 2 == 0 ? ItemData.SizeHeight : ItemData.SizeWidth;

    public void Init(ItemData itemData)
    {
        ItemData = itemData;
        GetComponent<Image>().sprite = itemData.Icon;
        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2 (itemData.SizeWidth * ItemGrid.baseTileSizeWidth,
                                               itemData.SizeHeight * ItemGrid.baseTileSizeHeight);
        gridPosition = Vector2Int.zero;
        rotationCount = 0;
    }
}
