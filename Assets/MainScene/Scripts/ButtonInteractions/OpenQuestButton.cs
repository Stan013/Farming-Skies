using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OpenQuestButton : MonoBehaviour
{
    public bool questActive = false;
    public Image questButtonImage;
    public Sprite upIcon;
    public Sprite downIcon;

    public void OpenQuestMenu()
    {
        if (!questActive)
        {
            questButtonImage.sprite = downIcon;
            GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x, 235);
            GameManager.QM.questMenu.SetActive(true);
            questActive = true;
        }
        else
        {
            questButtonImage.sprite = upIcon;
            GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x, 505);
            GameManager.QM.questMenu.SetActive(false);
            questActive = false;
        }
        if (GetComponent<Image>().color == Color.green)
        {
            GetComponent<Image>().color = Color.white;
        }
    }
}
