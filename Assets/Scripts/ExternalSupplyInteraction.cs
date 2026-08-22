using UnityEngine;

public class ExternalSupplyInteraction : MonoBehaviour
{
    [Header("Trade")]
    [SerializeField] private int tradeReward = 30;

    private bool playerInside = false;

    private void Update()
    {
        if (!playerInside)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Trade();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = true;

        Debug.Log(
            "External Supply: E tugmasini bosing."
        );
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = false;
    }

    private void Trade()
    {
        if (ResourceManager.Instance == null)
        {
            Debug.LogError(
                "ResourceManager topilmadi!"
            );

            return;
        }

        ResourceManager.Instance.AddMoney(
            tradeReward
        );

        Debug.Log(
            "Trade amalga oshdi! +" +
            tradeReward +
            " Money"
        );
    }
}