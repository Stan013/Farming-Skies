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
            GameManager.HM.HideCardsInHand(false);
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void OnKeyboardButtonClick(GameObject windowMode)
    {
        closeWindow = windowMode;
        closeWindow.SetActive(false);
        GameManager.HM.HideCardsInHand(false);
        GameManager.IPM.ToggleState(GameManager.GameState.ManageMode, GameManager.GameState.Default);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
