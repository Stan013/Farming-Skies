using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OpenUIButton : MonoBehaviour
{
    public bool UIActive = false;
    public TMP_Text openUIText;
    
    public void OpenUIMenu()
    {
        if (!UIActive)
        {
            openUIText.SetText("v");
            openUIText.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, -90f);
            GetComponent<RectTransform>().anchoredPosition = new Vector2(-510, GetComponent<RectTransform>().anchoredPosition.y);
            GameManager.UM.UIMenu.SetActive(true);
            UIActive = true;
        }
        else
        {
            openUIText.SetText("v");
            openUIText.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, 90f);
            GetComponent<RectTransform>().anchoredPosition = new Vector2(-930, GetComponent<RectTransform>().anchoredPosition.y);
            GameManager.UM.UIMenu.SetActive(false);
            GameManager.UM.infoMenu.SetActive(false);
            UIActive = false;
        }
        if (GetComponent<Image>().color == Color.green)
        {
            GetComponent<Image>().color = Color.white;
        }
    }
}
