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
        if (cycleButton.name == "LeftArrow")
        {
            GameManager.CRM.ChangeCraftingCard(-1);
        }
        else
        {
            GameManager.CRM.ChangeCraftingCard(1);
        }
    }
}
