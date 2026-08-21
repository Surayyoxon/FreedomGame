using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    [Header("Starting Resource")]
    [SerializeField] private int startingMoney = 100;

    public int Money { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Money = startingMoney;
    }

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
}