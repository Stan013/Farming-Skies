using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
                GameManager.IPM.ToggleState(GameManager.GameState.InventoryMode, GameManager.GameState.Default);
                openWindow.SetActive(true);
                break;
            case "MarketButton":
                GameManager.IPM.ToggleState(GameManager.GameState.MarketMode, GameManager.GameState.Default);
                openWindow.SetActive(true);
                break;
            case "SettingsButton":
                GameManager.IPM.ToggleState(GameManager.GameState.MenuMode, GameManager.GameState.Default);
                openWindow.SetActive(true);
                break;
        }
    }
}
