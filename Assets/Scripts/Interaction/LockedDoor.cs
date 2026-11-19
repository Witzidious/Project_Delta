using UnityEngine;

public class LockedDoor : InteractableBase
{
    [SerializeField] private string requiredItemId = "rusty_key";
    private bool open = false;
    void Start()
    {
        
    }
    void Update()
    {
        
    }

    public override void Interact()
    {
        if (open == true)
        {
            Debug.Log("Door locked!");
            open = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (InventoryManager.Instance.HasItem(requiredItemId))
        {
            Debug.Log("Door unlocked!");
            open = true;

            InventoryManager.Instance.UseItem(requiredItemId);

            //InventoryManager.Instance.RemoveItem(requiredItemId);
            //Destroy(gameObject);

            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().color = Color.grey;
        }
        else
        {
            Debug.Log("The door is locked. I need a key.");
        }
    }
}