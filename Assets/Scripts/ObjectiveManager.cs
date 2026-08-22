using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance;

    [Header("UI")]
    [SerializeField] private TMP_Text objectiveText;

    [Header("Objectives")]
    [SerializeField]
    private string[] objectives =
    {
        "Build a Solar Panel.",
        "Build a Well.",
        "Build a Farm.",
        "Build a Workshop.",
        "Make the village 100% independent!"
    };

    private int currentObjective = 0;

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
        UpdateObjectiveUI();
    }

    public void BuildingCompleted(Building.BuildingType buildingType)
    {
        if (currentObjective >= objectives.Length)
            return;

        bool correctBuilding = false;

        switch (currentObjective)
        {
            case 0:
                correctBuilding =
                    buildingType == Building.BuildingType.Solar;
                break;

            case 1:
                correctBuilding =
                    buildingType == Building.BuildingType.Well;
                break;

            case 2:
                correctBuilding =
                    buildingType == Building.BuildingType.Farm;
                break;

            case 3:
                correctBuilding =
                    buildingType == Building.BuildingType.Workshop;
                break;
        }

        if (!correctBuilding)
            return;

        currentObjective++;

        UpdateObjectiveUI();
    }

    public void CheckFinalObjective()
    {
        if (DependencyManager.Instance == null)
            return;

        if (DependencyManager.Instance.GetIndependence() >= 100f)
        {
            currentObjective = objectives.Length - 1;

            UpdateObjectiveUI();

            if (GameManager.Instance != null)
            {
                GameManager.Instance.WinGame();
            }
        }
    }

    private void UpdateObjectiveUI()
    {
        if (objectiveText == null)
            return;

        if (currentObjective >= objectives.Length)
        {
            objectiveText.text =
                "VILLAGE IS INDEPENDENT!";
            return;
        }

        objectiveText.text =
            "OBJECTIVE\n" +
            objectives[currentObjective];
    }
}