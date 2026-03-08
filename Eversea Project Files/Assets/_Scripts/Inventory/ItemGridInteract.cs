using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ItemGrid))]
public class ItemGridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private bool hideWhenUntargeted = false;

    private ItemGrid itemGrid;
    private Image image;
    private Color imageColor;

    void Awake()
    {
        itemGrid = GetComponent<ItemGrid>();
        if (hideWhenUntargeted)
        {
            image = GetComponent<Image>();
            imageColor = image.color;
            image.color = new Color(imageColor.r, imageColor.g, imageColor.b, 0);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InventoryManager.Instance.SetSelectedItemGrid(itemGrid);

        if (hideWhenUntargeted)
        {
            image.color = imageColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.UnselectItemGrid(itemGrid);

        if (hideWhenUntargeted && itemGrid.HasInventoryItemListEmpty())
        {
            image.color = new Color(imageColor.r, imageColor.g, imageColor.b, 0);
        }
    }
}
