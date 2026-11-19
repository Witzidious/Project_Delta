using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Inventory Settings")]
    [SerializeField, Min(1)]
    private int capacity = 6;
    public int Capacity => capacity;
    private List<InventoryItem> items = new();
    public IReadOnlyList<InventoryItem> Items => items;
    public System.Action OnInventoryChanged;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        for (int i = 0; i < capacity; i++)
        {
            items.Add(null);
        }
        ResizeInventory(capacity);
    }

    public bool AddItem(InventoryItem item)
    {
        if (item == null || HasItem(item.itemId))
            return false;

        int emptySlot = items.IndexOf(null);

        if (emptySlot == -1)
        {
            Debug.Log("Inventory is full!");
            return false;
        }

        items[emptySlot] = item;

        OnInventoryChanged?.Invoke();

        Debug.Log("Picked up: " + item.itemName);

        return true;
    }
    public bool ResizeInventory(int newCapacity)
    {
        newCapacity = Mathf.Max(1, newCapacity);

        if (newCapacity < items.Count)
        {
            // Kiểm tra các slot sắp bị cắt có đang chứa item không
            for (int i = newCapacity; i < items.Count; i++)
            {
                if (items[i] != null)
                {
                    Debug.LogWarning(
                        "Cannot reduce inventory capacity: " +
                        "some slots still contain items!"
                    );

                    return false;
                }
            }

            items.RemoveRange(
                newCapacity,
                items.Count - newCapacity
            );
        }
        else
        {
            // Thêm các slot trống
            while (items.Count < newCapacity)
            {
                items.Add(null);
            }
        }

        capacity = newCapacity;

        OnInventoryChanged?.Invoke();

        return true;
    }

    public bool HasItem(string itemId)
    {
        return items.Exists(item =>
            item != null && item.itemId == itemId);
    }

    public bool RemoveItem(string itemId)
    {
        int index = items.FindIndex(item =>
            item != null && item.itemId == itemId);

        if (index == -1)
            return false;

        items[index] = null;

        OnInventoryChanged?.Invoke();

        return true;
    }
    public bool UseItem(string itemId)
    {
        int index = items.FindIndex(item =>
            item != null && item.itemId == itemId);

        if (index == -1)
            return false;

        return true;
    }
    public void SwapSlots(int indexA, int indexB)
    {
        if (indexA < 0 || indexA >= items.Count ||
            indexB < 0 || indexB >= items.Count)
            return;

        InventoryItem temp = items[indexA];
        items[indexA] = items[indexB];
        items[indexB] = temp;

        OnInventoryChanged?.Invoke();
    }
    public bool DropItemFromSlot(int slotIndex)
    {
        // Kiểm tra slot hợp lệ
        if (slotIndex < 0 || slotIndex >= items.Count)
            return false;

        InventoryItem item = items[slotIndex];

        if (item == null)
            return false;

        // Kiểm tra prefab dùng để thả
        if (item.worldPrefab == null)
        {
            Debug.LogError(
                "World Prefab is missing for item: " + item.itemName
            );
            return false;
        }

        // Tìm Player
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Cannot drop item: Player not found!");
            return false;
        }

        // Vị trí thả item
        Vector3 dropPosition =
            player.transform.position + new Vector3(0.75f, 0f, 0f);

        // Tạo item trong thế giới
        GameObject droppedItem = Instantiate(
            item.worldPrefab,
            dropPosition,
            Quaternion.identity
        );

        if (droppedItem == null)
            return false;

        // Chỉ xóa item khỏi Inventory sau khi tạo prefab thành công
        items[slotIndex] = null;

        OnInventoryChanged?.Invoke();

        Debug.Log("Dropped: " + item.itemName);

        return true;
    }
}