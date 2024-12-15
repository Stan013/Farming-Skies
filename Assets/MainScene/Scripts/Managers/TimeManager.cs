using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private List<int> date;
    private string dayText;
    private string monthText;

    public string UpdateDate()
    {
        if(date[0] <= 9)
        {
            dayText = "0" + date[0].ToString();
        }
        else
        {
            dayText = date[0].ToString();
        }

        if(date[1] <= 9)
        {
            monthText = "0" + date[1].ToString();
        }
        else
        {
            monthText = date[1].ToString();
        }

        return dayText + "-" + monthText + "-" + date[2].ToString();
    }

    public void NextDay()
    {
        if(date[0] < 30)
        {
            date[0] += 1;
        }
        else
        {
            date[0] = 1;
            if(date[1] < 12)
            {
                date[1] += 1;
            }
            else
            {
                date[1] = 1;
                date[2] += 1;
            }

        }
        GameManager.UM.UpdateUI();
    }
    public void LoadData(GameData data)
    {
        for (int i = 0; i < date.Count; i++)
        {
            data.date.Add(date[i]);
        }
    }

    public void SaveData(ref GameData data)
    {
        for (int i = 0; i < data.date.Count; i++)
        {
            date.Add(data.date[i]);
        }
    }
}
