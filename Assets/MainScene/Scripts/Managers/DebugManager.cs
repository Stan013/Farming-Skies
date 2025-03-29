using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public bool skipIntro;

    public void DebugInput()
    {
        if (Input.GetKey(KeyCode.F1))
        {
            AddMoney();
        }
        if (Input.GetKey(KeyCode.F2))
        {
            AddWater();
        }
        if (Input.GetKey(KeyCode.F3))
        {
            AddFertiliser();
        }
    }

    public void AddMoney()
    {
        GameManager.UM.balance += 500;
        GameManager.UM.UpdateUI();
    }

    public void AddWater()
    {
        GameManager.UM.water += 500;
        GameManager.UM.UpdateUI();
    }

    public void AddFertiliser()
    {
        GameManager.UM.fertiliser += 500;
        GameManager.UM.UpdateUI();
    }
}
