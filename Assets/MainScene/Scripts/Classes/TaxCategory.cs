using UnityEngine;

public class TaxCategory : MonoBehaviour
{
    [SerializeField] private int permitCost;

    public void UnlockPermit(string permit)
    {
        switch (permit)
        {
            case "Farming":
                GameManager.QM.farmingUnlocked = true;
                break;
            case "Building":
                GameManager.QM.buildingUnlocked = true;
                break;
            case "Crafting":
                GameManager.QM.craftingUnlocked = true;
                break;
            case "Trading":
                GameManager.QM.tradingUnlocked = true;
                break;
        }
    }
}
