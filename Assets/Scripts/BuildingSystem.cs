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

        if (ResourceManager.Instance == null)
        {
            Debug.LogError(
                "BuildingSystem: ResourceManager topilmadi!"
            );
        }
    }

    private void Update()
    {
        /*
         * B bosilganda Solar avtomatik tanlanmaydi.
         * Hozircha:
         *
         * 1 = Solar
         * 2 = Well
         * 3 = Farm
         * 4 = Workshop
         */

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectBuilding(solarPanelPrefab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectBuilding(wellPrefab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectBuilding(farmPrefab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectBuilding(workshopPrefab);
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
    // SELECT BUILDING
    // ==================================================

    private void SelectBuilding(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError(
                "Building prefab ulanmagan!"
            );

            return;
        }

        // Oldingi preview bo'lsa o'chiramiz
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

        Building building =
            previewObject.GetComponent<Building>();

        if (building == null)
        {
            Debug.LogError(
                "Prefabda Building componenti yo'q!"
            );

            return;
        }

        int cost =
            building.GetCost();

        if (ResourceManager.Instance == null)
        {
            Debug.LogError(
                "ResourceManager topilmadi!"
            );

            return;
        }

        if (!ResourceManager.Instance
            .CanAfford(cost))
        {
            Debug.Log(
                "Pul yetarli emas! Cost: " +
                cost
            );

            return;
        }

        bool spent =
            ResourceManager.Instance
                .SpendMoney(cost);

        if (!spent)
            return;

        // Aynan mouse bosilgan joy
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
            selectedPrefab.name
        );

        Debug.Log(
            "Cost: " +
            cost
        );

        previewObject = null;
        selectedPrefab = null;

        isBuilding = false;
        canBuild = false;
    }

    // ==================================================
    // CANCEL
    // ==================================================

    private void CancelBuilding()
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

    private void SetPreviewMode(
        bool preview
    )
    {
        if (previewObject == null)
            return;

        Collider[] colliders =
            previewObject
                .GetComponentsInChildren<Collider>();

        foreach (
            Collider collider
            in colliders)
        {
            if (collider != null)
            {
                collider.enabled =
                    !preview;
            }
        }
    }
}