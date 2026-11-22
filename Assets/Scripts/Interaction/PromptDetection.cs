using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PromptDetector : MonoBehaviour
{
    private CircleCollider2D detectionCollider;
    private SpriteRenderer promptRenderer;

    private int playerCount = 0;

    private void Awake()
    {
        detectionCollider = GetComponent<CircleCollider2D>();
        promptRenderer = GetComponent<SpriteRenderer>();

        detectionCollider.isTrigger = true;

        if (promptRenderer != null)
            promptRenderer.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerCount++;

        ShowPrompt();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerCount = Mathf.Max(0, playerCount - 1);

        if (playerCount == 0)
            HidePrompt();
    }

    private void ShowPrompt()
    {
        if (promptRenderer != null)
            promptRenderer.enabled = true;
    }

    private void HidePrompt()
    {
        if (promptRenderer != null)
            promptRenderer.enabled = false;
    }
}
