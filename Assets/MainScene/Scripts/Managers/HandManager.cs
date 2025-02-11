using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour, IDataPersistence
{
    public List<Card> cardsInHand = new List<Card>();
    public GameObject handSlotParent;
    public CardSlot cardSlotPrefab;
    public List<CardSlot> handSlots;
    public Card dragCard;
    public bool dragging;
    public string placement;
    private int offsetX = 175;
    private float cardMoveDurationBottom = 0.25f;
    private float cardMoveDurationBoth = 0.125f;
    public bool needsCard = true;

    public void SetStartingHand()
    {
        foreach(Card card in GameManager.CM.starterCards)
        {
            GameManager.DM.AddCardToDeck(card.cardId);
        }
        SetCardsInHand();
        needsCard = false;
    }

    public void SetCardsInHand()
    {
        if(GameManager.DM.cardsInDeck.Count != 0)
        {
            while (handSlots.Count < 9 && GameManager.DM.cardsInDeck.Count > 0)
            {
                CardSlot newSlot = Instantiate(cardSlotPrefab, new Vector3(-700 + (handSlots.Count * offsetX), -525, 0), Quaternion.identity);
                newSlot.transform.SetParent(handSlotParent.transform, false);
                handSlots.Add(newSlot);
                Card originalCard = GameManager.DM.cardsInDeck[Random.Range(0, GameManager.DM.cardsInDeck.Count)];
                newSlot.AddCardToSlot(handSlots.Count, originalCard);
                originalCard.ToggleState(Card.CardState.InHand, Card.CardState.InDeck);
                StartCoroutine(MoveCardsFromBottom(originalCard));
            }
            needsCard = false;
        }
        else
        {
            return;
        }
    }

    public void AddCardToHand(string cardId)
    {
        Card newCard = Instantiate(GameManager.CM.FindCardById(cardId), Vector3.zero, Quaternion.identity);
        CardSlot newSlot = Instantiate(cardSlotPrefab, new Vector3(-700 + (handSlots.Count * offsetX), -550, 0f), Quaternion.identity);
        newSlot.transform.SetParent(handSlotParent.transform, false);
        handSlots.Add(newSlot);
        newSlot.AddCardToSlot(handSlots.Count, newCard);
        newCard.ToggleState(Card.CardState.InHand, Card.CardState.InDeck);
        StartCoroutine(MoveCardsFromBottom(newCard));
    }

    private IEnumerator MoveCardsFromBottom(Card card)
    {
        RectTransform cardRectTransform = card.GetComponent<RectTransform>();
        Vector2 startPosition = new Vector2(cardRectTransform.anchoredPosition.x, -Screen.height);
        Vector2 finalPosition = cardRectTransform.anchoredPosition;
        float elapsedTime = 0;
        while (elapsedTime < cardMoveDurationBottom)
        {
            cardRectTransform.anchoredPosition = Vector2.Lerp(startPosition, finalPosition, elapsedTime / cardMoveDurationBottom);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cardRectTransform.anchoredPosition = finalPosition;
    }

    public void ClearCardsInHand()
    {
        foreach (Card card in cardsInHand)
        {
            card.ToggleState(Card.CardState.InDeck, Card.CardState.Destroy);
            RemoveEmptyCardSlot();
        }
    }

    public void RemoveEmptyCardSlot()
    {
        CardSlot emptySlot = handSlots[cardsInHand.Count];
        handSlots.Remove(emptySlot);
        Destroy(emptySlot.gameObject);
    }

    public Card FindCardInHandById(string id)
    {
        return cardsInHand.Find(card => card.cardId == id);
    }

    public void MoveCardsInHand(Card usedCard)
    {
        foreach (Card card in cardsInHand)
        {
            if (card.cardIndex > usedCard.cardIndex)
            {
                card.cardIndex--;
                Transform originalParent = card.transform.parent;
                card.transform.SetParent(null);
                RectTransform cardRectTransform = card.GetComponent<RectTransform>();
                Vector3 offScreenPosition = cardRectTransform.position - new Vector3(0, 100, 0);
                Vector3 newPosition = handSlots[card.cardIndex-1].transform.position;
                StartCoroutine(MoveCardCoroutine(cardRectTransform, offScreenPosition, newPosition, () =>
                {
                    card.transform.SetParent(handSlots[card.cardIndex-1].transform);
                    cardRectTransform.anchoredPosition = Vector2.zero;
                }));
            }
        }
    }

    private IEnumerator MoveCardCoroutine(RectTransform cardRectTransform, Vector3 offScreenPosition, Vector3 newPosition, System.Action onComplete = null)
    {
        Vector3 startPosition = cardRectTransform.position;
        float elapsedTime = 0f;
        while (elapsedTime < cardMoveDurationBoth)
        {
            cardRectTransform.anchoredPosition = Vector2.Lerp(startPosition, offScreenPosition, elapsedTime / cardMoveDurationBoth);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cardRectTransform.position = offScreenPosition;
        elapsedTime = 0f;
        startPosition = offScreenPosition;
        while (elapsedTime < cardMoveDurationBoth)
        {
            cardRectTransform.anchoredPosition = Vector2.Lerp(startPosition, newPosition, elapsedTime / cardMoveDurationBoth);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cardRectTransform.position = newPosition;
        onComplete?.Invoke();
    }

    public void HideCardsInHand()
    {
        foreach(Card card in cardsInHand)
        {
            card.transform.gameObject.SetActive(true);
        }
    }

    public void LoadData(GameData data)
    {
        cardsInHand.Clear();
        foreach (string cardID in data.cardsInHand)
        {
            AddCardToHand(cardID);
        }
    }

    public void SaveData(ref GameData data)
    {
        data.cardsInHand.Clear();
        foreach (Card card in cardsInHand)
        {
            data.cardsInHand.Add(card.cardId);
        }
    }
}
