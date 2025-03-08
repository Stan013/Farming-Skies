using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OpenWindowButton : MonoBehaviour
{
    public Button openButton;
    public GameObject openWindow;

    public void ChangeMode(string mode)
    {
        if (!openWindow.transform.gameObject.activeSelf)
        {
            openWindow.SetActive(true);
            GameManager.HM.HideCardsInHand(true);
            switch (mode)
            {
                case "Inventory":
                    GameManager.IPM.ToggleState(GameManager.GameState.InventoryMode, GameManager.GameState.Default);
                    break;
                case "Crafting":
                    GameManager.IPM.ToggleState(GameManager.GameState.CraftMode, GameManager.GameState.Default);
                    break;
                case "Market":
                    GameManager.IPM.ToggleState(GameManager.GameState.MarketMode, GameManager.GameState.Default);
                    break;
            }
            EventSystem.current.SetSelectedGameObject(null);
        }
        else 
        {
            GameManager.UM.closeButton.ChangeModeToDefault();
        }
    }
}