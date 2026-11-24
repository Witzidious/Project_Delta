
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(SceneStateID))]
public class LockedDoor : InteractableBase
{
    [Header("Door Settings")]
    [SerializeField] private string requiredItemId = "rusty_key";

    private SceneStateID stateId;
    private BoxCollider2D doorCollider;
    private SpriteRenderer doorRenderer;

    private bool isOpen = false;

    protected override void Awake()
    {
        // Quan trọng: gọi Awake của InteractableBase
        // để tạo InteractionPrompt.
        base.Awake();

        stateId = GetComponent<SceneStateID>();
        doorCollider = GetComponent<BoxCollider2D>();
        doorRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        isOpen = stateId.GetState("Opened");
        UpdateDoorVisual();
    }

    public override void Interact()
    {
        if (isOpen)
        {
            Debug.Log("Closing door!");

            isOpen = false;
            stateId.SetState("Opened", false);
            UpdateDoorVisual();
        }
        else
        {
            if (InventoryManager.Instance == null)
            {
                Debug.LogError("InventoryManager is missing!");
                return;
            }

            if (InventoryManager.Instance.HasItem(requiredItemId))
            {
                Debug.Log("Door unlocked!");

                isOpen = true;
                stateId.SetState("Opened", true);

                InventoryManager.Instance.UseItem(requiredItemId);

                UpdateDoorVisual();
            }
            else
            {
                Debug.Log("The door is locked. I need a key.");
            }
        }
    }

    private void UpdateDoorVisual()
    {
        doorCollider.isTrigger = isOpen;
        doorRenderer.color = isOpen ? Color.grey : Color.white;
    }
}
