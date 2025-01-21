using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class Card : MonoBehaviour
{
    [Header("Invisible card properties")] 
    public string cardId;
    public bool cardSetup;
    public string cardType;
    public CardState CurrentState;
    public int cardIndex;
    public bool dragSucces;

    [Header("Visible card properties")]
    public string cardName;
    public TMP_Text cardNameText;

    [Header("Card in-hand properties")]
    public GameObject cardImage;
    public Sprite cardSprite;

    [Header("Card in-choosing properties")]
    public TMP_Text cardAmountText;
    public int cardAmount;
    public GameObject cardPickButton;
    public GameObject cardAnimation;
    public VideoClip cardClip;

    [Header("Associated item properties")]
    public string itemName;

    [Header("Associated inventory item properties")]
    public InventoryItem inventoryItem;
    public int itemQuantity;
    public bool cardUnlocked;

    [Header("Associated market item properties")]
    public MarketItem marketItem;
    public bool cardAddedToMarket;
    public float itemPrice;
    public float itemDemand;
    public float itemSupply;

    [Header("Associated plant properties")]
    public int plantTier;
    public int yield;
    public int nitrogen;
    public TMP_Text nitrogenText;
    public int phosphorus;
    public TMP_Text phosphorusText;
    public int potassium;
    public TMP_Text potassiumText;

    public enum CardState
    {
        InDeck,
        InHand,
        InChoosing,
        InDrag,
        Destroy,
        Hidden,
        Available,
        Inspect,
    }

    public void ToggleState(CardState targetState, CardState fallbackState)
    {
        SetState(CurrentState == targetState ? fallbackState : targetState);
    }

    public void SetState(CardState newState)
    {
        ExitState(CurrentState);
        CurrentState = newState;
        EnterState(CurrentState);
    }

    private void EnterState(CardState state)
    {
        switch (state)
        {
            case CardState.InDeck:
                GameManager.UM.UpdateUI();
                GameManager.DM.cardsInDeck.Add(this);
                this.gameObject.transform.SetParent(GameManager.DM.cardsInDeckParent.transform);
                cardIndex = GameManager.DM.cardsInDeck.Count;
                this.gameObject.SetActive(false);
                break;
            case CardState.InHand:
                if(GameManager.HM.needsCard)
                {
                    if (!cardSetup)
                    {
                        GameManager.CM.SetupCard(GetComponent<Card>());
                    }
                    GameManager.HM.cardsInHand.Add(this);
                    cardIndex = GameManager.HM.cardsInHand.Count;
                }
                this.gameObject.SetActive(true);
                break;
            case CardState.InChoosing:
                this.gameObject.SetActive(true);
                cardImage.SetActive(false);
                cardAnimation.SetActive(true);
                cardAnimation.GetComponent<VideoPlayer>().clip = cardClip;
                cardPickButton.SetActive(true);
                cardAmount = Random.Range(1, 3);
                cardAmountText.SetText($"X{cardAmount}");
                cardNameText.SetText(cardName);
                break;
            case CardState.InDrag:
                this.transform.GetChild(0).gameObject.SetActive(false);
                break;
            case CardState.Inspect:
                GetComponent<RectTransform>().anchoredPosition = new Vector2(transform.parent.GetComponent<RectTransform>().anchoredPosition.x*2*-1, 1250);
                transform.localScale = new Vector3(1.5f, 1.5f, 1f);
                break;
            case CardState.Destroy:
                Destroy(this.gameObject);
                break;
            case CardState.Hidden:
                this.gameObject.SetActive(false);
                break;
        }
        GameManager.UM.UpdateUI();
    }

    private void ExitState(CardState state)
    {
        switch (state)
        {
            case CardState.InDeck:
                GameManager.UM.UpdateUI();
                GameManager.DM.cardsInDeck.Remove(this);
                break;
            case CardState.InHand:
                if(GameManager.HM.needsCard)
                {
                    GameManager.HM.cardsInHand.Remove(this);
                }
                break;
            case CardState.InDrag:
                if(dragSucces)
                {
                    GameManager.HM.needsCard = true;
                    GameManager.HM.cardsInHand.Remove(this);
                    GameManager.HM.MoveCardsInHand(this);
                    GameManager.HM.SetCardsInHand();
                    dragSucces = false;
                }
                else
                {
                    this.transform.GetChild(0).gameObject.SetActive(true);
                }
                break;
            case CardState.Inspect:
                GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                transform.localScale = Vector3.one;
                break;
            case CardState.Destroy:
                Debug.LogWarning("Wasn't able to destroy " + cardId);
                break;
            case CardState.Hidden:
                this.gameObject.SetActive(true);
                break;
        }
    }
}
