using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OpenButton : MonoBehaviour
{
    [SerializeField] private Button openButton;
    [SerializeField] private GameObject openWindow;
    void Start()
    {
        openButton.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        switch(openButton.name)
        {
            case "InventoryButton":
                if(GameManager.TTM.tutorialCount == 9)
                {
                    GameManager.TTM.QuestCompleted = true;
                    GameManager.UM.openInventoryButton.GetComponent<Image>().color = Color.white;
                }
                GameManager.IPM.ToggleState(GameManager.GameState.InventoryMode, GameManager.GameState.Default);
                openWindow.SetActive(true);
                break;
            case "MarketButton":
                GameManager.IPM.ToggleState(GameManager.GameState.MarketMode, GameManager.GameState.Default);
                openWindow.SetActive(true);
                break;
            case "SettingsButton":
                GameManager.IPM.ToggleState(GameManager.GameState.SettingsMode, GameManager.GameState.Default);
                openWindow.SetActive(true);
                break;
        }
        EventSystem.current.SetSelectedGameObject(null);
    }
}
