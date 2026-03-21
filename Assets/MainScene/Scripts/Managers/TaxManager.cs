using UnityEngine;

public class TaxManager : MonoBehaviour
{
    [Header("Total tax variables")]
    [SerializeField] private float totalLandTax;
    [SerializeField] private float totalStructureTax;
    [SerializeField] private float totalAnimalTax;
    [SerializeField] private float totalProductionTax;
    [SerializeField] private float totalSalesTax;

    [Header("License cost variables")]
    [SerializeField] private float farmingLicense;
    [SerializeField] private float structureLicense;
    [SerializeField] private float animalLicense;
    [SerializeField] private float productionLicense;

    [Header("Inflation variables")]
    public float landInflation;
    public float structureInflation;
    public float animalInflation;
    public float productionInflation;
    public float salesInflation;

    public void CalculateTaxes()
    {
        totalLandTax = GameManager.EM.expenseIslandsTotal * landInflation;
        totalStructureTax = GameManager.EM.expenseStructuresTotal * structureInflation;
        totalAnimalTax = GameManager.EM.expenseAnimalsTotal * animalInflation;
        totalProductionTax = GameManager.EM.expenseProductionTotal * productionInflation;
        totalSalesTax = GameManager.EM.expenseSalesTotal * salesInflation;
    }
}
