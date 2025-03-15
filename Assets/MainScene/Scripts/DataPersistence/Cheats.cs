using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    public void AddMoney()
    {
        GameManager.UM.money += 500;
    }
}
