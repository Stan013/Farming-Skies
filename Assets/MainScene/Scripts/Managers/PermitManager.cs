using UnityEngine;
using UnityEngine.UI;

public class PermitManager : MonoBehaviour
{
    [Header("UI unlock buttons")]
    [SerializeField] private Button craftingButton;
    [SerializeField] private Sprite craftingButtonSprite;
    [SerializeField] private Button marketButton;
    [SerializeField] private Sprite marketButtonSprite;

    [Header("Action unlock variables")]
    public bool farmingAllowed;
    public bool buildingAllowed;

    public void AcquirePermit(Permit permit)
    {
        if(GameManager.UM.Balance >= permit.permitCost)
        {
            if(permit.permitLevel + 1 < permit.permitMaxLevel)
            {
                permit.PermitUpgrade();
            }
            else
            {
                permit.PermitFullyUpgraded();
            }

            if(permit.permitLevel == 0)
            {
                switch (permit.permitType)
                {
                    case "Farming":
                        farmingAllowed = true;   
                        break;
                    case "Building":
                            buildingAllowed = true;
                        break;
                    case "Crafting":
                            craftingButton.interactable = true;
                            craftingButton.GetComponent<Image>().sprite = craftingButtonSprite;
                        break;
                    case "Trading":
                            marketButton.interactable = true;
                            marketButton.GetComponent<Image>().sprite = marketButtonSprite;
                        break;
                }   
            }
        }
    }
}
