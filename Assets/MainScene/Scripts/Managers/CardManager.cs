using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour, IDataPersistence
{
    public List<Card> starterCards = new List<Card>();
    public List<Card> availableCards = new List<Card>();
    public Card inspectCard;

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
