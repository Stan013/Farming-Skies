using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OpenQuestButton : MonoBehaviour
{
    public bool questActive = false;
    public TMP_Text openQuestText;

    public void OpenQuestMenu()
    {
        if (!questActive)
        {
            openQuestText.SetText("v");
            openQuestText.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, 180f);
            GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x, 270);
            GameManager.QM.questMenu.SetActive(true);
            questActive = true;
        }
        else
        {
            openQuestText.SetText("v");
            GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x, 510);
            GameManager.QM.questMenu.SetActive(false);
            questActive = false;
        }
        if (GetComponent<Image>().color == Color.green)
        {
            GetComponent<Image>().color = Color.white;
        }
    }
}
