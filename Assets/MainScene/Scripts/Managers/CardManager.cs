using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour, IDataPersistence
{
    public Transform availableCardsParent;
    public List<Card> starterCards = new List<Card>();
    public List<Card> availableCards = new List<Card>();
    public List<Card> inspectedCards = new List<Card>();
    public Card inspectCard;

    public void SetupCards()
    {
        foreach(Transform cardCategory in availableCardsParent)
        {
            foreach(Card childCard in cardCategory.GetComponentsInChildren<Card>())
            {
                if(childCard.cardType == "Utility" && childCard.cardUnlocked)
                {
                    GameManager.CRM.craftableCards.Add(childCard);
                }
                if(childCard.cardStarter)
                {
                    GameManager.CM.starterCards.Add(childCard);
                }
                availableCards.Add(childCard);
            }
        }
    }

    public void CheckCardInspectTutorial(Card card)
    {
        if(GameManager.TTM.tutorialCount == 5)
        {
            if (!card.hasBeenInspected)
            {
                card.hasBeenInspected = true;
                inspectedCards.Add(card);
                card.GetComponent<Image>().color = new Color(0.735f, 0.735f, 0.735f);
            }
            if (inspectedCards.Count == 4)
            {
                GameManager.TTM.QuestCompleted = true;
            }
        }
    }

    public void InitializeCard(Card card)
    {
        card.cardImage.GetComponent<Image>().sprite = card.cardSprite;
        card.cardNameText.SetText(card.cardName);
        if (card.cardType == "PlantSmall" || card.cardType == "PlantMedium" || card.cardType == "PlantBig")
        {
            CardDrag cardDrag = card.GetComponent<CardDrag>();
            Plant dragPlant = cardDrag.dragModel.GetComponent<Plant>();
            card.cardDescriptionText.gameObject.SetActive(false);
            card.nitrogenText.SetText(dragPlant.nitrogen.ToString() + " L");
            card.phosphorusText.SetText(dragPlant.phosphorus.ToString() + " L");
            card.potassiumText.SetText(dragPlant.potassium.ToString() + " L");
            card.waterText.SetText(dragPlant.water.ToString() + " L");
            card.plantSizeText.SetText(card.cardType.Replace("Plant", ""));

            if(GameManager.TTM.tutorial && GameManager.TTM.tutorialCount > 6)
            {
                card.GetComponent<CardDrag>().enabled = true;
            }
            else
            {
                card.GetComponent<CardDrag>().enabled = false;
            }
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
            card.GetComponent<CardDrag>().enabled = true;
        }
        card.cardSetup = true;
    }

    public Card FindCardById(string id)
    {
        return availableCards.Find(card => card.cardId == id);
    }

    public void LoadData(GameData data)
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
    }
}
