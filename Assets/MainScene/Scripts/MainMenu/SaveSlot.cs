using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveSlot : MonoBehaviour
{
    public string profileId = "";
    public GameObject noDataContent;
    public GameObject hasDataContent;
    public TMP_Text saveNameText;
    public TMP_Text balanceText;

    public void SetData(string saveSlotName, GameData data)
    {
        if (data == null)
        {
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
        }
        else
        {
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);
            saveNameText.text = saveSlotName;
            balanceText.text = data.balance.ToString() + " ₴";
        }
    }

}
