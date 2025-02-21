using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CycleButton : MonoBehaviour
{
    public Button cycleButton;

    public void Start()
    {
        cycleButton.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        GameManager.CRM.SetCardCraftAmount(0);
        if (cycleButton.name == "LeftArrow")
        {
            GameManager.CRM.ChangeSelectedCard(-1);
        }
        else
        {
            GameManager.CRM.ChangeSelectedCard(1);
        }
    }
}
