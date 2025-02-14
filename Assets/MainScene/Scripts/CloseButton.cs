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
        closeWindow.SetActive(false);
        GameManager.UM.openQuestButton.transform.gameObject.SetActive(true);
        GameManager.UM.openUIButton.transform.gameObject.SetActive(true);
        GameManager.HM.HideCardsInHand(false);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
