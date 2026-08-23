using UnityEngine;

public class DependencyManager : MonoBehaviour
{
    public static DependencyManager Instance;

    [Header("Dependency Levels")]
    [SerializeField] private float electricity = 90f;
    [SerializeField] private float water = 80f;
    [SerializeField] private float food = 85f;
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

    public void ReduceElectricity(float amount)
    {
        electricity = 0f;
    }

    public void ReduceWater(float amount)
    {
        water = 0f;
    }

    public void ReduceFood(float amount)
    {
        food = 0f;
    }

    public void ReduceTechnology(float amount)
    {
        technology = 0f;
    }

    public void SetElectricityIndependent()
    {
        electricity = 0f;
    }

    public void SetWaterIndependent()
    {
        water = 0f;
    }

    public void SetFoodIndependent()
    {
        food = 0f;
    }

    public void SetTechnologyIndependent()
    {
        technology = 0f;
    }

    public float GetIndependence()
    {
        float average =
            (electricity + water + food + technology) / 4f;

        return Mathf.Clamp(100f - average, 0f, 100f);
    }

    public bool IsFullyIndependent()
    {
        return electricity <= 0f &&
               water <= 0f &&
               food <= 0f &&
               technology <= 0f;
    }
}