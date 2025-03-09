using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OpenWindowButton : MonoBehaviour
{
    public void ChangeMode(GameObject openWindow)
    {
        openWindow.SetActive(true);
        GameManager.HM.HideCardsInHand(true);
        EventSystem.current.SetSelectedGameObject(null);
    }
}