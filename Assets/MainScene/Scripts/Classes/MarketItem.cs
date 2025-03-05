using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketItem : MonoBehaviour
{
    public List<float> allItemPrices;
    public int itemIndex;
    public Card attachedItemCard;

    public TMP_Text itemQuantityText;
    public TMP_Text itemNameText;
    public Image itemImage;
    public Image changeIcon;
    public Sprite upIcon, downIcon;

    public TMP_Text priceText, pricehigh3dText, pricelow3dText, pricehigh5dText, pricelow5dText, pricehigh7dText, pricelow7dText;
    public TMP_Text demandText, supplyText;
    public float priceCurrent, pricehigh3d, pricelow3d, pricehigh5d, pricelow5d, pricehigh7d, pricelow7d;
    public MarketButton sellUI, buyUI;

    public void SetMarketItem(Card itemCard)
    {
        if(itemCard != null)
        {
            attachedItemCard = itemCard;
            itemNameText.SetText(attachedItemCard.itemName);
            itemQuantityText.SetText(attachedItemCard.itemQuantity.ToString());
            itemIndex = GameManager.MM.itemsInMarket.Count;
            itemImage.sprite = attachedItemCard.cardSprite;
            priceCurrent = attachedItemCard.itemPrice;
            for (int i = 0; i < 7; i++)
            {
                allItemPrices.Add(priceCurrent);
            }
            UpdateLowHighPrices();
            UpdateMarketItem(attachedItemCard);
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
                changeIcon.sprite = downIcon;
            }
            else
            {
                if(priceCurrent == allItemPrices[allItemPrices.Count - 1])
                {
                    //Add no market change icon
                }
                else
                {
                    changeIcon.sprite = upIcon;

                }
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
        if (priceCurrent != allItemPrices[0])
        {
            allItemPrices.Insert(1, priceCurrent);
            allItemPrices.RemoveAt(0);
        }
        pricehigh3d = allItemPrices.Take(3).DefaultIfEmpty(0).Max();
        pricelow3d = allItemPrices.Take(3).DefaultIfEmpty(float.MaxValue).Min();
        pricehigh5d = allItemPrices.Take(5).DefaultIfEmpty(0).Max();
        pricelow5d = allItemPrices.Take(5).DefaultIfEmpty(float.MaxValue).Min();
        pricehigh7d = allItemPrices.Take(7).DefaultIfEmpty(0).Max();
        pricelow7d = allItemPrices.Take(7).DefaultIfEmpty(float.MaxValue).Min();
    }
}
