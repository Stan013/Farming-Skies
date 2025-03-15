using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketItem : MonoBehaviour
{
    public int itemIndex;
    public Card attachedItemCard;

    public List<float> itemPricesMonth;
    public List<float> itemPrices6Month;
    public List<float> itemPricesYear;
    public float previousPrice = 0;
    public float previousDemand = 0;
    public float previousSupply = 0;

    public TMP_Text itemQuantityText;
    public TMP_Text itemNameText;
    public Image itemImage;
    public Image supplyChangeIcon, demandChangeIcon, priceChangeIcon;
    public Sprite upIcon, neutralIcon, downIcon;

    public TMP_Text priceText, priceMonthHighText, priceMonthAverageText, priceMonthLowText, price6MonthHighText, price6MonthAverageText, price6MonthLowText, priceYearHighText, priceYearAverageText, priceYearLowText;
    public TMP_Text demandText, supplyText;
    public float priceCurrent, priceMonthHigh, priceMonthAverage, priceMonthLow, price6MonthHigh, price6MonthAverage, price6MonthLow, priceYearHigh, priceYearAverage, priceYearLow;
    
    public MarketButton sellUI, buyUI;
    public GameObject itemDetails;
    public GameObject itemPrices;

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
            //for (int i = 0; i < 4; i++) itemPricesMonth.Add(priceCurrent);
            //for (int i = 0; i < 24; i++) itemPrices6Month.Add(priceCurrent);
            //for (int i = 0; i < 52; i++) itemPricesYear.Add(priceCurrent);
            //UpdateDetailedPrices();
            UpdateMarketItem(attachedItemCard);
        }
    }

    public void UpdateMarketItem(Card itemCard)
    {
        priceText.SetText(priceCurrent.ToString() + " ₴");
        //priceMonthLowText.SetText(priceMonthLow.ToString() + " ₴");
        //priceMonthAverageText.SetText(priceMonthAverage.ToString() + " ₴");
        //priceMonthHighText.SetText(priceMonthHigh.ToString() + " ₴");
        //price6MonthLowText.SetText(price6MonthLow.ToString() + " ₴");
        //price6MonthAverageText.SetText(priceMonthAverage.ToString() + " ₴");
        //price6MonthHighText.SetText(price6MonthHigh.ToString() + " ₴");
        //priceYearLowText.SetText(priceYearLow.ToString() + " ₴");
        //priceYearAverageText.SetText(priceYearAverage.ToString() + " ₴");
        //priceYearHighText.SetText(priceYearHigh.ToString() + " ₴");
        demandText.SetText(FormatNumber(itemCard.itemDemand));
        supplyText.SetText(FormatNumber(itemCard.itemSupply));

        if (previousPrice != 0)
        {
            if (priceCurrent > previousPrice)
                priceChangeIcon.sprite = upIcon;
            else if (priceCurrent < previousPrice)
                priceChangeIcon.sprite = downIcon;
            else
                priceChangeIcon.sprite = neutralIcon;
        }
        else
        {
            priceChangeIcon.sprite = neutralIcon;
        }


        if (previousDemand != 0)
        {
            if (itemCard.itemDemand > previousDemand)
                demandChangeIcon.sprite = upIcon;
            else if (itemCard.itemDemand < previousDemand)
                demandChangeIcon.sprite = downIcon;
            else
                demandChangeIcon.sprite = neutralIcon;
        }
        else
        {
            demandChangeIcon.sprite = neutralIcon;
        }

        if (previousSupply != 0)
        {
            if (itemCard.itemSupply > previousSupply)
                supplyChangeIcon.sprite = upIcon;
            else if (itemCard.itemSupply < previousSupply)
                supplyChangeIcon.sprite = downIcon;
            else
                supplyChangeIcon.sprite = neutralIcon;
        }
        else
        {
            supplyChangeIcon.sprite = neutralIcon;
        }

        previousPrice = priceCurrent;
        previousDemand = itemCard.itemDemand;
        previousSupply = itemCard.itemSupply;
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

    public void UpdateDetailedPrices()
    {
        if (itemPricesMonth.Count >= 4) itemPricesMonth.RemoveAt(0);
        itemPricesMonth.Add(priceCurrent);

        if (itemPrices6Month.Count >= 24) itemPrices6Month.RemoveAt(0);
        itemPrices6Month.Add(priceCurrent);

        if (itemPricesYear.Count >= 52) itemPricesYear.RemoveAt(0);
        itemPricesYear.Add(priceCurrent);

        priceMonthHigh = itemPricesMonth.Max();
        priceMonthLow = itemPricesMonth.Min();
        priceMonthAverage = itemPricesMonth.Average();

        price6MonthHigh = itemPrices6Month.Max();
        price6MonthLow = itemPrices6Month.Min();
        price6MonthAverage = itemPrices6Month.Average();

        priceYearHigh = itemPricesYear.Max();
        priceYearLow = itemPricesYear.Min();
        priceYearAverage = itemPricesYear.Average();
    }


    public void SwitchItemInfo()
    {
        if (itemDetails.activeSelf)
        {
            itemDetails.SetActive(false);
            itemPrices.SetActive(true);
            //UpdateDetailedPrices();
        }
        else
        {
            itemDetails.SetActive(true);
            itemPrices.SetActive(false);
        }
    }
}
