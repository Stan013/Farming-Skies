using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public List<int> date;
    public string dayText;
    public string monthText;
    public List<string> cardDates;
    public int cardDateIndex;
    public List<string> taxDates;
    public int taxDateIndex;
    public List<string> refillDates;
    public int refillDateIndex;
    public int[] daysInMonth = { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
    public TMP_Text dateText;
    public int daysToAdvance;
    public GameObject timeWindow;

    public string GetDate()
    {
        if (date[0] <= 9) dayText = "0" + date[0].ToString();
        else dayText = date[0].ToString();
        if (date[1] <= 9) monthText = "0" + date[1].ToString();
        else monthText = date[1].ToString();
        return dayText + "-" + monthText + "-" + date[2].ToString();
    }

    public void StartWeekCycle()
    {
        StartCoroutine(CycleDays());
    }

    private IEnumerator CycleDays()
    {
        for (int i = 0; i < daysToAdvance; i++)
        {
            date[0]++;
            if (date[0] > daysInMonth[date[1]])
            {
                date[0] = 1;
                date[1]++;
                if (date[1] > 12)
                {
                    date[1] = 1;
                    date[2]++;
                }
            }
            dateText.text = GetDate();
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void RotateSky(float skyRotationSpeed)
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * skyRotationSpeed);
    }

    public string CheckDate()
    {
        if (cardDates.Exists(date => date == GetDate()))
        {
            string cardDate = cardDates[cardDateIndex];
            cardDateIndex++;
            return cardDate;
        }
        else
        {
            if (taxDates.Exists(date => date == GetDate()))
            {
                string taxDate = taxDates[taxDateIndex];
                taxDateIndex++;
                return taxDate;
            }
            else
            {
                if (refillDates.Exists(date => date == GetDate()))
                {
                    string refillDate = refillDates[refillDateIndex];
                    refillDateIndex++;
                    return refillDate;
                }
                else
                {
                    return cardDates[0];
                }
            }
        }
    }

/*    public void LoadData(GameData data)
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
    }*/
}
