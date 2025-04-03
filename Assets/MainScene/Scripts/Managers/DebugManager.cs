using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public bool skipIntro;

    public void DebugInput()
    {
        if (Input.GetKeyUp(KeyCode.F1))
        {
            AddMoney();
        }
        if (Input.GetKeyUp(KeyCode.F2))
        {
            AddWater();
        }
        if (Input.GetKeyUp(KeyCode.F3))
        {
            AddFertiliser();
        }
        if (Input.GetKeyUp(KeyCode.Keypad1))
        {
            GameManager.DM.AddCardToDeck("CardWaterBarrelBuildable");
            GameManager.HM.SetCardsInHand();
        }
    }

    public void AddMoney()
    {
        GameManager.UM.Balance += 500;
    }

    public void AddWater()
    {
        GameManager.UM.Water += 500;
    }

    public void AddFertiliser()
    {
        GameManager.UM.Fertiliser += 500;
    }
}
