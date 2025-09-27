using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour, IDataPersistence
{
    [Header("Cards lists")]
    public List<Card> allCards;
    public List<Card> starterCards;
    public List<Card> unlockedCards;
    public List<Card> inspectedCards;
    public List<string> cardTypes;

    [Header("Cards variables")]
    public Transform availableCardsParent;
    public Card inspectCard;
    public Sprite waterIcon;
    public Sprite fertiliserIcon;

    public void SetupCards()
    {
        foreach(Transform cardCategory in availableCardsParent)
        {
            foreach(Card childCard in cardCategory.GetComponentsInChildren<Card>())
            {
                if(childCard.cardUnlocked)
                {
                    unlockedCards.Add(childCard);
                    GameManager.CRM.craftableCards.Add(childCard);
                    GameManager.CRM.UnlockCraftItem(childCard);
                }
                if(childCard.cardStarter)
                {
                    GameManager.CM.starterCards.Add(childCard);
                }
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
        else if (card.cardType == "Structures")
        {
            card.cardDescriptionText.gameObject.SetActive(true);
            card.cardDescriptionText.SetText(card.cardDescription);
            card.nitrogenText.transform.parent.gameObject.SetActive(false);
            card.phosphorusText.transform.parent.gameObject.SetActive(false);
            card.potassiumText.transform.parent.gameObject.SetActive(false);
            card.waterText.transform.parent.gameObject.SetActive(false);
            card.resourceAdditionText.transform.parent.gameObject.SetActive(true);
            card.resourceAdditionText.SetText("100₴");
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
        }
        if (!GameManager.QM.questActive)
        {
            card.GetComponent<CardDrag>().enabled = true;
            card.GetComponent<CardInspect>().enabled = true;
        }
        card.cardSetup = true;
    }   

    public Card FindCardByID(string id)
    {
        return allCards.Find(card => card.cardId == id);
    }

    public void LoadData(GameData data)
    {
        unlockedCards.Clear();
        foreach (string cardID in data.unlockedCardsID)
        {
            FindCardByID(cardID).cardUnlocked = true;
        }

        inspectedCards.Clear();
        foreach (string cardID in data.inspectedCardsID)
        {
            inspectedCards.Add(FindCardByID(cardID));
        }
        SetupCards();
    }

    public void SaveData(ref GameData data)
    {
        data.unlockedCardsID.Clear();
        foreach (Card card in unlockedCards)
        {
            data.unlockedCardsID.Add(card.cardId);
        }

        data.inspectedCardsID.Clear();
        foreach (Card card in inspectedCards)
        {
            data.inspectedCardsID.Add(card.cardId);
        }
    }
}
