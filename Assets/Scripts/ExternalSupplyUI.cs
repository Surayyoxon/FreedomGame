using UnityEngine;
using TMPro;

public class ExternalSupplyUI : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TMP_Text electricityText;
    [SerializeField] private TMP_Text waterText;
    [SerializeField] private TMP_Text foodText;
    [SerializeField] private TMP_Text technologyText;

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

        UpdateText(
            electricityText,
            "Electricity",
            electricity
        );

        UpdateText(
            waterText,
            "Water",
            water
        );

        UpdateText(
            foodText,
            "Food",
            food
        );

        UpdateText(
            technologyText,
            "Technology",
            technology
        );
    }

    private void UpdateText(
        TMP_Text text,
        string resourceName,
        float dependency
    )
    {
        if (text == null)
            return;

        if (dependency <= 0)
        {
            text.text =
                resourceName +
                ": INDEPENDENT";
        }
        else
        {
            text.text =
                resourceName +
                ": " +
                Mathf.RoundToInt(dependency) +
                "% EXTERNAL";
        }
    }
}