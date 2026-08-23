using UnityEngine;

public class FarmSeedPickup : MonoBehaviour
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

        ObjectiveManager.Instance.AddFarmSeed();

        Debug.Log("🌱 Seed collected!");

        Destroy(gameObject);
    }
}