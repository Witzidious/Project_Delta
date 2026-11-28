using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference interactAction;

    [Header("Interaction")]
    [SerializeField] private float interactionDistance = 0.8f;
    [SerializeField] private LayerMask interactableLayer;

    private Vector2 facingDirection = Vector2.down;
    private Animator animator;
    private bool isInteracting;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    public void SetAnimator(Animator newAnimator)
    {
        animator = newAnimator;

        if (animator == null)
        {
            Debug.LogError("PlayerMovement: Animator is NULL!");
            return;
        }
        Debug.Log("PlayerInteraction received Animator: " + animator.name);

    }
    private void OnEnable()
    {
        interactAction?.action.Enable();
    }

    private void OnDisable()
    {
        interactAction?.action.Disable();
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        Vector2 movement = Vector2.zero;

        // Hướng nhìn theo hướng di chuyển gần nhất.
        if (Keyboard.current.wKey.isPressed ||
            Keyboard.current.upArrowKey.isPressed)
            movement = Vector2.up;
        else if (Keyboard.current.sKey.isPressed ||
                 Keyboard.current.downArrowKey.isPressed)
            movement = Vector2.down;
        else if (Keyboard.current.aKey.isPressed ||
                 Keyboard.current.leftArrowKey.isPressed)
            movement = Vector2.left;
        else if (Keyboard.current.dKey.isPressed ||
                 Keyboard.current.rightArrowKey.isPressed)
            movement = Vector2.right;

        if (movement != Vector2.zero)
            facingDirection = movement;

        if (interactAction != null &&
            interactAction.action.WasPressedThisFrame())
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        if (isInteracting)
            return;

        Vector2 position =
            (Vector2)transform.position +
            facingDirection * interactionDistance;

        Collider2D hit = Physics2D.OverlapCircle(
            position,
            0.8f,
            interactableLayer
        );

        if (hit == null)
            return;

        IInteractable interactable = hit.GetComponent<IInteractable>();
        //InteractableBase interactable = hit.GetComponentInParent<InteractableBase>();

        if (interactable == null)
            return;

        StartCoroutine(InteractionRoutine());

        interactable.Interact();
    }
    private IEnumerator InteractionRoutine()
    {
        SetInteracting(true);

        yield return new WaitForSeconds(
            animator.GetCurrentAnimatorStateInfo(0).length
        );

        SetInteracting(false);
    }
    public void SetInteracting(bool value)
    {
        isInteracting = value;

        if (animator == null)
        {
            Debug.LogError("PlayerInteraction: Animator is NULL!");
            return;
        }

        animator.SetBool("isInteracting", value);

        Debug.Log("Set isInteracting = " + value);
    }
    public void RefreshInputAction()
    {
        if (interactAction != null)
        {
            interactAction.action.Enable();
        }

        Debug.Log("PlayerInteraction Input Action refreshed!");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector2 position = (Vector2)transform.position
                         + facingDirection * interactionDistance;

        Gizmos.DrawWireSphere(position, 0.8f);
    }
}
