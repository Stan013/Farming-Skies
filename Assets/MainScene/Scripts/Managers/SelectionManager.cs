using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private GameObject pickSlotParent;
    public List<SelectionPicker> selectionPickers = new List<SelectionPicker>();
    public List<CardSlot> pickSlots;

    public List<Card> cropCards = new List<Card>();
    public List<Card> structureCards = new List<Card>();
    public List<Card> utilityCards = new List<Card>();

    public void SetupSelection()
    {
        foreach (SelectionPicker picker in selectionPickers)
        {
            picker.SetupPicker(picker);
        }

        cropCards.Clear();
        structureCards.Clear();
        utilityCards.Clear();

        foreach (Card unlockedCard in GameManager.CM.unlockedCards)
        {
            switch (unlockedCard.cardType)
            {
                case "Small crops":
                    cropCards.Add(unlockedCard);
                    break;
                case "Medium crops":
                    cropCards.Add(unlockedCard);
                    break;
                case "Big crops":
                    cropCards.Add(unlockedCard);
                    break;
                case "Utilities":
                    utilityCards.Add(unlockedCard);
                    break;
                case "Structure":
                    structureCards.Add(unlockedCard);
                    break;
            }
        }
    }
}
