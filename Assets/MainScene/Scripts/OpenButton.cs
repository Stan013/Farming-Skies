using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OpenButton : MonoBehaviour
{
    public Button openButton;
    public GameObject openWindow;

    public void Start()
    {
        openButton.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        openWindow.SetActive(true);
        GameManager.UM.openQuestButton.transform.gameObject.SetActive(false);
        GameManager.UM.openUIButton.transform.gameObject.SetActive(false);
        GameManager.HM.HideCardsInHand(true);
        EventSystem.current.SetSelectedGameObject(null);
        if(openWindow.name == "InventoryWindow" && GameManager.TTM.tutorialCount == 9)
        { 
            GameManager.TTM.QuestCompleted = true;
            GameManager.UM.openInventoryButton.GetComponent<Image>().color = Color.white;
        }
        if(openWindow.name == "CraftWindow" && GameManager.TTM.tutorialCount == 12)
        {
            GameManager.TTM.QuestCompleted = true;
            GameManager.UM.openCraftButton.GetComponent<Image>().color = Color.white;
        }
    }
}
