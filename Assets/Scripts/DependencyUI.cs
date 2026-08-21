using UnityEngine;
using TMPro;

public class DependencyUI : MonoBehaviour
{
    [Header("Dependency Texts")]
    [SerializeField] private TMP_Text electricityText;
    [SerializeField] private TMP_Text waterText;
    [SerializeField] private TMP_Text foodText;
    [SerializeField] private TMP_Text technologyText;

    [Header("Independence")]
    [SerializeField] private TMP_Text independenceText;

    private void Update()
    {
        if (DependencyManager.Instance == null)
            return;

        float electricity =
            DependencyManager.Instance.Electricity;

        float water =
            DependencyManager.Instance.Water;

        float food =
            DependencyManager.Instance.Food;

        float technology =
            DependencyManager.Instance.Technology;

        float independence =
            DependencyManager.Instance.GetIndependence();

        // Electricity
        if (electricityText != null)
        {
            electricityText.text =
                "Electricity   " +
                Mathf.RoundToInt(electricity) +
                "%";
        }

        // Water
        if (waterText != null)
        {
            waterText.text =
                "Water   " +
                Mathf.RoundToInt(water) +
                "%";
        }

        // Food
        if (foodText != null)
        {
            foodText.text =
                "Food   " +
                Mathf.RoundToInt(food) +
                "%";
        }

        // Technology
        if (technologyText != null)
        {
            technologyText.text =
                "Technology   " +
                Mathf.RoundToInt(technology) +
                "%";
        }

        // Independence
        if (independenceText != null)
        {
            independenceText.text =
                "INDEPENDENCE   " +
                Mathf.RoundToInt(independence) +
                "%";
        }
    }
}