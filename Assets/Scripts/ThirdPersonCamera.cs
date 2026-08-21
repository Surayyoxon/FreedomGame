using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    [Header("Camera")]
    [SerializeField] private float distance = 7f;
    [SerializeField] private float height = 3f;

    [Header("Smooth")]
    [SerializeField] private float positionSmooth = 10f;
    [SerializeField] private float rotationSmooth = 10f;

    private void LateUpdate()
    {
        if (target == null)
            return;

        // Kameraning doimiy joylashuvi
        Vector3 desiredPosition =
            target.position
            + new Vector3(0f, height, -distance);

        // Smooth movement
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            positionSmooth * Time.deltaTime
        );

        // Playerga qarab turadi
        Vector3 direction = target.position - transform.position;

        Quaternion desiredRotation =
            Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            desiredRotation,
            rotationSmooth * Time.deltaTime
        );
    }
}