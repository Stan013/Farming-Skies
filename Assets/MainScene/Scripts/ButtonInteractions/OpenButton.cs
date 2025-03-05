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
        if (!openWindow.transform.gameObject.activeSelf)
        {
            openWindow.SetActive(true);
            GameManager.HM.HideCardsInHand(true);
            switch (openWindow.name)
            {
                case "InventoryWindow":
                    GameManager.IPM.ToggleState(GameManager.GameState.InventoryMode, GameManager.GameState.Default);
                    if (GameManager.TTM.tutorialCount == 9)
                    {
                        //GameManager.TTM.QuestCompleted = true;
                    }
                    break;
                case "CraftingWindow":
                    GameManager.IPM.ToggleState(GameManager.GameState.CraftMode, GameManager.GameState.Default);
                    if (GameManager.TTM.tutorialCount == 12)
                    {
                        //GameManager.TTM.QuestCompleted = true;
                    }
                    break;
                case "MarketWindow":
                    GameManager.IPM.ToggleState(GameManager.GameState.MarketMode, GameManager.GameState.Default);
                    if (GameManager.TTM.tutorialCount == 17)
                    {
                        //GameManager.TTM.QuestCompleted = true;
                    }
                    break;
            }
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            GameManager.UM.closeButton.GetComponent<CloseButton>().OnKeyboardButtonClick(openWindow);
        }
    }
}