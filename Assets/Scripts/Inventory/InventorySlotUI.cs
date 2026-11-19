using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class InventorySlotUI : MonoBehaviour,
    IPointerClickHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler,
    IDropHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Image backgroundImage;
    private RectTransform inventoryPanel;

    private InventoryItem item;
    private int slotIndex;
    private Action<int> onSelected;

    private Canvas canvas;
    private GameObject dragIcon;

    public void Setup(
        int index,
        Canvas rootCanvas,
         RectTransform panel,
        Action<int> selectCallback)
    {
        slotIndex = index;
        canvas = rootCanvas;
        inventoryPanel = panel;
        onSelected = selectCallback;
    }

    public void SetItem(InventoryItem newItem)
    {
        item = newItem;

        if (iconImage == null)
            return;

        bool hasIcon = item != null && item.icon != null;

        iconImage.sprite = hasIcon ? item.icon : null;
        iconImage.enabled = hasIcon;
        iconImage.preserveAspect = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onSelected?.Invoke(slotIndex);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null || item.icon == null)
            return;

        iconImage.color = Color.grey;

        dragIcon = new GameObject(
            "DragIcon",
            typeof(RectTransform),
            typeof(CanvasGroup),
            typeof(Image)
        );

        dragIcon.transform.SetParent(canvas.transform, false);

        Image image = dragIcon.GetComponent<Image>();
        image.sprite = item.icon;
        image.preserveAspect = true;
        image.raycastTarget = false;

        RectTransform rect = dragIcon.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(64, 64);
        rect.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragIcon != null)
        {
            dragIcon.transform.position = eventData.position;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventorySlotUI source =
            eventData.pointerDrag != null
            ? eventData.pointerDrag.GetComponent<InventorySlotUI>()
            : null;

        if (source == null || source == this)
            return;

        InventoryManager.Instance.SwapSlots(
            source.slotIndex,
            slotIndex
        );
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Kiểm tra con trỏ có nằm ngoài Inventory không
        bool isOutsideInventory = inventoryPanel != null &&
            !RectTransformUtility.RectangleContainsScreenPoint(
                inventoryPanel,
                eventData.position,
                eventData.pressEventCamera
            );

        // Nếu thả bên ngoài Inventory thì làm rơi item
        if (isOutsideInventory && item != null)
        {
            InventoryManager.Instance.DropItemFromSlot(slotIndex);
        }

        // Khôi phục màu icon
        if (iconImage != null) iconImage.color = Color.white;

        if (dragIcon != null)
        {
            Destroy(dragIcon);
        }
    }
}