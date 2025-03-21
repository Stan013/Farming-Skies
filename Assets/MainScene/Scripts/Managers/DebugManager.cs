using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public bool skipIntro;

    public void AddMoney()
    {
        GameManager.UM.balance += 500;
    }
}
