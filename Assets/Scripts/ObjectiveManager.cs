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
    [SerializeField]
    private ObjectiveType currentObjective =
        ObjectiveType.CollectCoinsForSolar;

    [Header("Required Amounts")]
    [SerializeField] private int solarCoinsRequired = 5;
    [SerializeField] private int wellMaterialsRequired = 3;
    [SerializeField] private int farmSeedsRequired = 3;
    [SerializeField] private int workshopPartsRequired = 3;

    [Header("Progress")]
    [SerializeField] private int solarCoins = 0;
    [SerializeField] private int wellMaterials = 0;
    [SerializeField] private int farmSeeds = 0;
    [SerializeField] private int workshopParts = 0;

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

    // ==================================================
    // CURRENT OBJECTIVE
    // ==================================================

    public ObjectiveType GetCurrentObjective()
    {
        return currentObjective;
    }

    // ==================================================
    // SOLAR COINS
    // ==================================================

    public void AddSolarCoin()
    {
        // Faqat Solar vazifasi faol bo'lsa hisoblaymiz
        if (currentObjective !=
            ObjectiveType.CollectCoinsForSolar)
        {
            return;
        }

        if (solarCoins >= solarCoinsRequired)
        {
            return;
        }

        solarCoins++;

        Debug.Log(
            "Solar Coins: " +
            solarCoins +
            " / " +
            solarCoinsRequired
        );

        if (solarCoins >= solarCoinsRequired)
        {
            currentObjective =
                ObjectiveType.BuildSolar;

            Debug.Log(
                "✅ Solar coins complete!"
            );

            Debug.Log(
                "🎯 New objective: Build Solar Panel."
            );
        }

        UpdateUI();
    }

    // ==================================================
    // WELL MATERIALS
    // ==================================================

    public void AddWellMaterial()
    {
        if (currentObjective !=
            ObjectiveType.CollectMaterialsForWell)
        {
            return;
        }

        if (wellMaterials >= wellMaterialsRequired)
        {
            return;
        }

        wellMaterials++;

        Debug.Log(
            "Well Materials: " +
            wellMaterials +
            " / " +
            wellMaterialsRequired
        );

        if (wellMaterials >= wellMaterialsRequired)
        {
            currentObjective =
                ObjectiveType.BuildWell;

            Debug.Log(
                "✅ Well materials complete!"
            );

            Debug.Log(
                "🎯 New objective: Build Well."
            );

            // Well buttonni ochamiz
            if (GameManager.Instance != null)
            {
                GameManager.Instance.UnlockWell();
            }
        }

        UpdateUI();
    }

    // ==================================================
    // FARM SEEDS
    // ==================================================

    public void AddFarmSeed()
    {
        if (currentObjective !=
            ObjectiveType.CollectSeedsForFarm)
        {
            return;
        }

        if (farmSeeds >= farmSeedsRequired)
        {
            return;
        }

        farmSeeds++;

        Debug.Log(
            "Farm Seeds: " +
            farmSeeds +
            " / " +
            farmSeedsRequired
        );

        if (farmSeeds >= farmSeedsRequired)
        {
            currentObjective =
                ObjectiveType.BuildFarm;

            Debug.Log(
                "✅ Farm seeds complete!"
            );

            Debug.Log(
                "🎯 New objective: Build Farm."
            );

            // Farm buttonni ochamiz
            if (GameManager.Instance != null)
            {
                GameManager.Instance.UnlockFarm();
            }
        }

        UpdateUI();
    }

    // ==================================================
    // WORKSHOP PARTS
    // ==================================================

    public void AddWorkshopPart()
    {
        // Eng muhim tekshiruv:
        // faqat Farm qurilgandan keyin hisoblanadi.
        if (currentObjective !=
            ObjectiveType.CollectPartsForWorkshop)
        {
            Debug.Log(
                "Workshop part picked up, " +
                "but current objective is: " +
                currentObjective
            );

            return;
        }

        if (workshopParts >= workshopPartsRequired)
        {
            return;
        }

        workshopParts++;

        Debug.Log(
            "Workshop Parts: " +
            workshopParts +
            " / " +
            workshopPartsRequired
        );

        if (workshopParts >= workshopPartsRequired)
        {
            currentObjective =
                ObjectiveType.BuildWorkshop;

            Debug.Log(
                "✅ Workshop parts complete!"
            );

            Debug.Log(
                "🎯 New objective: Build Workshop."
            );

            // Workshop buttonni ochamiz
            if (GameManager.Instance != null)
            {
                GameManager.Instance.UnlockWorkshop();
            }
        }

        UpdateUI();
    }

    // ==================================================
    // BUILDING COMPLETED
    // ==================================================

    public void BuildingCompleted(
        Building.BuildingType buildingType)
    {
        // ------------------------------------------
        // SOLAR
        // ------------------------------------------

        if (currentObjective ==
            ObjectiveType.BuildSolar &&
            buildingType ==
            Building.BuildingType.Solar)
        {
            currentObjective =
                ObjectiveType.CollectMaterialsForWell;

            Debug.Log(
                "✅ SOLAR BUILT!"
            );

            Debug.Log(
                "🎯 NEW OBJECTIVE: " +
                "Find 3 materials for Well."
            );

            UpdateUI();
            return;
        }

        // ------------------------------------------
        // WELL
        // ------------------------------------------

        if (currentObjective ==
            ObjectiveType.BuildWell &&
            buildingType ==
            Building.BuildingType.Well)
        {
            currentObjective =
                ObjectiveType.CollectSeedsForFarm;

            Debug.Log(
                "✅ WELL BUILT!"
            );

            Debug.Log(
                "🎯 NEW OBJECTIVE: " +
                "Find 3 seeds for Farm."
            );

            UpdateUI();
            return;
        }

        // ------------------------------------------
        // FARM
        // ------------------------------------------

        if (currentObjective ==
            ObjectiveType.BuildFarm &&
            buildingType ==
            Building.BuildingType.Farm)
        {
            currentObjective =
                ObjectiveType.CollectPartsForWorkshop;

            Debug.Log(
                "✅ FARM BUILT!"
            );

            Debug.Log(
                "🎯 NEW OBJECTIVE: " +
                "Find 3 parts for Workshop."
            );

            UpdateUI();
            return;
        }

        // ------------------------------------------
        // WORKSHOP
        // ------------------------------------------

        if (currentObjective ==
            ObjectiveType.BuildWorkshop &&
            buildingType ==
            Building.BuildingType.Workshop)
        {
            currentObjective =
                ObjectiveType.Complete;

            Debug.Log(
                "✅ WORKSHOP BUILT!"
            );

            Debug.Log(
                "🎯 ALL BUILDINGS COMPLETE!"
            );

            UpdateUI();

            CheckFinalObjective();
            return;
        }

        Debug.Log(
            "BuildingCompleted called, but it does not " +
            "match the current objective."
        );
    }

    // ==================================================
    // FINAL OBJECTIVE
    // ==================================================

    private void CheckFinalObjective()
    {
        // Workshop hali qurilmagan bo'lsa
        if (currentObjective !=
            ObjectiveType.Complete)
        {
            return;
        }

        if (DependencyManager.Instance == null)
        {
            Debug.LogError(
                "DependencyManager topilmadi!"
            );

            return;
        }

        float independence =
            DependencyManager.Instance
                .GetIndependence();

        Debug.Log(
            "Final Independence: " +
            independence.ToString("0") +
            "%"
        );

        // Faqat barcha dependency 0 bo'lsa
        if (DependencyManager.Instance
            .IsFullyIndependent())
        {
            Debug.Log(
                "🏆 VILLAGE IS FULLY INDEPENDENT!"
            );

            if (GameManager.Instance != null)
            {
                GameManager.Instance.WinGame();
            }
        }
        else
        {
            Debug.Log(
                "All buildings are built, " +
                "but village is not fully independent."
            );
        }
    }

    // ==================================================
    // UI
    // ==================================================

    private void UpdateUI()
    {
        if (objectiveTitleText == null ||
            objectiveDescriptionText == null ||
            progressText == null)
        {
            return;
        }

        switch (currentObjective)
        {
            // =========================================
            // SOLAR COINS
            // =========================================

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

            // =========================================
            // BUILD SOLAR
            // =========================================

            case ObjectiveType.BuildSolar:

                objectiveTitleText.text =
                    "VAZIFA";

                objectiveDescriptionText.text =
                    "☀️ SOLAR PANELNI QURING";

                progressText.text =
                    "Tayyor!";

                break;

            // =========================================
            // WELL MATERIALS
            // =========================================

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

            // =========================================
            // BUILD WELL
            // =========================================

            case ObjectiveType.BuildWell:

                objectiveTitleText.text =
                    "VAZIFA";

                objectiveDescriptionText.text =
                    "💧 QUDUQNI QURING";

                progressText.text =
                    "Tayyor!";

                break;

            // =========================================
            // FARM SEEDS
            // =========================================

            case ObjectiveType.CollectSeedsForFarm:

                objectiveTitleText.text =
                    "VAZIFA";

                objectiveDescriptionText.text =
                    "🌱 3 ta urug' toping.";

                progressText.text =
                    "Urug'lar: " +
                    farmSeeds +
                    " / " +
                    farmSeedsRequired;

                break;

            // =========================================
            // BUILD FARM
            // =========================================

            case ObjectiveType.BuildFarm:

                objectiveTitleText.text =
                    "VAZIFA";

                objectiveDescriptionText.text =
                    "🌾 FERMANI QURING";

                progressText.text =
                    "Tayyor!";

                break;

            // =========================================
            // WORKSHOP PARTS
            // =========================================

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

            // =========================================
            // BUILD WORKSHOP
            // =========================================

            case ObjectiveType.BuildWorkshop:

                objectiveTitleText.text =
                    "VAZIFA";

                objectiveDescriptionText.text =
                    "🔧 USTAXONANI QURING";

                progressText.text =
                    "Tayyor!";

                break;

            // =========================================
            // COMPLETE
            // =========================================

            case ObjectiveType.Complete:

                objectiveTitleText.text =
                    "YAKUNIY VAZIFA";

                objectiveDescriptionText.text =
                    "QISHLOQNI 100% MUSTAQIL QILING.";

                progressText.text =
                    "Barcha binolar qurildi!";

                break;
        }
    }

    // ==================================================
    // GETTERS
    // ==================================================

    public int GetSolarCoins()
    {
        return solarCoins;
    }

    public int GetWellMaterials()
    {
        return wellMaterials;
    }

    public int GetFarmSeeds()
    {
        return farmSeeds;
    }

    public int GetWorkshopParts()
    {
        return workshopParts;
    }
}