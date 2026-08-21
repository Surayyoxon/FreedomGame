using UnityEngine;

public class DependencyManager : MonoBehaviour
{
    public static DependencyManager Instance;

    [Header("Dependency Levels")]

    [Range(0, 100)]
    [SerializeField] private float electricity = 90f;

    [Range(0, 100)]
    [SerializeField] private float water = 80f;

    [Range(0, 100)]
    [SerializeField] private float food = 85f;

    [Range(0, 100)]
    [SerializeField] private float technology = 95f;

    public float Electricity => electricity;
    public float Water => water;
    public float Food => food;
    public float Technology => technology;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // =========================================
    // ELECTRICITY
    // =========================================

    public void ReduceElectricity(float amount)
    {
        electricity =
            Mathf.Clamp(
                electricity - amount,
                0f,
                100f
            );

        CheckVictory();
    }

    // =========================================
    // WATER
    // =========================================

    public void ReduceWater(float amount)
    {
        water =
            Mathf.Clamp(
                water - amount,
                0f,
                100f
            );

        CheckVictory();
    }

    // =========================================
    // FOOD
    // =========================================

    public void ReduceFood(float amount)
    {
        food =
            Mathf.Clamp(
                food - amount,
                0f,
                100f
            );

        CheckVictory();
    }

    // =========================================
    // TECHNOLOGY
    // =========================================

    public void ReduceTechnology(float amount)
    {
        technology =
            Mathf.Clamp(
                technology - amount,
                0f,
                100f
            );

        CheckVictory();
    }

    // =========================================
    // INDEPENDENCE
    // =========================================

    public float GetIndependence()
    {
        float totalDependency =
            electricity +
            water +
            food +
            technology;

        float averageDependency =
            totalDependency / 4f;

        float independence =
            100f - averageDependency;

        return Mathf.Clamp(
            independence,
            0f,
            100f
        );
    }

    // =========================================
    // VICTORY CHECK
    // =========================================

    private void CheckVictory()
    {
        if (electricity <= 0f &&
            water <= 0f &&
            food <= 0f &&
            technology <= 0f)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.WinGame();
            }
        }
    }
}