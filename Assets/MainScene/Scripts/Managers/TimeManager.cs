using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public List<int> date;
    public string dayText;
    public string monthText;
    public List<string> cardDates;
    public int cardDateIndex = 1;
    public List<string> taxDates;
    public int taxDateIndex;

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

    public string CheckDate()
    {
        if(cardDates.Exists(UpdateDate() => UpdateDate()))
        {
            string cardDate = cardDates[cardDateIndex];
            cardDateIndex++;
            return cardDate;
        }
        else
        {
            if (taxDates.Exists(UpdateDate() => UpdateDate()))
            {
                string taxDate = taxDates[taxDateIndex];
                taxDateIndex++;
                return taxDate;
            }
            else
            {
                return null;
            }
        }
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
