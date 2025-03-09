using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CloseWindowButton : MonoBehaviour
{
    public void ChangeModeToDefault(GameObject closeWindow)
    {
        closeWindow.SetActive(false);
        GameManager.HM.HideCardsInHand(false);
        GameManager.IPM.ToggleState(GameManager.GameState.Default, GameManager.GameState.ManageMode);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
