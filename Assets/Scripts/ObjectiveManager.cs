using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance;

    public enum ObjectiveType
    {
        CollectCoinsForSolar,
        BuildSolar,
        CollectMaterialsForWell,
        BuildWell,
        CollectSeedsForFarm,
        BuildFarm,
        CollectPartsForWorkshop,
        BuildWorkshop,
        Complete
    }

    [Header("UI")]
    [SerializeField] private TMP_Text objectiveTitleText;
    [SerializeField] private TMP_Text objectiveDescriptionText;
    [SerializeField] private TMP_Text progressText;

    [Header("Current Objective")]
    [SerializeField] private ObjectiveType currentObjective;

    [Header("Required Amounts")]
    [SerializeField] private int solarCoinsRequired = 5;
    [SerializeField] private int wellMaterialsRequired = 3;
    [SerializeField] private int farmSeedsRequired = 3;
    [SerializeField] private int workshopPartsRequired = 3;

    private int solarCoins;
    private int wellMaterials;
    private int farmSeeds;
    private int workshopParts;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        currentObjective =
            ObjectiveType.CollectCoinsForSolar;

        UpdateUI();
    }

    // =========================================
    // OBJECTIVE PROGRESS
    // =========================================

    public void AddSolarCoin()
    {
        if (currentObjective !=
            ObjectiveType.CollectCoinsForSolar)
            return;

        solarCoins++;

        if (solarCoins >= solarCoinsRequired)
        {
            currentObjective =
                ObjectiveType.BuildSolar;
        }

        UpdateUI();
    }

    public void AddWellMaterial()
    {
        if (currentObjective !=
            ObjectiveType.CollectMaterialsForWell)
            return;

        wellMaterials++;

        if (wellMaterials >= wellMaterialsRequired)
        {
            currentObjective =
                ObjectiveType.BuildWell;
        }

        UpdateUI();
    }

    public void AddFarmSeed()
    {
        if (currentObjective !=
            ObjectiveType.CollectSeedsForFarm)
            return;

        farmSeeds++;

        if (farmSeeds >= farmSeedsRequired)
        {
            currentObjective =
                ObjectiveType.BuildFarm;
        }

        UpdateUI();
    }

    public void AddWorkshopPart()
    {
        if (currentObjective !=
            ObjectiveType.CollectPartsForWorkshop)
            return;

        workshopParts++;

        if (workshopParts >= workshopPartsRequired)
        {
            currentObjective =
                ObjectiveType.BuildWorkshop;
        }

        UpdateUI();
    }

    // =========================================
    // BUILDING COMPLETED
    // =========================================

    public void BuildingCompleted(
        Building.BuildingType buildingType)
    {
        switch (currentObjective)
        {
            case ObjectiveType.BuildSolar:

                if (buildingType ==
                    Building.BuildingType.Solar)
                {
                    currentObjective =
                        ObjectiveType.CollectMaterialsForWell;
                }

                break;

            case ObjectiveType.BuildWell:

                if (buildingType ==
                    Building.BuildingType.Well)
                {
                    currentObjective =
                        ObjectiveType.CollectSeedsForFarm;
                }

                break;

            case ObjectiveType.BuildFarm:

                if (buildingType ==
                    Building.BuildingType.Farm)
                {
                    currentObjective =
                        ObjectiveType.CollectPartsForWorkshop;
                }

                break;

            case ObjectiveType.BuildWorkshop:

                if (buildingType ==
                    Building.BuildingType.Workshop)
                {
                    currentObjective =
                        ObjectiveType.Complete;
                }

                break;
        }

        UpdateUI();
    }

    // =========================================
    // FINAL
    // =========================================

    public void CheckFinalObjective()
    {
        if (DependencyManager.Instance == null)
            return;

        if (currentObjective !=
            ObjectiveType.Complete)
            return;

        if (DependencyManager.Instance
            .GetIndependence() >= 100f)
        {
            UpdateUI();

            if (GameManager.Instance != null)
            {
                GameManager.Instance.WinGame();
            }
        }
    }

    // =========================================
    // UI
    // =========================================

    private void UpdateUI()
    {
        if (objectiveTitleText == null ||
            objectiveDescriptionText == null ||
            progressText == null)
            return;

        switch (currentObjective)
        {
            case ObjectiveType.CollectCoinsForSolar:

                objectiveTitleText.text =
                    "VAZIFA";

                objectiveDescriptionText.text =
                    "☀️ SOLAR ENERGIYA\n" +
                    "5 ta tanga to'plang.";

                progressText.text =
                    "Tangalar: " +
                    solarCoins +
                    " / " +
                    solarCoinsRequired;

                break;

            case ObjectiveType.BuildSolar:

                objectiveTitleText.text =
                    "VAZIFA";

                objectiveDescriptionText.text =
                    "☀️ SOLAR PANELNI QURING";

                progressText.text =
                    "Tayyor!";

                break;

            case ObjectiveType.CollectMaterialsForWell:

                objectiveTitleText.text =
                    "VAZIFA";

                objectiveDescriptionText.text =
                    "💧 QUDUQ UCHUN\n" +
                    "3 ta material toping.";

                progressText.text =
                    "Materiallar: " +
                    wellMaterials +
                    " / " +
                    wellMaterialsRequired;

                break;

            case ObjectiveType.BuildWell:

                objectiveTitleText.text =
                    "VAZIFA";

                objectiveDescriptionText.text =
                    "💧 QUDUQNI QURING";

                progressText.text =
                    "Tayyor!";

                break;

            case ObjectiveType.CollectSeedsForFarm:

                objectiveTitleText.text =
                    "VAZIFA";

                objectiveDescriptionText.text =
                    "🌾 3 ta urug' toping.";

                progressText.text =
                    "Urug'lar: " +
                    farmSeeds +
                    " / " +
                    farmSeedsRequired;

                break;

            case ObjectiveType.BuildFarm:

                objectiveTitleText.text =
                    "VAZIFA";

                objectiveDescriptionText.text =
                    "🌾 FERMANI QURING";

                progressText.text =
                    "Tayyor!";

                break;

            case ObjectiveType.CollectPartsForWorkshop:

                objectiveTitleText.text =
                    "VAZIFA";

                objectiveDescriptionText.text =
                    "🔧 3 ta texnika detalini toping.";

                progressText.text =
                    "Detallar: " +
                    workshopParts +
                    " / " +
                    workshopPartsRequired;

                break;

            case ObjectiveType.BuildWorkshop:

                objectiveTitleText.text =
                    "VAZIFA";

                objectiveDescriptionText.text =
                    "🔧 USTAXONANI QURING";

                progressText.text =
                    "Tayyor!";

                break;

            case ObjectiveType.Complete:

                objectiveTitleText.text =
                    "YAKUNIY VAZIFA";

                objectiveDescriptionText.text =
                    "QISHLOQNI 100% MUSTAQIL QILING.";

                progressText.text =
                    "Deyarli tayyor!";

                break;
        }
    }
}