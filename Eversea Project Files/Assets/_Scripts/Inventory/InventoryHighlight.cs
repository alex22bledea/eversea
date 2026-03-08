using UnityEngine;
using UnityEngine.UI;

public class InventoryHighlight : MonoBehaviour
{
    public enum HighlighterState
    {
        None,
        Placeable,
        Unplaceable
    }

    private HighlighterState state;

    [SerializeField] private RectTransform highlighterRectTransform;
    [SerializeField] private GridLayoutGroup gridLayoutGroup;

    [Space(12)]
    [SerializeField] private RectTransform HighlighterCellPrefab;

    private int storedRotation;
    private ItemData storedItemData;
    private InventoryItem storedItem;

    private HighlighterCell[] highlighterCellsArray;
    private void Awake()
    {
        storedRotation = -1;
        storedItemData = null;
        storedItem = null;

        highlighterCellsArray = new HighlighterCell[0];
        state = HighlighterState.None;
    }

    public void SetSize(InventoryItem item)
    {
        if (storedItem != null && storedRotation == item.GetRotationCount() && storedItemData == item.ItemData)
            return;

        storedRotation = item.GetRotationCount();
        storedItemData = item.ItemData;
        storedItem = item;

        int oldLength = highlighterCellsArray.Length;
        int newLength = storedItemData.SizeWidth * storedItemData.SizeHeight;

        if (oldLength > newLength)
        {
            for (int i = newLength; i < oldLength; i++)
                Destroy(highlighterCellsArray[i].gameObject);

            HighlighterCell[] highlighterCellsArrayRef = highlighterCellsArray;
            highlighterCellsArray = new HighlighterCell[newLength];

            for (int i = 0; i < newLength; i++)
                highlighterCellsArray[i] = highlighterCellsArrayRef[i];
        }
        else if (oldLength < newLength)
        {
            HighlighterCell[] highlighterCellsArrayRef = highlighterCellsArray;
            highlighterCellsArray = new HighlighterCell[newLength];

            for(int i = 0; i < oldLength; i++)
                highlighterCellsArray[i] = highlighterCellsArrayRef[i];
            for (int i = oldLength; i < newLength; i++)
            {
                RectTransform childRectTransform = Instantiate(HighlighterCellPrefab, highlighterRectTransform);
                childRectTransform.SetAsLastSibling();
                highlighterCellsArray[i] = childRectTransform.GetComponent<HighlighterCell>();
            }
        }

        gridLayoutGroup.constraintCount = item.GetCurrentHeight();

        state = HighlighterState.None;
        SetState(HighlighterState.Placeable);
    }

    public void SetState(HighlighterState state)
    {
        if (this.state == state)
            return;

        this.state = state;

        int width, height;

        switch (state)
        {
            case HighlighterState.Placeable:
                width = storedItem.GetCurrentWidth();
                height = storedItem.GetCurrentHeight();

                for (int w = 0; w < width; w++)
                    for (int h = 0; h < height; h++)
                    {
                        if (storedItem.IsOccupied(w, h))
                            highlighterCellsArray[w * height + h].SetColor(HighlighterCell.HighlighterCellState.White);
                        else
                            highlighterCellsArray[w * height + h].SetColor(HighlighterCell.HighlighterCellState.FullyTransparent);
                    }
                break;

            case HighlighterState.Unplaceable:
                width = storedItem.GetCurrentWidth();
                height = storedItem.GetCurrentHeight();

                for (int w = 0; w < width; w++)
                    for (int h = 0; h < height; h++)
                    {
                        if (storedItem.IsOccupied(w, h))
                            highlighterCellsArray[w * height + h].SetColor(HighlighterCell.HighlighterCellState.Red);
                        else
                            highlighterCellsArray[w * height + h].SetColor(HighlighterCell.HighlighterCellState.FullyTransparent);
                    }
                break;
        }
    }

    public void SetPosition(ItemGrid itemGrid, InventoryItem item)
    {
        highlighterRectTransform.localPosition = new Vector2(item.GetCurrentGridPosition().x * ItemGrid.baseTileSizeWidth,
                                                             item.GetCurrentGridPosition().y * ItemGrid.baseTileSizeHeight);                   
    }

    public void SetPosition(ItemGrid itemGrid, InventoryItem item, Vector2Int gridPosition)
    {
        highlighterRectTransform.localPosition = new Vector2(gridPosition.x * ItemGrid.baseTileSizeWidth,
                                                             gridPosition.y * ItemGrid.baseTileSizeHeight);
    }

    public void SetParent(RectTransform rectTransform)
    {
        highlighterRectTransform.SetParent(rectTransform);
        highlighterRectTransform.SetSiblingIndex(0);
    }

    public void Show()
    {
        highlighterRectTransform.gameObject.SetActive(true);
    }

    public void Hide()
    {
        highlighterRectTransform.gameObject.SetActive(false);
    }
}
