using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugManager : MonoBehaviour
{
    public bool skipTutorial;
    public TMP_InputField cardIdInput;
    public TMP_InputField cropIdInput;

    public void DebugInput()
    {
        if (Input.GetKeyUp(KeyCode.F1))
        {
            AddMoney();
        }
        if (Input.GetKeyUp(KeyCode.F2))
        {
            AddWater();
        }
        if (Input.GetKeyUp(KeyCode.F3))
        {
            AddFertiliser();
        }
        if (Input.GetKeyUp(KeyCode.Keypad1))
        {
            if(cardIdInput.gameObject.activeSelf)
            {
                cardIdInput.gameObject.SetActive(false);
            }
            else
            {
                cardIdInput.gameObject.SetActive(true);
            }

            if (cropIdInput.gameObject.activeSelf)
            {
                cropIdInput.gameObject.SetActive(false);
            }
            else
            {
                cropIdInput.gameObject.SetActive(true);
            }
        }
    }

    public void AddMoney()
    {
        GameManager.UM.Balance += 500;
    }

    public void AddWater()
    {
        GameManager.UM.Water += 500;
    }

    public void AddFertiliser()
    {
        GameManager.UM.Fertiliser += 500;
    }

    public void AddCrop()
    {
        string cardID = cropIdInput.text;
        if (!string.IsNullOrEmpty(cardID))
        {
            GameManager.INM.FindInventoryItemByID(cardID).ItemQuantity += 5;
        }
    }

    public void AddCard()
    {
        string cropID = cardIdInput.text;
        if (!string.IsNullOrEmpty(cropID))
        {
            GameManager.DM.AddCardToDeck(GameManager.CM.FindCardByID(cropID).cardId);
        }
    }
}
