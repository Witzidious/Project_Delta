using UnityEngine;

public class ItemPickup : InteractableBase
{
    [SerializeField] private InventoryItem item;

    public override void Interact()
    {
        if (item == null)
        {
            Debug.LogError("Item is missing!");
            return;
        }

        bool added =
            InventoryManager.Instance.AddItem(item);

        if (added)
        {
            Debug.Log("Picked up: " + item.itemName);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Inventory is full!");
        }
    }
}