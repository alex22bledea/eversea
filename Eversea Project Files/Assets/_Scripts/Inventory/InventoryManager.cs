using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : Singleton<InventoryManager>
{
    public event Action OnAnyItemSold;

    private ItemGrid selectedItemGrid;

    private InventoryItem selectedItem; /// item held in "hand"
    private InventoryItem hoveredItem;
    private RectTransform selectedItemRectTransform;

    [SerializeField] private InputReaderSO input;

    [Space(12)]
    [SerializeField] private RectTransform canvasRectTransform;

    [Space(12), Header("Inventories")]
    [SerializeField] private ItemGrid inventoryItemGrid;
    [SerializeField] private ItemGrid temporaryInventoryItemGrid;

    [Space(12), Header("Highlighter")]
    [SerializeField] private InventoryHighlight highlight;
    [SerializeField] private RectTransform inventoryHighlightPrefab;

    [Space(12), Header("Item Tooltip")]
    [SerializeField] private ItemTooltip tooltip;
    [SerializeField] private RectTransform itemTooltipPrefab;

    private bool canSellItems;
    private MarketPricesSO marketPrices;

    #region testing

    [Space(12), Header("Testing")]
    [SerializeField] private List<ItemData> itemDataList;
    [SerializeField] private RectTransform itemPrefab;

    private void GenerateRandomItem()
    {
        ItemData itemData = itemDataList.GetRandomElement();

        RectTransform itemRectTransform = Instantiate<RectTransform>(itemPrefab, canvasRectTransform);
        InventoryItem item = itemRectTransform.GetComponent<InventoryItem>();
        item.Init(itemData);

        SetSelectedInventoryItem(item);
    }

    #endregion

    private void OnEnable()
    {
        input.RotateItemEvent += TryRoateItem;
        input.DiscardItemEvent += TryDiscardItem;
        input.PlaceOrPickupItemEvent += TryInteractWithItemGrid;
        input.SellItemEvent += TrySellItem;
    }

    private void OnDisable()
    {
        input.RotateItemEvent -= TryRoateItem;
        input.DiscardItemEvent -= TryDiscardItem;
        input.PlaceOrPickupItemEvent -= TryInteractWithItemGrid;
        input.SellItemEvent -= TrySellItem;
    }

    private void Start()
    {
        if (highlight == null)
        {
            RectTransform inventoryHighlightRectTransform = Instantiate<RectTransform>(inventoryHighlightPrefab, canvasRectTransform);
            highlight = inventoryHighlightRectTransform.GetComponent<InventoryHighlight>();
        }
        highlight.Hide();

        if (tooltip == null)
        {
            RectTransform itemTooltipRectTransform = Instantiate<RectTransform>(itemTooltipPrefab, canvasRectTransform);
            tooltip = itemTooltipRectTransform.GetComponent<ItemTooltip>();
        }
        tooltip.Hide();
    }

    private void Update()
    {
        if (selectedItemGrid == null)
            hoveredItem = null;
        else
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector2Int selectedGridPosition = selectedItemGrid.GetGridPositionOfCanvasPosition(mousePosition);

            if (selectedItemGrid.HasItemAtGridPosition(selectedGridPosition))
                hoveredItem = selectedItemGrid.GetItemReferenceAtGridPosition(selectedGridPosition);
            else
                hoveredItem = null;
        }

        HandleItemIconDrag();
        HandleItemHighlight();
        HandleItemToolTip();

        if (selectedItemGrid != null)
        {
            /// FOR TESTING:
            if (Input.GetKeyDown(KeyCode.Q))
                GenerateRandomItem();
            /*

            if (Input.GetKeyDown(KeyCode.T))
            {
                GenerateRandomItem();
                if (selectedItemGrid.TryToPlaceItemAnywhere(selectedItem))
                    selectedItem = null;
                else
                    Debug.Log("Could not place item");
            }
            */
        }
    }

    private void TryRoateItem()
    {
        if(selectedItem != null)
            selectedItem.RotateOnce();
    }

    private void TryDiscardItem()
    {
        if (selectedItem != null)
        {
            Destroy(selectedItem.gameObject);
            SetSelectedInventoryItem(null);
        }
        else if (hoveredItem != null && selectedItemGrid != null)
        {
            selectedItemGrid.PickupItem(hoveredItem.GetCurrentGridPosition()); /// We must make space in the item grid 
            Destroy(hoveredItem.gameObject);
            hoveredItem = null;
        }
    }

    private void HandleItemIconDrag()
    {
        if (selectedItem != null)
        {
            selectedItemRectTransform.position = Mouse.current.position.ReadValue();
        }
    }

    private void HandleItemHighlight()
    {
        if (selectedItemGrid == null)
        {
            highlight.Hide();
            return;
        }

        if (selectedItem != null)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            AddItemPlacementOffset(ref mousePosition);

            Vector2Int selectedGridPosition = selectedItemGrid.GetGridPositionOfCanvasPosition(mousePosition);

            highlight.SetSize(selectedItem);
            highlight.SetPosition(selectedItemGrid, selectedItem, selectedGridPosition);

            if (selectedItemGrid.CanPlaceItemAtGridPosition(selectedItem, selectedGridPosition))
                highlight.SetState(InventoryHighlight.HighlighterState.Placeable);
            else
                highlight.SetState(InventoryHighlight.HighlighterState.Unplaceable);

            highlight.Show();
        }
        else if (hoveredItem != null)
        {
            highlight.SetSize(hoveredItem);
            highlight.SetPosition(selectedItemGrid, hoveredItem);
            highlight.Show();
        }
        else
            highlight.Hide();
    }

    public void SetCanSellItems(MarketPricesSO marketPrices)
    {
        canSellItems = true;
        this.marketPrices = marketPrices;
        tooltip.SetMarketPrices(marketPrices);
    }

    public void SetCannotSellItems()
    {
        canSellItems = false;
        marketPrices = null;
        tooltip.SetMarketPrices(null);
    }

    public void ProperlyCloseInventory()
    {
        /// Handle selected item

        if (selectedItem != null)
        {
            if (!inventoryItemGrid.TryToPlaceItemAnywhere(selectedItem))
                Destroy(selectedItem.gameObject);
            SetSelectedInventoryItem(null);
        }

        /// Handle selected item grid

        if (selectedItemGrid != null)
            UnselectItemGrid(selectedItemGrid);

        /// Handle remaining items in temporary inventory

        ProperlyCloseTemporaryInventory();
    }

    public void ProperlyCloseTemporaryInventory()
    {
        if (temporaryInventoryItemGrid.isActiveAndEnabled && !temporaryInventoryItemGrid.HasInventoryItemListEmpty())
            temporaryInventoryItemGrid.EmptyInventory(inventoryItemGrid);
    }


    public void GiveItemInHand(ItemData itemData)
    {
        if (selectedItem != null)
        {
            if (!inventoryItemGrid.TryToPlaceItemAnywhere(selectedItem))
                Destroy(selectedItem.gameObject);
            SetSelectedInventoryItem(null);
        }

        RectTransform itemRectTransform = Instantiate<RectTransform>(itemPrefab, canvasRectTransform);
        InventoryItem item = itemRectTransform.GetComponent<InventoryItem>();
        item.Init(itemData);

        SetSelectedInventoryItem(item);
    }

    private void HandleItemToolTip()
    {
        if (selectedItemGrid == null)
        {
            tooltip.Hide(); 
            return;
        }

        if(selectedItem != null)
        {
            if (canSellItems)
                tooltip.SetInfo(selectedItem, ItemTooltip.TooltipType.Sell, ItemTooltip.TooltipInteractType.Place);
            else
                tooltip.SetInfo(selectedItem, ItemTooltip.TooltipType.Simple, ItemTooltip.TooltipInteractType.Place);

            tooltip.SetPosition(selectedItemGrid);
            tooltip.Show();
        }
        else if (hoveredItem != null)
        {
            if (canSellItems)
                tooltip.SetInfo(hoveredItem, ItemTooltip.TooltipType.Sell, ItemTooltip.TooltipInteractType.Pickup);
            else
                tooltip.SetInfo(hoveredItem, ItemTooltip.TooltipType.Simple, ItemTooltip.TooltipInteractType.Pickup);

            tooltip.SetPosition(selectedItemGrid);
            tooltip.Show();
        }
        else
            tooltip.Hide();
    }

    private void TrySellItem()
    {
        if ( !canSellItems)
            return;

        if (selectedItem != null)
        {
            if (marketPrices.IsInList(selectedItem.ItemData))
            {
                UpgradeSystem.Instance.AddMoney(marketPrices.Price(selectedItem.ItemData));
                Destroy(selectedItem.gameObject);
                SetSelectedInventoryItem(null);

                OnAnyItemSold?.Invoke();
            }
        }
        else if(hoveredItem != null && selectedItemGrid != null)
        {
            if (marketPrices.IsInList(hoveredItem.ItemData))
            {
                selectedItemGrid.PickupItem(hoveredItem.GetCurrentGridPosition()); /// We must make space in the item grid

                UpgradeSystem.Instance.AddMoney(marketPrices.Price(hoveredItem.ItemData));
                Destroy(hoveredItem.gameObject);
                hoveredItem = null;

                OnAnyItemSold?.Invoke();
            }
        }
    }

    private void TryInteractWithItemGrid()
    {
        if (selectedItemGrid == null)
            return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        if (selectedItem == null)
        {
            Vector2Int selectedGridPosition = selectedItemGrid.GetGridPositionOfCanvasPosition(mousePosition);

            if (FishingMinigamesManager.Instance.IsPlayingMinigame) /// Cannot pick up while playing fishing minigames
                return;

            if(selectedItemGrid.HasItemAtGridPosition(selectedGridPosition))
                SetSelectedInventoryItem(selectedItemGrid.PickupItem(selectedGridPosition));
        }
        else
        {
            AddItemPlacementOffset(ref mousePosition);

            Vector2Int selectedGridPosition = selectedItemGrid.GetGridPositionOfCanvasPosition(mousePosition);

            if(selectedItemGrid.CanPlaceItemAtGridPosition(selectedItem, selectedGridPosition))
            {
                selectedItemGrid.PlaceItem(selectedItem, selectedGridPosition);
                SetSelectedInventoryItem(null);
            }
        }
    }

    private void AddItemPlacementOffset(ref Vector2 position)
    {
        position.x -= (selectedItem.GetCurrentWidth() - 1) * selectedItemGrid.GetCanvasTileSizeWidth() / 2;
        position.y -= (selectedItem.GetCurrentHeight() - 1) * selectedItemGrid.GetCanvasTileSizeHeight() / 2;
    }

    public void SetSelectedItemGrid(ItemGrid itemGrid)
    {
        selectedItemGrid = itemGrid;
        highlight.SetParent(itemGrid.GetComponent<RectTransform>());
    }

    public void UnselectItemGrid(ItemGrid itemGrid)
    {
        if (selectedItemGrid == itemGrid)
        {
            selectedItemGrid = null;
            highlight.SetParent(canvasRectTransform);
        }
    }

    private void SetSelectedInventoryItem(InventoryItem item)
    {
        if (item == null)
        {
            selectedItem = null;
            selectedItemRectTransform = null;
        }
        else
        {
            if(selectedItem != null)
                Destroy(selectedItem.gameObject);

            selectedItem = item;
            selectedItemRectTransform = item.GetComponent<RectTransform>();
            selectedItemRectTransform.SetParent(canvasRectTransform); /// So it it drawn over the other items in the grid!
            selectedItemRectTransform.SetAsLastSibling();
        }
    }

    public bool HasItemInHand() => selectedItem != null;
}
