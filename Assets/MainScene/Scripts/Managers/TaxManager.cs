using UnityEngine;
using TMPro;

public class TaxManager : MonoBehaviour
{
    [Header("Total tax variables")]
    private float totalTax;
    private float totalLandTax;
    [SerializeField] private GameObject landTax;
    [SerializeField] private TMP_Text totalLandTaxText; 
    [SerializeField] private TMP_Text landTaxText; 
    private float totalStructureTax;
    [SerializeField] private GameObject structureTax;
    [SerializeField] private TMP_Text totalStructureTaxText; 
    [SerializeField] private TMP_Text structureTaxText; 
    private float totalAnimalTax;
    private float totalProductionTax;
    private float totalSalesTax;

    [Header("License cost variables")]
    [SerializeField] private int farmingLicense;
    [SerializeField] private int structureLicense;
    [SerializeField] private int animalLicense;
    [SerializeField] private int productionLicense;

    [Header("Inflation variables")]
    [SerializeField] private TMP_Text landInflationText; 
    public float landInflation;
    [SerializeField] private TMP_Text structureInflationText; 
    public float structureInflation;
    public float animalInflation;
    public float productionInflation;
    public float salesInflation;

    public void SetTaxes()
    {
        CalculateTaxes();

        if(totalLandTax == 0)
        {
            landTax.SetActive(false);
        }
        else
        {
            landTax.SetActive(true);
        }
                
        if(totalStructureTax == 0)
        {
            structureTax.SetActive(false);
        }
        else
        {
            structureTax.SetActive(true);
        }
        

        landTaxText.text = GameManager.EM.expenseIslandsTotal + " ₴";
        totalLandTaxText.text = GameManager.UM.FormatNumber(totalLandTax, true) + " ₴";
        structureTaxText.text = GameManager.EM.expenseStructuresTotal + " ₴";
        totalStructureTaxText.text = GameManager.UM.FormatNumber(totalStructureTax, true)+ " ₴";
    }

    public void CalculateTaxes()
    {
        totalLandTax = GameManager.EM.expenseIslandsTotal + (GameManager.EM.expenseIslandsTotal * (landInflation / 100f));
        totalStructureTax = GameManager.EM.expenseStructuresTotal + (GameManager.EM.expenseStructuresTotal * (structureInflation / 100f));
        totalAnimalTax = Mathf.FloorToInt(GameManager.EM.expenseAnimalsTotal * animalInflation);
        totalProductionTax = Mathf.FloorToInt(GameManager.EM.expenseProductionTotal * productionInflation);
        totalSalesTax = Mathf.FloorToInt(GameManager.EM.expenseSalesTotal * salesInflation);

        totalTax = totalLandTax + totalStructureTax + totalAnimalTax + totalProductionTax + totalSalesTax;
        GameManager.EM.Expense = totalTax;
    }

    public void GenerateInflation()
    {
        switch(GameManager.LM.FarmLevel)
        {
            case 1:
                landInflation = Mathf.Round(Random.Range(0f, 2f) * 10f) / 10f;
                break;
            case 2:
                landInflation = Mathf.Round(Random.Range(2f, 4f) * 10f) / 10f;
                break;
            case 3:
                landInflation = Mathf.Round(Random.Range(4f, 7f) * 10f) / 10f;
                break;
            case 4:
                landInflation = Mathf.Round(Random.Range(7f, 10f) * 10f) / 10f;
                break;
            default:
                landInflation = Mathf.Round(Random.Range(0f, 2f) * 10f) / 10f;
                break;
        }

        landInflationText.text = landInflation + " %";
        structureInflationText.text = structureInflation + " %";

        CalculateTaxes();
    }
}
