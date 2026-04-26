using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class PermitManager : MonoBehaviour
{
    [Header("License cost variables")]
    [SerializeField] private int farmingPermitCost;
    [SerializeField] private int buildingPermitCost;
    [SerializeField] private int craftingPermitCost;
    [SerializeField] private int tradingPermitCost;

    [SerializeField] private GameObject farmingPermitUnlock;
    [SerializeField] private GameObject buildingPermitUnlock;
    [SerializeField] private GameObject craftingPermitUnlock;
    [SerializeField] private GameObject tradingPermitUnlock;

    [SerializeField] private GameObject farmingPermitAcquired;
    [SerializeField] private GameObject buildingPermitAcquired;
    [SerializeField] private GameObject craftingPermitAcquired;
    [SerializeField] private GameObject tradingPermitAcquired;

    [SerializeField] private Image farmingPermitIcon;
    [SerializeField] private Image buildingPermitIcon;
    [SerializeField] private Image craftingPermitIcon;
    [SerializeField] private Image tradingPermitIcon;

    [SerializeField] private Sprite farmingPermitAcquiredIcon;
    [SerializeField] private Sprite buildingPermitAcquiredIcon;
    [SerializeField] private Sprite craftingPermitAcquiredIcon;
    [SerializeField] private Sprite tradingPermitAcquiredIcon;

    [SerializeField] private TMP_Text farmingPermitText;
    [SerializeField] private TMP_Text buildingPermitText;
    [SerializeField] private TMP_Text craftingPermitText;
    [SerializeField] private TMP_Text tradingPermitText;

    public void AcquirePermit(string permitType)
    {
        switch (permitType)
        {
            case "Farming":
                if(GameManager.UM.Balance >= farmingPermitCost)
                {
                    GameManager.UM.Balance -= farmingPermitCost;
                    farmingPermitUnlock.SetActive(false);
                    farmingPermitAcquired.SetActive(true);
                    farmingPermitIcon.sprite = farmingPermitAcquiredIcon;   
                }
                break;
            case "Building":
                if(GameManager.UM.Balance >= buildingPermitCost)
                {
                    GameManager.UM.Balance -= buildingPermitCost;
                    buildingPermitUnlock.SetActive(false);
                    buildingPermitAcquired.SetActive(true);
                    buildingPermitIcon.sprite = buildingPermitAcquiredIcon;   
                }
                break;
            case "Crafting":
                if(GameManager.UM.Balance >= craftingPermitCost)
                {
                    GameManager.UM.Balance -= craftingPermitCost;
                    craftingPermitUnlock.SetActive(false);
                    craftingPermitAcquired.SetActive(true);
                    craftingPermitIcon.sprite = craftingPermitAcquiredIcon;   
                }
                break;
            case "Trading":
                if(GameManager.UM.Balance >= tradingPermitCost)
                {
                    GameManager.UM.Balance -= tradingPermitCost;
                    tradingPermitUnlock.SetActive(false);
                    tradingPermitAcquired.SetActive(true);
                    tradingPermitIcon.sprite = tradingPermitAcquiredIcon;   
                }
                break;
        }
    }
}
