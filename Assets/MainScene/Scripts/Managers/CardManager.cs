using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour, IDataPersistence
{
    public List<Card> starterCards = new List<Card>();
    public List<Card> availableCards = new List<Card>();
    public List<Card> inspectedCards = new List<Card>();
    public Card inspectCard;

    public void CheckCardInspectTutorial(Card card)
    {
        if(GameManager.TTM.tutorialCount == 6)
        {
            if (!card.hasBeenInspected)
            {
                card.hasBeenInspected = true;
                inspectedCards.Add(card);
                card.cardBackground.GetComponent<Image>().color = new Color(0.735f, 0.735f, 0.735f);
            }
            if (inspectedCards.Count == 4)
            {
                GameManager.TTM.QuestCompleted = true;
            }
        }
    }

    public void SetupMarketItems()
    {
        foreach (Card card in availableCards)
        {
            if (card.cardType != "Utility" && card.cardUnlocked && !card.cardAddedToMarket)
            {
                GameManager.MM.UnlockMarketItem(card);
            }
        }
    }

    public void SetupCard(Card card)
    {
        card.cardImage.GetComponent<Image>().sprite = card.cardSprite;
        card.cardNameText.SetText(card.cardName);
        if (card.cardType == "PlantSmall" || card.cardType == "PlantMedium" || card.cardType == "PlantBig")
        {
            card.nitrogenText.SetText(card.nitrogen.ToString() + " L");
            card.phosphorusText.SetText(card.phosphorus.ToString() + " L");
            card.potassiumText.SetText(card.potassium.ToString() + " L");
            card.waterText.SetText(card.water.ToString() + " L");
            card.plantSizeText.SetText(card.cardType.Replace("Plant", ""));
        }
        else
        {
            card.nitrogenText.transform.parent.gameObject.SetActive(false);
            card.phosphorusText.transform.parent.gameObject.SetActive(false);
            card.potassiumText.transform.parent.gameObject.SetActive(false);
            card.waterText.transform.parent.gameObject.SetActive(false);
            card.plantSizeText.transform.parent.gameObject.SetActive(false);
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
