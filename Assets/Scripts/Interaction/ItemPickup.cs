using UnityEngine;

public class ItemPickup : InteractableBase
{
    [SerializeField] private InventoryItem item;

    private SceneStateID stateId;
    protected override void Awake()
    {
        base.Awake();
        stateId = GetComponent<SceneStateID>();
    }

    private void Start()
    {

        if (stateId != null && stateId.GetState("Collected"))
        {
            Destroy(gameObject);
        }
    }
    public override void Interact()
    {
        if (item == null)
        {
            Debug.LogError("Item is missing!");
            return;
        }

        bool added = InventoryManager.Instance.AddItem(item);

        if (added)
        {
            Debug.Log("Picked up: " + item.itemName);
            if (stateId != null) stateId.SetState("Collected", true);
            PersistentEntity entity = GetComponent<PersistentEntity>();

            if (entity != null && EntityManager.Instance != null)
            {
                EntityManager.Instance.RemoveEntity(entity);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.Log("Inventory is full!");
        }
    }
    public void DropItem()
    {
        if (stateId != null) stateId.SetState("Dropped", true);
    }
}