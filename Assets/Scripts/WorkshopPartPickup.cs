using UnityEngine;

public class WorkshopPartPickup : MonoBehaviour
{
    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;

        if (!other.CompareTag("Player"))
            return;

        if (ObjectiveManager.Instance == null)
            return;

        collected = true;

        ObjectiveManager.Instance.AddWorkshopPart();

        Debug.Log("🔧 Workshop part collected!");

        Destroy(gameObject);
    }
}