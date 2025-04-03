using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class Card : MonoBehaviour
{
    [Header("Invisible card variables")] 
    public string cardId;
    public bool cardStarter;
    public bool cardSetup;
    public CardState CurrentState;
    public bool dragSucces;
    public int nutrientIndex;
    public int nutrientAddition;

    [Header("Visible card variables")]
    public string cardName;
    public TMP_Text cardNameText;
    public string cardDescription;
    public TMP_Text cardDescriptionText;

    [Header("Card in-hand variables")]
    public GameObject cardImage;
    public Sprite cardSprite;

    [Header("Card in-choosing variables")]
    public TMP_Text cardAmountText;
    public int cardAmount;
    public GameObject cardPickButton;
    public GameObject cardAnimation;
    public VideoClip cardClip;

    [Header("Card in-craft variables")]
    public int[] cardCraftResources;
    public int cardDropsRequired;
    public int cardCraftIndex;

    [Header("Associated item variables")]
    public string itemName;
    public Sprite itemSprite;

    [Header("Associated inventory item variables")]
    public InventoryItem inventoryItem;
    public bool cardUnlocked;

    [Header("Associated market item variables")]
    public MarketItem marketItem;
    public bool cardAddedToMarket;
    public float itemPrice;
    public float itemDemand;
    public float itemSupply;

    [Header("Associated plant variables")]
    public string cardType;
    public string plantGroup;
    public TMP_Text plantSizeText;
    public TMP_Text waterText;
    public TMP_Text nitrogenText;
    public TMP_Text phosphorusText;
    public TMP_Text potassiumText;

    [Header("Tutorial card variables")]
    public bool hasBeenInspected;

    public enum CardState
    {
        InDeck,
        InHand,
        InChoosing,
        InDrag,
        Hidden,
        Available,
        InCraft,
    }

    public void SetCardState(CardState newState)
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
                GameManager.DM.cardsInDeck.Add(this);
                this.gameObject.transform.SetParent(GameManager.DM.cardsInDeckParent.transform);
                this.gameObject.SetActive(false);
                break;
            case CardState.InHand:
                if (!cardSetup)
                {
                    GameManager.CM.InitializeCard(GetComponent<Card>());
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
                cardName = cardName.Replace(" crops", "").Trim();
                cardNameText.SetText(cardName);
                break;
            case CardState.InDrag:
                GetComponent<Image>().enabled = false;
                foreach (Transform child in this.transform)
                {
                    child.gameObject.SetActive(false);
                }
                break;
            case CardState.InCraft:
                if (!cardSetup)
                {
                    GameManager.CM.InitializeCard(GetComponent<Card>());
                }
                break;
            case CardState.Hidden:
                this.gameObject.SetActive(false);
                break;
        }
    }

    private void ExitState(CardState state)
    {
        switch (state)
        {
            case CardState.InDeck:
                GameManager.DM.cardsInDeck.Remove(this);
                GameManager.HM.cardsInHand.Add(this);
                break;
            case CardState.InHand:
                break;
            case CardState.InDrag:
                if(dragSucces)
                {
                    GameManager.HM.lastFilledSlotIndex--;
                    GameManager.HM.cardsInHand.Remove(this);
                    GameManager.HM.MoveCardsInHand(this);
                    GameManager.HM.SetCardsInHand();
                    Destroy(this.gameObject);
                    dragSucces = false;
                }
                else
                {
                    GetComponent<Image>().enabled = true;
                    foreach (Transform child in this.transform)
                    {
                        if (cardType == "Small crops" || cardType == "Medium crops" || cardType == "Large crops")
                        {
                            if (child.gameObject.name != "CardAnimation" && child.gameObject.name != "CardPickButton")
                            {
                                child.gameObject.SetActive(true);
                            }
                        }
                        else
                        {
                            if (child.gameObject.name != "CardAnimation" && child.gameObject.name != "CardPickButton" && child.gameObject.name != "PlantSize")
                            {
                                child.gameObject.SetActive(true);
                            }
                        }
                    }
                }
                break;
            case CardState.Hidden:
                this.gameObject.SetActive(true);
                break;
        }
    }
}
