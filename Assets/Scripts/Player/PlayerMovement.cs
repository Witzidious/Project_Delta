using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float sprintSpeed = 5f;

    [Header("Input Settings")]
    [SerializeField] private InputActionReference walkAction;
    [SerializeField] private InputActionReference sprintAction;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movementInput;
    private Vector2 facingDirection = Vector2.down;
    private bool isSprinting;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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

        Debug.Log("PlayerMovement: Animator connected to "
                  + animator.gameObject.name);
    }

    private void OnEnable()
    {
        if (walkAction != null)
        {
            walkAction.action.Enable();
        }
        if (sprintAction != null)
        {
            sprintAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (walkAction != null)
        {
            walkAction.action.Disable();
        }
        if (sprintAction != null)
        {
            sprintAction.action.Disable();
        }
    }
    public void SetMovementSpeeds(float walk, float sprint)
    {
        walkSpeed = walk;
        sprintSpeed = sprint;
    }

    private void Update()
    {
        if (walkAction != null)
        {
            movementInput = walkAction.action.ReadValue<Vector2>();
            movementInput = Vector2.ClampMagnitude(movementInput, 1f);
        }

        if (sprintAction != null)
        {
            isSprinting = sprintAction.action.IsPressed();
        }
        UpdateFacingDirection();
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        float speed = isSprinting ? sprintSpeed : walkSpeed;
        rb.linearVelocity = movementInput * speed;
    }

    private void UpdateFacingDirection()
    {
        if (movementInput.sqrMagnitude < 0.01f) return;

        if (Mathf.Abs(movementInput.x) >
            Mathf.Abs(movementInput.y))
        {
            facingDirection = movementInput.x > 0
                ? Vector2.right
                : Vector2.left;
        }
        else
        {
            facingDirection = movementInput.y > 0
                ? Vector2.up
                : Vector2.down;
        }
    }

    private void UpdateAnimator()
    {
        if (animator == null)
            return;

        bool isMoving = movementInput.sqrMagnitude > 0.01f;

        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isSprinting", isSprinting);

        animator.SetFloat("moveX", isMoving ? facingDirection.x : 0f);
        animator.SetFloat("moveY", isMoving ? facingDirection.y : 0f);
    }
    void Start()
    {
        
    }

}
