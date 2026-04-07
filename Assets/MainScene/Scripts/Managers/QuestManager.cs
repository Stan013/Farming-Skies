using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour, IDataPersistence
{
    [Header("Permit unlocks")]
    public bool farmingUnlocked;
    public bool buildingUnlocked;
    public bool craftingUnlocked;
    public bool tradingUnlocked;

    [Header("Quest variables")]
    public bool questActive;
    public int questCount;

    public void LoadData(GameData data)
    {
        questCount = data.questCount;
        if (data.questActive)
        {
            GameManager.WM.OpenQuestWindow();
        }
    }

    public void SaveData(ref GameData data)
    {
        data.questActive = questActive;
        data.questCount = questCount;
    }
}