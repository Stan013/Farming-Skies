using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour, IDataPersistence
{
    public List<Card> cardsInHand = new List<Card>();
    public GameObject handSlotParent;
    public CardSlot cardSlotPrefab;
    public List<CardSlot> handSlots;
    public int lastFilledSlotIndex;

    public Card dragCard;
    public bool dragging;
    public string placement;
    private float cardMoveDuration = 0.125f;
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
            while (lastFilledSlotIndex < 9 && GameManager.DM.cardsInDeck.Count > 0)
            {
                Card originalCard = GameManager.DM.cardsInDeck[Random.Range(0, GameManager.DM.cardsInDeck.Count)];
                handSlots[lastFilledSlotIndex].AddCardToSlot(lastFilledSlotIndex, originalCard);
                lastFilledSlotIndex++;
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
        handSlots[lastFilledSlotIndex].AddCardToSlot(lastFilledSlotIndex, newCard);
        lastFilledSlotIndex++;
        newCard.ToggleState(Card.CardState.InHand, Card.CardState.InDeck);
        StartCoroutine(MoveCardsFromBottom(newCard));
    }

    private IEnumerator MoveCardsFromBottom(Card card)
    {
        RectTransform cardRectTransform = card.GetComponent<RectTransform>();
        Vector2 startPosition = new Vector2(cardRectTransform.anchoredPosition.x, -Screen.height);
        Vector2 finalPosition = cardRectTransform.anchoredPosition;
        float elapsedTime = 0;
        while (elapsedTime < cardMoveDuration)
        {
            cardRectTransform.anchoredPosition = Vector2.Lerp(startPosition, finalPosition, elapsedTime / cardMoveDuration);
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
        }
    }

    public Card FindCardInHandById(string id)
    {
        return cardsInHand.Find(card => card.cardId == id);
    }

    public void MoveCardsInHand(Card usedCard)
    {
        foreach (Card card in cardsInHand)
        {
            if (card.transform.parent.GetComponent<CardSlot>().index > usedCard.transform.parent.GetComponent<CardSlot>().index)
            {
                int originalParentIndex = card.transform.parent.GetComponent<CardSlot>().index;
                Vector3 offScreenPosition = new Vector3(0, 100, 0);
                StartCoroutine(MoveCardCoroutine(card, offScreenPosition, originalParentIndex));
            }
        }
    }

    private IEnumerator MoveCardCoroutine(Card card, Vector3 offScreenPosition, int originalParentIndex)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = card.transform.position;
        while (elapsedTime < cardMoveDuration * 2)
        {
            card.transform.position = Vector3.Lerp(startPosition, offScreenPosition, elapsedTime / cardMoveDuration * 2);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        card.transform.SetParent(null);
        card.transform.SetParent(handSlots[originalParentIndex-1].transform, false);
        elapsedTime = 0f;
        startPosition = card.transform.position;
        Vector3 finalPosition = startPosition + new Vector3(0, 100, 0);
        while (elapsedTime < cardMoveDuration * 2)
        {
            card.transform.position = Vector3.Lerp(startPosition, finalPosition, elapsedTime / cardMoveDuration * 2);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        card.transform.localPosition = Vector3.zero;
        card.transform.localRotation = Quaternion.identity;
        card.transform.localScale = Vector3.one;
    }


    public void HideCardsInHand(bool hidden)
    {
        foreach(Card card in cardsInHand)
        {
            if(hidden)
            {
                card.transform.gameObject.SetActive(false);
            }
            else
            {
                card.transform.gameObject.SetActive(true);
            }
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
