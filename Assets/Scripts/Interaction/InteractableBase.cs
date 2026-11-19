
using UnityEngine;

public abstract class InteractableBase : MonoBehaviour, Interactable
{
    [Header("Interaction Prompt")]
    [SerializeField] protected Sprite promptSprite;

    [SerializeField]
    protected Vector3 promptOffset =
        new Vector3(0.5f, 0.5f, 0f);

    [SerializeField] protected float promptScale = 1f;

    [Header("Detection Settings")]
    [SerializeField] protected float interactionRadius = 1f;

    [SerializeField] protected int promptSortingOrder = 15;

    protected GameObject promptObject;

    protected virtual void Awake()
    {
        CreatePrompt();
    }

    private void CreatePrompt()
    {
        promptObject = new GameObject("InteractionPrompt");

        promptObject.transform.SetParent(transform, false);

        promptObject.transform.localPosition = promptOffset;
        promptObject.transform.localRotation = Quaternion.identity;
        promptObject.transform.localScale =
            Vector3.one * promptScale;

        SpriteRenderer renderer =
            promptObject.AddComponent<SpriteRenderer>();

        renderer.sprite = promptSprite;
        renderer.sortingOrder = promptSortingOrder;
        renderer.sortingLayerID = 1;

        CircleCollider2D trigger =
            promptObject.AddComponent<CircleCollider2D>();

        trigger.isTrigger = true;
        trigger.radius = interactionRadius;

        // Script phát hiện được gắn trực tiếp lên Prompt
        promptObject.AddComponent<PromptDetector>();
    }

    public abstract void Interact();
}
