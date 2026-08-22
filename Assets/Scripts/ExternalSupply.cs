using UnityEngine;

public class ExternalSupply : MonoBehaviour
{
    [Header("Supply Status")]
    [SerializeField] private bool electricityActive = true;
    [SerializeField] private bool waterActive = true;
    [SerializeField] private bool foodActive = true;
    [SerializeField] private bool technologyActive = true;

    public bool ElectricityActive => electricityActive;
    public bool WaterActive => waterActive;
    public bool FoodActive => foodActive;
    public bool TechnologyActive => technologyActive;

    public void DisableElectricity()
    {
        electricityActive = false;
    }

    public void DisableWater()
    {
        waterActive = false;
    }

    public void DisableFood()
    {
        foodActive = false;
    }

    public void DisableTechnology()
    {
        technologyActive = false;
    }
}