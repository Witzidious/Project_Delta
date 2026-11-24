using UnityEngine;

public class Camera : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Camera Settings")]
    [SerializeField]
    private Vector3 offset = new Vector3(0f, 0f, -10f);

    [SerializeField] private float smoothTime = 0.08f;

    private Vector3 velocity;

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 targetPosition = target.position + offset;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime
        );
    }
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}

