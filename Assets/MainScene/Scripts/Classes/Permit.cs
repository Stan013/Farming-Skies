using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Permit : MonoBehaviour
{
    [Header("Permit variables")]
    public int permitCost;
    public GameObject permitUnlock;
    public GameObject permitAcquired;
    public Image permitIcon;
    public Sprite permitAcquiredIcon;
    public TMP_Text permitText;
    public string permitType;
    public int permitLevel;
    public int permitMaxLevel;

    public void PermitUpgrade()
    {
        
    }

    public void PermitFullyUpgraded()
    {
        GameManager.UM.Balance -= permitCost;
        permitUnlock.SetActive(false);
        permitAcquired.SetActive(true);
        permitIcon.sprite = permitAcquiredIcon;
    }
}
