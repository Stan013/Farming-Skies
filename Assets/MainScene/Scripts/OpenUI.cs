using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OpenUI : MonoBehaviour
{
    [SerializeField] private GameObject UIMenu;
    [SerializeField] private Button OpenUIButton;
    [SerializeField] private TMP_Text OpenUIText;
    private bool UIactive = false;

    public void OpenUIMenu()
    {
        if(!UIactive)
        {
            OpenUIText.SetText("<");
            OpenUIButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-500, OpenUIButton.GetComponent<RectTransform>().anchoredPosition.y);
            UIMenu.SetActive(true);
            UIactive = true;
        }
        else
        {
            OpenUIText.SetText(">");
            OpenUIButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-925, OpenUIButton.GetComponent<RectTransform>().anchoredPosition.y);
            UIMenu.SetActive(false);
            UIactive = false;
        }
    }
}
