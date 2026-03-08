
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    public enum TooltipType
    {
        None,
        Simple,
        Sell
    }

    public enum TooltipInteractType
    {
        None,
        Place,
        Pickup
    }

    [SerializeField] private RectTransform tooltipRectTransform;

    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI placeOrPickupText;
    [SerializeField] private RectTransform backgroundRectTransform;
    [SerializeField] private RectTransform sellPriceRectTransform;
    [SerializeField] private TextMeshProUGUI sellPrice;

    private MarketPricesSO marketPrices;

    private TooltipType currentType;
    private TooltipInteractType currentInteractType;
    private InventoryItem selectedItem;

    public void SetMarketPrices(MarketPricesSO marketPrices)
    {
        this.marketPrices = marketPrices;
    }

    public void SetInfo(InventoryItem item, TooltipType type, TooltipInteractType interactType)
    {
        if (item == selectedItem && type == currentType && interactType == currentInteractType)
            return;

        selectedItem = item;
        currentType = type;
        currentInteractType = interactType;

        itemName.text = item.ItemData.Name;

        if (interactType == TooltipInteractType.Place)
            placeOrPickupText.text = "Place";
        else if(interactType == TooltipInteractType.Pickup)
            placeOrPickupText.text = "Pick up";

        if (type == TooltipType.Simple)
        {
            backgroundRectTransform.localPosition = new Vector3(0, 35, 0);
            backgroundRectTransform.sizeDelta = new Vector2(300, 230);
            sellPriceRectTransform.gameObject.SetActive(false);
        }
        else if (type == TooltipType.Sell)
        {
            backgroundRectTransform.localPosition = new Vector3(0, 0, 0);
            backgroundRectTransform.sizeDelta = new Vector2(300, 300);
            sellPriceRectTransform.gameObject.SetActive(true);

            if (marketPrices.IsInList(item.ItemData))
                sellPrice.text = "Sell\n" + "[" + marketPrices.Price(item.ItemData).ToString() + "]";
            else
                sellPrice.text = "Can't sell\nthis here";
        }
    }

    public void SetPosition(ItemGrid itemGrid)
    {
        switch (itemGrid.tooltipPlacement)
        {
            case ItemGrid.TooltipPlacement.Right:
                tooltipRectTransform.position = new Vector2(itemGrid.tooltipPlacementBorder.position.x, selectedItem.transform.position.y);
                break;
            case ItemGrid.TooltipPlacement.Left:
                tooltipRectTransform.position = new Vector2(itemGrid.tooltipPlacementBorder.position.x, selectedItem.transform.position.y);
                break;
        }
    }

    public void Hide() => tooltipRectTransform.gameObject.SetActive(false);

    public void Show() => tooltipRectTransform.gameObject.SetActive(true);
}
