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
            GameManager.HM.HideCardsInHand(true);
            switch (openWindow.name)
            {
                case "InventoryWindow":
                    GameManager.IPM.ToggleState(GameManager.GameState.InventoryMode, GameManager.GameState.Default);
                    if (GameManager.TTM.tutorialCount == 9)
                    {
                        GameManager.TTM.QuestCompleted = true;
                        GameManager.UM.openInventoryButton.GetComponent<Image>().color = Color.white;
                    }
                    break;
                case "CraftingWindow":
                    GameManager.IPM.ToggleState(GameManager.GameState.CraftMode, GameManager.GameState.Default);
                    if (GameManager.TTM.tutorialCount == 12)
                    {
                        GameManager.TTM.QuestCompleted = true;
                        GameManager.UM.openCraftButton.GetComponent<Image>().color = Color.white;
                    }
                    break;
                case "MarketWindow":
                    GameManager.IPM.ToggleState(GameManager.GameState.MarketMode, GameManager.GameState.Default);
                    if (GameManager.TTM.tutorialCount == 17)
                    {
                        GameManager.TTM.QuestCompleted = true;
                        GameManager.UM.openMarketButton.GetComponent<Image>().color = Color.white;
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

    public void OnKeyboardButtonClick(string mode)
    {
        if (!openWindow.transform.gameObject.activeSelf && GameManager.CurrentState != GameManager.GameState.Default)
        {
            openWindow.SetActive(true);
            GameManager.HM.HideCardsInHand(true);
            switch (mode)
            {
                case "Inventory":
                    if (GameManager.TTM.tutorialCount == 9)
                    {
                        GameManager.TTM.QuestCompleted = true;
                        GameManager.UM.openInventoryButton.GetComponent<Image>().color = Color.white;
                    }
                    break;
                case "Craft":
                    if (GameManager.TTM.tutorialCount == 12)
                    {
                        GameManager.TTM.QuestCompleted = true;
                        GameManager.UM.openCraftButton.GetComponent<Image>().color = Color.white;
                    }
                    break;
                case "Market":
                    if (GameManager.TTM.tutorialCount == 22)
                    {
                        GameManager.TTM.QuestCompleted = true;
                        GameManager.UM.openMarketButton.GetComponent<Image>().color = Color.white;
                    }
                    break;
            }
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            GameManager.UM.closeButton.onClick.Invoke();
        }
    }
}
