using System.Collections;
using UnityEngine;

public class IntroCameraController : MonoBehaviour
{
    [SerializeField] private Transform introCameraPoint;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private float introDuration = 5f;

    private IEnumerator Start()
    {
        // CameraFollow'ni o'chiramiz
        if (cameraFollow != null)
        {
            cameraFollow.enabled = false;
        }

        if (introCameraPoint == null)
        {
            Debug.LogError("IntroCameraPoint ulanmagan!");
            yield break;
        }

        // Kamerani aniq pointga qo'yamiz
        transform.SetPositionAndRotation(
            introCameraPoint.position,
            introCameraPoint.rotation
        );

        // 5 soniya shu joyda QOTADI
        float timer = 0f;

        while (timer < introDuration)
        {
            transform.SetPositionAndRotation(
                introCameraPoint.position,
                introCameraPoint.rotation
            );

            timer += Time.deltaTime;

            yield return null;
        }

        // Intro tugadi
        if (cameraFollow != null)
        {
            cameraFollow.enabled = true;
        }
    }
}