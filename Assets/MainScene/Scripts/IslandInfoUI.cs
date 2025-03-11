using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IslandInfoUI : MonoBehaviour
{
    public List<TMP_Text> nutrientsRequiredText;
    public List<TMP_Text> nutrientsAvailableText;
    public GameObject requiredInfo;
    public GameObject availableInfo;

    public void SwitchInfo()
    {
        if(requiredInfo.activeSelf)
        {
            requiredInfo.SetActive(false);
            availableInfo.SetActive(true);
        }
        else
        {
            requiredInfo.SetActive(true);
            availableInfo.SetActive(false);
        }
    }

    public void SetupIslandInfo(Island island)
    {
        for (int i = 0; i < island.nutrientsRequired.Count; i++)
        {
            nutrientsRequiredText[i].SetText(island.nutrientsRequired[i].ToString() + " L");
        }
        for (int i = 0; i <= island.nutrientsAvailable.Count; i++)
        {
            nutrientsAvailableText[i].SetText(island.nutrientsAvailable[i].ToString() + " L");
        }
    }
}
