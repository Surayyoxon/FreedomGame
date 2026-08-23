using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    [Header("Building Prefabs")]
    [SerializeField] private GameObject solarPanelPrefab;
    [SerializeField] private GameObject wellPrefab;
    [SerializeField] private GameObject farmPrefab;
    [SerializeField] private GameObject workshopPrefab;

    [Header("Player")]
    [SerializeField] private Transform player;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;

    [Header("Placement")]
    [SerializeField] private float buildDistance = 10f;

    [Header("Ground")]
    [SerializeField] private LayerMask groundLayer;

    [Header("Height")]
    [SerializeField] private float groundOffset = 0f;

    private GameObject selectedPrefab;
    private GameObject previewObject;

    private bool isBuilding;
    private bool canBuild;

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera == null)
        {
            Debug.LogError(
                "BuildingSystem: Main Camera topilmadi!"
            );
        }

        if (player == null)
        {
            Debug.LogError(
                "BuildingSystem: Player ulanmagan!"
            );
        }
    }

    private void Update()
    {
        // Keyboard shortcuts
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectSolar();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectWell();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectFarm();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectWorkshop();
        }

        if (!isBuilding)
            return;

        UpdatePreview();

        if (Input.GetMouseButtonDown(0))
        {
            if (canBuild)
            {
                PlaceBuilding();
            }
            else
            {
                Debug.Log(
                    "Bu joyda bino qurib bo'lmaydi!"
                );
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancelBuilding();
        }
    }

    // ==================================================
    // CHECK WHETHER BUILDING IS CURRENTLY ALLOWED
    // ==================================================

    private bool IsBuildingAllowed(
        Building.BuildingType buildingType)
    {
        if (ObjectiveManager.Instance == null)
        {
            Debug.LogError(
                "ObjectiveManager topilmadi!"
            );

            return false;
        }

        ObjectiveManager.ObjectiveType objective =
            ObjectiveManager.Instance
                .GetCurrentObjective();

        switch (buildingType)
        {
            case Building.BuildingType.Solar:

                return objective ==
                    ObjectiveManager.ObjectiveType.BuildSolar;

            case Building.BuildingType.Well:

                return objective ==
                    ObjectiveManager.ObjectiveType.BuildWell;

            case Building.BuildingType.Farm:

                return objective ==
                    ObjectiveManager.ObjectiveType.BuildFarm;

            case Building.BuildingType.Workshop:

                return objective ==
                    ObjectiveManager.ObjectiveType.BuildWorkshop;
        }

        return false;
    }

    // ==================================================
    // SELECT BUILDING
    // ==================================================

    public void SelectBuilding(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError(
                "Building prefab ulanmagan!"
            );

            return;
        }

        Building building =
            prefab.GetComponent<Building>();

        if (building == null)
        {
            Debug.LogError(
                "Prefabda Building componenti yo'q!"
            );

            return;
        }

        // Objective ruxsat bermasa qurilmaydi
        if (!IsBuildingAllowed(
            building.GetBuildingType()))
        {
            Debug.Log(
                "Bu bino hali ochilmagan. " +
                "Avval joriy vazifani bajaring."
            );

            return;
        }

        // Oldingi preview
        if (previewObject != null)
        {
            Destroy(previewObject);
        }

        selectedPrefab = prefab;

        previewObject =
            Instantiate(selectedPrefab);

        isBuilding = true;
        canBuild = false;

        SetPreviewMode(true);

        Debug.Log(
            "Building selected: " +
            selectedPrefab.name
        );
    }

    // ==================================================
    // UI BUTTON METHODS
    // ==================================================

    public void SelectSolar()
    {
        SelectBuilding(solarPanelPrefab);
    }

    public void SelectWell()
    {
        SelectBuilding(wellPrefab);
    }

    public void SelectFarm()
    {
        SelectBuilding(farmPrefab);
    }

    public void SelectWorkshop()
    {
        SelectBuilding(workshopPrefab);
    }

    // ==================================================
    // UPDATE PREVIEW
    // ==================================================

    private void UpdatePreview()
    {
        if (mainCamera == null)
            return;

        if (previewObject == null)
            return;

        Ray ray =
            mainCamera.ScreenPointToRay(
                Input.mousePosition
            );

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            1000f,
            groundLayer))
        {
            Vector3 position =
                hit.point;

            position.y +=
                groundOffset;

            previewObject.transform.position =
                position;

            if (player != null)
            {
                float distance =
                    Vector3.Distance(
                        player.position,
                        hit.point
                    );

                canBuild =
                    distance <= buildDistance;
            }
            else
            {
                canBuild = true;
            }
        }
        else
        {
            canBuild = false;
        }
    }

    // ==================================================
    // PLACE BUILDING
    // ==================================================

    private void PlaceBuilding()
    {
        if (previewObject == null)
            return;

        Building building =
            previewObject.GetComponent<Building>();

        if (building == null)
        {
            Debug.LogError(
                "Previewda Building componenti yo'q!"
            );

            return;
        }

        // Yana bir marta tekshiramiz
        if (!IsBuildingAllowed(
            building.GetBuildingType()))
        {
            Debug.Log(
                "Bu bino uchun vazifa hali bajarilmagan!"
            );

            CancelBuilding();
            return;
        }

        Ray ray =
            mainCamera.ScreenPointToRay(
                Input.mousePosition
            );

        if (!Physics.Raycast(
            ray,
            out RaycastHit hit,
            1000f,
            groundLayer))
        {
            Debug.Log(
                "Ground topilmadi!"
            );

            return;
        }

        if (player != null)
        {
            float distance =
                Vector3.Distance(
                    player.position,
                    hit.point
                );

            if (distance > buildDistance)
            {
                Debug.Log(
                    "Playerdan juda uzoq!"
                );

                return;
            }
        }

        Vector3 finalPosition =
            hit.point;

        finalPosition.y +=
            groundOffset;

        previewObject.transform.position =
            finalPosition;

        SetPreviewMode(false);

        building.Activate();

        Debug.Log(
            "Building qurildi: " +
            building.GetBuildingType()
        );

        previewObject = null;
        selectedPrefab = null;

        isBuilding = false;
        canBuild = false;
    }

    // ==================================================
    // CANCEL
    // ==================================================

    public void CancelBuilding()
    {
        if (previewObject != null)
        {
            Destroy(previewObject);
        }

        previewObject = null;
        selectedPrefab = null;

        isBuilding = false;
        canBuild = false;

        Debug.Log(
            "Building bekor qilindi."
        );
    }

    // ==================================================
    // PREVIEW MODE
    // ==================================================

    private void SetPreviewMode(bool preview)
    {
        if (previewObject == null)
            return;

        Collider[] colliders =
            previewObject
                .GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            if (collider != null)
            {
                collider.enabled =
                    !preview;
            }
        }
    }
}