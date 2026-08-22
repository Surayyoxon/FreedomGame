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

    private void Update()
    {
        if (DependencyManager.Instance == null)
            return;

        // DEPENDENCY

        electricityText.text =
            "Electricity   " +
            Mathf.RoundToInt(
                DependencyManager.Instance.Electricity
            ) + "%";

        waterText.text =
            "Water   " +
            Mathf.RoundToInt(
                DependencyManager.Instance.Water
            ) + "%";

        foodText.text =
            "Food   " +
            Mathf.RoundToInt(
                DependencyManager.Instance.Food
            ) + "%";

        technologyText.text =
            "Technology   " +
            Mathf.RoundToInt(
                DependencyManager.Instance.Technology
            ) + "%";

        independenceText.text =
            "INDEPENDENCE   " +
            DependencyManager.Instance
                .GetIndependence()
                .ToString("0") + "%";


        // REAL FOOD RESOURCE

        if (ResourceManager.Instance != null &&
            foodAmountText != null)
        {
            foodAmountText.text =
                "Food: " +
                ResourceManager.Instance.GetFood();
        }
    }
}