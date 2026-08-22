using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    [Header("Starting Money")]
    [SerializeField] private int startingMoney = 100;

    [Header("Starting Food")]
    [SerializeField] private int startingFood = 0;

    public int Money { get; private set; }
    public int Food { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Money = startingMoney;
        Food = startingFood;
    }

    // =========================================
    // MONEY
    // =========================================

    public bool CanAfford(int amount)
    {
        return Money >= amount;
    }

    public bool SpendMoney(int amount)
    {
        if (Money < amount)
        {
            Debug.Log("Not enough money!");

            return false;
        }

        Money -= amount;

        Debug.Log("Money: " + Money);

        return true;
    }

    public void AddMoney(int amount)
    {
        Money += amount;

        Debug.Log("Money: " + Money);
    }

    public int GetMoney()
    {
        return Money;
    }

    // =========================================
    // FOOD
    // =========================================

    public void AddFood(int amount)
    {
        Food += amount;

        Debug.Log("Food: " + Food);
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

    public int GetFood()
    {
        return Food;
    }
}