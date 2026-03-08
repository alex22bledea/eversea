
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemGridInteract))]
public class ItemGrid : MonoBehaviour
{
    public enum TooltipPlacement
    {
        Left,
        Right,
    }

    public const float baseTileSizeWidth = 64;
    public const float baseTileSizeHeight = 64;

    private InventoryItem[,] inventoryItemSlotMatrix;
    private List<InventoryItem> inventoryItemList;

    private RectTransform rectTransform;

    [SerializeField] private int gridSizeWidth;
    [SerializeField] private int gridSizeHeight;

    Vector2 positionOnTheGrid = new Vector2();
    Vector2Int tileGridPosition = new Vector2Int();

    [SerializeField] private Canvas canvas;

    [field: SerializeField] public TooltipPlacement tooltipPlacement { get; private set; }
    [field: SerializeField] public RectTransform tooltipPlacementBorder { get; private set; }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(gridSizeWidth, gridSizeHeight);
    }

    private void Init(int width, int height)
    {
        inventoryItemSlotMatrix = new InventoryItem[width, height];
        inventoryItemList = new List<InventoryItem>();

        Vector2 size = new Vector2(width * baseTileSizeWidth, height * baseTileSizeHeight);
        rectTransform.sizeDelta = size;
    }

    public bool HasItemAtGridPosition(Vector2Int gridPosition) => inventoryItemSlotMatrix[gridPosition.x, gridPosition.y] != null;

    public InventoryItem PickupItem(Vector2Int gridPosition)
    {
        InventoryItem item = inventoryItemSlotMatrix[gridPosition.x, gridPosition.y];

        gridPosition = item.GetCurrentGridPosition(); /// We move the targeted grid position to the representative grid position of the item

        for (int w = 0; w < item.GetCurrentWidth(); w++)
            for (int h = 0; h < item.GetCurrentHeight(); h++)
                if(item.IsOccupied(w, h))
                    inventoryItemSlotMatrix[gridPosition.x + w, gridPosition.y + h] = null;

        item.SetCurrentGridPosition(Vector2Int.zero);

        inventoryItemList.Remove(item);

        return item;
    }

    public void PlaceItem(InventoryItem item, Vector2Int gridPosition)
    {
        item.SetCurrentGridPosition(gridPosition);
        for(int w = 0; w < item.GetCurrentWidth(); w++)
            for(int h = 0; h < item.GetCurrentHeight(); h++)
                if(item.IsOccupied(w, h))
                    inventoryItemSlotMatrix[gridPosition.x + w, gridPosition.y + h] = item;

        RectTransform itemRectTransform = item.GetComponent<RectTransform>();
        itemRectTransform.SetParent(rectTransform);

        itemRectTransform.localPosition = new Vector2(gridPosition.x * baseTileSizeWidth + item.GetCurrentWidth() * baseTileSizeWidth / 2,
                                             gridPosition.y * baseTileSizeHeight + item.GetCurrentHeight() * baseTileSizeHeight / 2);

        inventoryItemList.Add(item);
    }

    public bool TryToPlaceItemAnywhere(InventoryItem item)
    {
        for (int rotation = 0; rotation < 4; rotation++)
        {
            item.SetRotation(rotation);

            int width = item.GetCurrentWidth();
            int height = item.GetCurrentHeight();

            for (int w = 0; w <= gridSizeWidth - width; w++)
                for (int h = 0; h <= gridSizeHeight - height; h++)
                    if (CanPlaceItemAtGridPosition(item, new Vector2Int(w, h)))
                    {
                        PlaceItem(item, new Vector2Int(w, h));
                        return true;
                    }
        }

        return false;
    }

    public InventoryItem GetItemReferenceAtGridPosition(Vector2Int gridPosition) => inventoryItemSlotMatrix[gridPosition.x, gridPosition.y];

    public bool CanPlaceItemAtGridPosition(InventoryItem item, Vector2Int gridPosition)
    {
        int width = item.GetCurrentWidth();
        int height = item.GetCurrentHeight();

        if (gridPosition.x < 0 || gridPosition.x + width - 1 >= gridSizeWidth
            || gridPosition.y < 0 || gridPosition.y + height - 1 >= gridSizeHeight)
            return false;

        for (int w = 0; w < width; w++)
            for (int h = 0; h < height; h++)
                if (item.IsOccupied(w, h) && inventoryItemSlotMatrix[gridPosition.x + w, gridPosition.y + h] != null)
                    return false;

        return true;
    }

    public Vector2Int GetGridPositionOfCanvasPosition(Vector2 mousePosition)
    {
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = mousePosition.y - rectTransform.position.y;

        tileGridPosition.x = (int)(positionOnTheGrid.x / GetCanvasTileSizeWidth());
        tileGridPosition.y = (int)(positionOnTheGrid.y / GetCanvasTileSizeHeight());

        return tileGridPosition;
    }

    public bool HasInventoryItemListEmpty() => inventoryItemList.Count == 0;

    public void EmptyInventory(ItemGrid otherItemGrid)
    {
        Vector2Int gridPosition;
        foreach (InventoryItem item in inventoryItemList)
        {
            gridPosition = item.GetCurrentGridPosition();

            for (int w = 0; w < item.GetCurrentWidth(); w++)
                for (int h = 0; h < item.GetCurrentHeight(); h++)
                    if (item.IsOccupied(w, h))
                        inventoryItemSlotMatrix[gridPosition.x + w, gridPosition.y + h] = null;

            item.SetCurrentGridPosition(Vector2Int.zero);

            if ( !otherItemGrid.TryToPlaceItemAnywhere(item))
                Destroy(item.gameObject);
        }

        inventoryItemList.Clear();
    }

    public float GetCanvasTileSizeWidth()
    {
        return baseTileSizeWidth * canvas.scaleFactor;
    }

    public float GetCanvasTileSizeHeight()
    {
        return baseTileSizeHeight * canvas.scaleFactor;
    }
}
