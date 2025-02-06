using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketItem : MonoBehaviour
{
    public TMP_Text itemQuantityText;
    public TMP_Text itemNameText;
    public Image itemImage;
    public Image changeIcon;
    public Sprite upIcon;
    public Sprite downIcon;
    public TMP_Text priceText;
    public TMP_Text pricehigh3dText;
    public TMP_Text pricelow3dText;
    public TMP_Text pricehigh5dText;
    public TMP_Text pricelow5dText;
    public TMP_Text pricehigh7dText;
    public TMP_Text pricelow7dText;
    public TMP_Text demandText;
    public TMP_Text supplyText;
    public MarketButton sellButton;
    public MarketButton buyButton;
    public List<float> allItemPrices;
    public int itemIndex;
    public Card attachedItemCard;
    public float priceCurrent;
    private float pricehigh3d;
    private float pricelow3d;
    private float pricehigh5d;
    private float pricelow5d;
    private float pricehigh7d;
    private float pricelow7d;

    public void SetMarketItem(Card itemCard)
    {
        if(itemCard != null)
        {
            attachedItemCard = itemCard;
            itemNameText.SetText(attachedItemCard.itemName);
            itemQuantityText.SetText(attachedItemCard.itemQuantity.ToString());
            itemIndex = GameManager.MM.itemsInMarket.Count;
            itemImage.sprite = attachedItemCard.cardSprite;
            allItemPrices.Add(priceCurrent);
            UpdateMarketItem(attachedItemCard);
            UpdateLowHighPrices();
        }
    }

    public void UpdateMarketItem(Card itemCard)
    {
        priceText.SetText("₴ " + priceCurrent.ToString());
        pricelow3dText.SetText("₴ " + pricelow3d.ToString());
        pricehigh3dText.SetText("₴ " + pricehigh3d.ToString());
        pricelow5dText.SetText("₴ " + pricelow5d.ToString());
        pricehigh5dText.SetText("₴ " + pricehigh5d.ToString());
        pricelow7dText.SetText("₴ " + pricelow7d.ToString());
        pricehigh7dText.SetText("₴ " + pricehigh7d.ToString());
        demandText.SetText(FormatNumber(itemCard.itemDemand));
        supplyText.SetText(FormatNumber(itemCard.itemSupply));
        if (allItemPrices.Count != 1)
        {
            if (priceCurrent < allItemPrices[allItemPrices.Count - 1])
            {
                changeIcon.sprite = upIcon;
            }
            else
            {
                changeIcon.sprite = downIcon;
            }
        }
    }

    private string FormatNumber(float number)
    {
        if (number >= 1000000000)
        {
            return (number / 1000000000f).ToString("0.0") + "B";
        }
        if (number >= 1000000)
        {
            return (number / 1000000f).ToString("0.0") + "M";
        }
        else if (number >= 1000) 
        {
            return (number / 1000f).ToString("0.0") + "K";
        }
        else
        {
            return number.ToString("0");
        }
    }
    public void UpdateLowHighPrices()
    {
        if (allItemPrices.Count > 7 && allItemPrices[6] != 0)
        {
            allItemPrices.RemoveAt(0);
        }
        pricehigh3d = allItemPrices.Where(price => price > 0).Take(3).DefaultIfEmpty(0).Max();
        pricelow3d = allItemPrices.Where(price => price > 0).Take(3).DefaultIfEmpty(float.MaxValue).Min();

        pricehigh5d = allItemPrices.Where(price => price > 0).Take(5).DefaultIfEmpty(0).Max();
        pricelow5d = allItemPrices.Where(price => price > 0).Take(5).DefaultIfEmpty(float.MaxValue).Min();

        pricehigh7d = allItemPrices.Where(price => price > 0).Take(7).DefaultIfEmpty(0).Max();
        pricelow7d = allItemPrices.Where(price => price > 0).Take(7).DefaultIfEmpty(float.MaxValue).Min();
    }
}
