using UnityEngine;

public class ExternalSupplyInteraction : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Debug.Log(
            "External Supply: Bu qishloq hali ham tashqi ta'minotga qaram."
        );
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
    }
}