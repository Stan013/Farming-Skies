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
        EventSystem.current.SetSelectedGameObject(null);
    }
}
