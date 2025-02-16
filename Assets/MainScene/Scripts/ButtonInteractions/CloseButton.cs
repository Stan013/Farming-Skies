using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CloseButton : MonoBehaviour
{
    public Button closeButton;
    public GameObject closeWindow;

    public void Start()
    {
        closeButton.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        if (closeWindow.transform.gameObject.activeSelf)
        {
            closeWindow.SetActive(false);
            GameManager.UM.OpenQuestMenu();
            GameManager.UM.OpenUIMenu();
            GameManager.HM.HideCardsInHand(false);
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
