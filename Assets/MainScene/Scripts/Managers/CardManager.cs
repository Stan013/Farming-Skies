using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [Header("Cards lists")]
    public List<Card> starterCards = new List<Card>();
    public List<Card> availableCards = new List<Card>();
    public List<Card> inspectedCards = new List<Card>();

    [Header("Cards variables")]
    public Transform availableCardsParent;
    public Card inspectCard;

    public void SetupCards()
    {
        foreach(Transform cardCategory in availableCardsParent)
        {
            foreach(Card childCard in cardCategory.GetComponentsInChildren<Card>())
            {
                if(childCard.cardUnlocked)
                {
                    GameManager.CRM.craftableCards.Add(childCard);
                    GameManager.CRM.UnlockCraftItem(childCard);
                }
                if(childCard.cardStarter)
                {
                    GameManager.CM.starterCards.Add(childCard);
                }
                availableCards.Add(childCard);
            }
        }
    }

    public void InitializeCard(Card card)
    {
        card.cardImage.GetComponent<Image>().sprite = card.cardSprite;
        card.cardNameText.SetText(card.cardName);
        if (card.cardType == "Small crops" || card.cardType == "Medium crops" || card.cardType == "Large crops")
        {
            CardDrag cardDrag = card.GetComponent<CardDrag>();
            Plant dragPlant = cardDrag.dragModel.GetComponent<Plant>();
            card.cardDescriptionText.gameObject.SetActive(false);
            card.waterText.SetText(dragPlant.nutrientsUsages[0].ToString() + " L");
            card.nitrogenText.SetText(dragPlant.nutrientsUsages[1].ToString() + " L");
            card.phosphorusText.SetText(dragPlant.nutrientsUsages[2].ToString() + " L");
            card.potassiumText.SetText(dragPlant.nutrientsUsages[3].ToString() + " L");
            card.plantSizeText.SetText(card.cardType.Replace(" crops", ""));
        }
        else
        {
            card.cardDescriptionText.gameObject.SetActive(true);
            card.cardDescriptionText.SetText(card.cardDescription);
            card.nitrogenText.transform.parent.gameObject.SetActive(false);
            card.phosphorusText.transform.parent.gameObject.SetActive(false);
            card.potassiumText.transform.parent.gameObject.SetActive(false);
            card.waterText.transform.parent.gameObject.SetActive(false);
            card.plantSizeText.transform.parent.gameObject.SetActive(false);
            if (!GameManager.QM.questActive)
            {
                card.GetComponent<CardDrag>().enabled = true;
                card.GetComponent<CardInspect>().enabled = true;

            }
        }
        card.cardSetup = true;
    }   

    public Card FindCardByID(string id)
    {
        return availableCards.Find(card => card.cardId == id);
    }

/*    public void LoadData(GameData data)
    {
        starterCards.Clear();
        foreach (string cardID in data.starterCards)
        {
            starterCards.Add(FindCardById(cardID));
        }
    }

    public void SaveData(ref GameData data)
    {
        data.starterCards.Clear();
        foreach (Card card in starterCards)
        {
            data.starterCards.Add(card.cardId);
        }
    }*/
}
