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
        if(!openWindow.transform.gameObject.activeSelf)
        {
            openWindow.SetActive(true);
            if (openButton.name != "StartButton")
            {
                GameManager.UM.OpenQuestMenu();
                GameManager.UM.OpenUIMenu();
                GameManager.HM.HideCardsInHand(true);
                if (openWindow.name == "InventoryWindow" && GameManager.TTM.tutorialCount == 9)
                {
                    GameManager.TTM.QuestCompleted = true;
                    GameManager.UM.openInventoryButton.GetComponent<Image>().color = Color.white;
                }
                if (openWindow.name == "CraftWindow" && GameManager.TTM.tutorialCount == 12)
                {
                    GameManager.TTM.QuestCompleted = true;
                    GameManager.UM.openCraftButton.GetComponent<Image>().color = Color.white;
                }
            }
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            GameManager.UM.closeButton.onClick.Invoke();
        }
    }
}
