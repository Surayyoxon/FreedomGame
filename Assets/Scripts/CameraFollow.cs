using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    [Header("Camera Position")]
    [SerializeField] private float distance = 7f;
    [SerializeField] private float height = 4f;

    [Header("Smooth")]
    [SerializeField] private float positionSmooth = 10f;

    [Header("Collision")]
    [SerializeField] private float cameraRadius = 0.3f;
    [SerializeField] private float collisionOffset = 0.2f;

    [SerializeField] private LayerMask collisionLayers;

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 targetPosition =
            target.position + Vector3.up * height;

        Vector3 desiredPosition =
            targetPosition - target.forward * distance;

        Vector3 finalPosition = desiredPosition;

        Vector3 direction =
            desiredPosition - targetPosition;

        float desiredDistance = direction.magnitude;

        if (Physics.SphereCast(
            targetPosition,
            cameraRadius,
            direction.normalized,
            out RaycastHit hit,
            desiredDistance,
            collisionLayers))
        {
            finalPosition =
                targetPosition +
                direction.normalized *
                Mathf.Max(0.5f, hit.distance - collisionOffset);
        }

        transform.position = Vector3.Lerp(
            transform.position,
            finalPosition,
            positionSmooth * Time.deltaTime
        );

        Vector3 lookPosition =
            target.position + Vector3.up * 1f;

        transform.LookAt(lookPosition);
    }
}