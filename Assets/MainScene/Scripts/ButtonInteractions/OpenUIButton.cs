using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OpenUIButton : MonoBehaviour
{
    public bool UIActive = false;
    public Image UIButtonImage;
    public Sprite leftIcon;
    public Sprite rightIcon;
    
    public void OpenUIMenu()
    {
        if (!UIActive)
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(-480, GetComponent<RectTransform>().anchoredPosition.y);
            GameManager.UM.UIMenu.SetActive(true);
            UIActive = true;
        }
        else
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(-925, GetComponent<RectTransform>().anchoredPosition.y);
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
