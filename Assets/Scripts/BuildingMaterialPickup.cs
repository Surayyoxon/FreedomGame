using UnityEngine;

public class BuildingMaterialPickup : MonoBehaviour
{
    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;

        if (!other.CompareTag("Player"))
            return;

        collected = true;

        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.AddWellMaterial();
        }

        Debug.Log("Building material collected!");

        Destroy(gameObject);
    }
}