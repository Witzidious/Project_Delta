using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Transform slotContainer;
    [SerializeField] private InventorySlotUI slotPrefab;
    [SerializeField] private Canvas rootCanvas;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text nameText;

    [Header("Input")]
    [SerializeField] private InputActionReference inventoryAction;

    private List<InventorySlotUI> slots = new();
    private int selectedIndex = -1;

    private void OnEnable()
    {
        inventoryAction?.action.Enable();
    }

    private void Start()
    {
        InventoryManager.Instance.OnInventoryChanged += RefreshInventory;

        CreateSlots();
        RefreshInventory();
    }

    private void OnDisable()
    {
        inventoryAction?.action.Disable();

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged -= RefreshInventory;
        }
    }

    private void Update()
    {
        if (inventoryAction != null &&
            inventoryAction.action.WasPressedThisFrame())
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }

    private void CreateSlots()
    {
        Debug.Log("Inv slot: " + InventoryManager.Instance.Items.Count);
        for (int i = 0; i < InventoryManager.Instance.Items.Count; i++)
        {
            InventorySlotUI slot = Instantiate(
                slotPrefab,
                slotContainer
            );

            slot.Setup(i, rootCanvas, inventoryPanel.GetComponent<RectTransform>(), SelectSlot);

            slots.Add(slot);
        }
    }

    private void RefreshInventory()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].SetItem(
                InventoryManager.Instance.Items[i]
            );
        }

        if (selectedIndex >= 0 &&
            selectedIndex < slots.Count)
        {
            UpdateDescription(selectedIndex);
        }

        if (selectedIndex == -1)
        {
            nameText.text = "Select an item";
        }
    }

    private void SelectSlot(int index)
    {
        selectedIndex = index;
        UpdateDescription(index);
    }

    private void UpdateDescription(int index)
    {
        if (descriptionText == null)
        {
            Debug.LogError("DescriptionText is not assigned!");
            return;
        }

        if (index < 0 ||
            index >= InventoryManager.Instance.Items.Count)
            return;

        InventoryItem item = InventoryManager.Instance.Items[index];

        if (item == null)
        {
            nameText.text = "Empty slot";
            descriptionText.text = string.Empty;
            return;
        }

        nameText.text = item.itemName;
        descriptionText.text = item.description;
    }
}