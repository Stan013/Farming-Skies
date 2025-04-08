using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour, IDataPersistence
{
    [Header("Cards list")]
    public List<Card> cardsInHand = new List<Card>();

    [Header("Handslot variables")]
    public GameObject handSlotParent;
    public CardSlot cardSlotPrefab;
    public List<CardSlot> handSlots;
    public int lastFilledSlotIndex;

    [Header("Drag variables")]
    public Card dragCard;
    public bool dragging;
    public string placement;

    [Header("Refill variables")]
    private float cardMoveDuration = 0.125f;

    public void SetHandSlots()
    {
        for (int i = 0; i < 7; i++)
        {
            CardSlot cardSlot = Instantiate(cardSlotPrefab, Vector3.zero, Quaternion.identity, handSlotParent.transform);
            cardSlot.transform.localPosition = new Vector3(-525 + (i*175), -525, 0);
            cardSlot.transform.localRotation = Quaternion.identity;
            handSlots.Add(cardSlot);
        }
    }

    public void SetCardsInHand()
    {
        if(GameManager.DM.cardsInDeck.Count != 0)
        {
            while (lastFilledSlotIndex < 7 && GameManager.DM.cardsInDeck.Count > 0)
            {
                Card newCard = GameManager.DM.FindCardInDeckByID(GameManager.DM.cardsInDeck[Random.Range(0, GameManager.DM.cardsInDeck.Count)].cardId);
                AddCardToHand(newCard);
            }
        }
    }

    public void AddCardToHand(Card card)
    {
        handSlots[lastFilledSlotIndex].AddCardToSlot(lastFilledSlotIndex, card);
        lastFilledSlotIndex++;
        card.SetCardState(Card.CardState.InHand);
        card.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        GameManager.DM.Deck = GameManager.DM.cardsInDeck.Count;
        StartCoroutine(MoveCardsFromBottom(card));
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
            card.SetCardState(Card.CardState.InDeck);
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
        card.transform.localScale = new Vector3(0.5f,0.5f,1f);
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
        if(handSlotParent.transform.childCount != 0)
        {
            foreach (Transform childSlot in handSlotParent.transform)
            {
                Destroy(childSlot.gameObject);
            }
            handSlots.Clear();
        }
        SetHandSlots();
        lastFilledSlotIndex = 0;
        cardsInHand.Clear();
        for (int i = 0; i < data.cardsInHand.Count; i++)
        {
            Card newCard = Instantiate(GameManager.CM.FindCardByID(data.cardsInHand[i]), Vector3.zero, Quaternion.identity);
            GameManager.CM.InitializeCard(newCard);
            GameManager.HM.AddCardToHand(newCard);
            GameManager.HM.cardsInHand.Add(newCard);
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
