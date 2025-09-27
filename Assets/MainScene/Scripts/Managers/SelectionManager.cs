using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private GameObject pickSlotParent;
    public List<SelectionPicker> selectionPickers = new List<SelectionPicker>();
    public List<CardSlot> pickSlots;

    public void SetupSelectionPickers()
    {
        foreach (SelectionPicker picker in selectionPickers)
        {
            picker.SetupPicker(picker);
        }
    }
    //public void SetupCardSelection()
    //{
    //    for (int i = 0; i < 3; i++)
    //    {
    //        CardSlot newSlot = Instantiate(cardSlotTemplate, new Vector3(-450f + (pickSlots.Count * 450f), -75f, 0f), Quaternion.identity, pickSlotParent.transform);
    //        newSlot.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    //        newSlot.transform.localPosition = new Vector3(-500 + (500 * i), 0, 0);
    //        pickSlots.Add(newSlot);

    //        foreach (Card card in GameManager.CM.unlockedCards)
    //        {

    //            Card randomCard = Instantiate(card, Vector3.zero, Quaternion.identity);
    //            randomCard.SetCardState(Card.CardState.InChoosing);
    //            newSlot.AddCardToSlot(pickSlots.Count, randomCard);
    //        }
    //    }
    //}
}
