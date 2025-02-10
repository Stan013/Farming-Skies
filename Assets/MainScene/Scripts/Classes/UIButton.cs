using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    public bool hasPressed;

    public void SetPressed()
    {
        if(!hasPressed)
        {
            hasPressed = true;
            GetComponent<Image>().color = Color.white;
            if (GameManager.UM.taxAmountText.transform.parent.GetComponent<Image>().color == Color.white && GameManager.UM.dateAmountText.transform.parent.GetComponent<Image>().color == Color.white & GameManager.TTM.tutorialCount == 2)
            {
                GameManager.TTM.QuestCompleted = true;
            }
        }
    }
}
