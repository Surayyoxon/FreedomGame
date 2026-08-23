using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    [Header("Starting Food")]
    [SerializeField] private int startingFood = 0;

    public int Food { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Food = startingFood;
    }

    public void AddFood(int amount)
    {
        Food += amount;

        Debug.Log("Food: " + Food);
    }

    public int GetFood()
    {
        return Food;
    }

    public bool CanAffordFood(int amount)
    {
        return Food >= amount;
    }

    public bool SpendFood(int amount)
    {
        if (Food < amount)
        {
            Debug.Log("Not enough food!");
            return false;
        }

        Food -= amount;

        Debug.Log("Food: " + Food);

        return true;
    }
}