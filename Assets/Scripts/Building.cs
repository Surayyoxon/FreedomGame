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

    [Header("Production")]
    [SerializeField] private int foodProduction = 10;
    [SerializeField] private float productionInterval = 5f;

    [Header("Build Sound")]
    [SerializeField] private AudioClip buildSound;
    [SerializeField] private float buildSoundVolume = 0.8f;

    private bool activated = false;

    public void Activate()
    {
        if (activated)
            return;

        activated = true;

        if (buildSound != null)
        {
            AudioSource.PlayClipAtPoint(
                buildSound,
                transform.position,
                buildSoundVolume
            );
        }

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

                InvokeRepeating(
                    nameof(ProduceFood),
                    productionInterval,
                    productionInterval
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

        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance
                .BuildingCompleted(buildingType);

          
        }
    }

    private void ProduceFood()
    {
        if (ResourceManager.Instance == null)
        {
            Debug.LogError(
                "ResourceManager topilmadi!"
            );

            return;
        }

        ResourceManager.Instance.AddFood(
            foodProduction
        );

        Debug.Log(
            "Farm produced Food: +" +
            foodProduction
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