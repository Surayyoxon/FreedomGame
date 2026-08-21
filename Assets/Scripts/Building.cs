using UnityEngine;

public class Building : MonoBehaviour
{
    public enum BuildingType
    {
        Solar,
        Well,
        Farm,
        Workshop
    }

    [Header("Building")]
    [SerializeField] private BuildingType buildingType;

    [Header("Cost")]
    [SerializeField] private int cost = 30;

    [Header("Dependency Reduction")]
    [SerializeField] private float dependencyReduction = 25f;

    private bool activated = false;

    public void Activate()
    {
        if (activated)
            return;

        activated = true;

        if (DependencyManager.Instance == null)
        {
            Debug.LogError(
                "DependencyManager topilmadi!"
            );

            return;
        }

        switch (buildingType)
        {
            case BuildingType.Solar:

                DependencyManager.Instance
                    .ReduceElectricity(
                        dependencyReduction
                    );

                break;

            case BuildingType.Well:

                DependencyManager.Instance
                    .ReduceWater(
                        dependencyReduction
                    );

                break;

            case BuildingType.Farm:

                DependencyManager.Instance
                    .ReduceFood(
                        dependencyReduction
                    );

                break;

            case BuildingType.Workshop:

                DependencyManager.Instance
                    .ReduceTechnology(
                        dependencyReduction
                    );

                break;
        }

        Debug.Log(
            buildingType +
            " built! Independence: " +
            DependencyManager.Instance
                .GetIndependence()
                .ToString("0") +
            "%"
        );
    }

    public int GetCost()
    {
        return cost;
    }

    public BuildingType GetBuildingType()
    {
        return buildingType;
    }
}