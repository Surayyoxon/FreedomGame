using UnityEngine;
using TMPro;

public class DependencyUI : MonoBehaviour
{
    [Header("Dependency Texts")]
    [SerializeField] private TMP_Text electricityText;
    [SerializeField] private TMP_Text waterText;
    [SerializeField] private TMP_Text foodText;
    [SerializeField] private TMP_Text technologyText;
    [SerializeField] private TMP_Text independenceText;

    [Header("Resource Texts")]
    [SerializeField] private TMP_Text foodAmountText;
    [SerializeField] private TMP_Text moneyText;

    private void Update()
    {
        if (DependencyManager.Instance == null)
            return;

        // DEPENDENCY

        if (electricityText != null)
        {
            electricityText.text =
                "Electricity   " +
                Mathf.RoundToInt(
                    DependencyManager.Instance.Electricity
                ) + "%";
        }

        if (waterText != null)
        {
            waterText.text =
                "Water   " +
                Mathf.RoundToInt(
                    DependencyManager.Instance.Water
                ) + "%";
        }

        if (foodText != null)
        {
            foodText.text =
                "Food   " +
                Mathf.RoundToInt(
                    DependencyManager.Instance.Food
                ) + "%";
        }

        if (technologyText != null)
        {
            technologyText.text =
                "Technology   " +
                Mathf.RoundToInt(
                    DependencyManager.Instance.Technology
                ) + "%";
        }

        if (independenceText != null)
        {
            independenceText.text =
                "INDEPENDENCE   " +
                DependencyManager.Instance
                    .GetIndependence()
                    .ToString("0") +
                "%";
        }

        // REAL FOOD

        if (ResourceManager.Instance != null &&
            foodAmountText != null)
        {
            foodAmountText.text =
                "Food: " +
                ResourceManager.Instance.GetFood();
        }

        // MONEY

        if (ResourceManager.Instance != null &&
            moneyText != null)
        {
            moneyText.text =
                "MONEY: $" +
                ResourceManager.Instance.GetMoney();
        }
    }
}