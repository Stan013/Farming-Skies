using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketItem : MonoBehaviour
{
    //Generate id for saving
    [SerializeField] private string id;
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    [SerializeField] private TMP_Text itemQuantityText;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private Image itemImage;
    [SerializeField] private Image changeIcon;
    [SerializeField] private Sprite upIcon;
    [SerializeField] private Sprite downIcon;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private TMP_Text pricehigh3dText;
    [SerializeField] private TMP_Text pricelow3dText;
    [SerializeField] private TMP_Text pricehigh5dText;
    [SerializeField] private TMP_Text pricelow5dText;
    [SerializeField] private TMP_Text pricehigh7dText;
    [SerializeField] private TMP_Text pricelow7dText;
    [SerializeField] private TMP_Text demandText;
    [SerializeField] private TMP_Text supplyText;
    [SerializeField] private MarketButton m_SellButton;
    public MarketButton sellButton { get { return m_SellButton; } }
    [SerializeField] private MarketButton m_BuyButton;
    public MarketButton buyButton { get { return m_BuyButton; } }
    [SerializeField] private List<float> m_AllItemPrices;
    public List<float> allItemPrices { get { return m_AllItemPrices; } }
    public int itemQuantity;
    public int itemIndex;
    public string itemName;
    public Sprite itemSprite;
    public float priceCurrent;
    private float pricehigh3d;
    private float pricelow3d;
    private float pricehigh5d;
    private float pricelow5d;
    private float pricehigh7d;
    private float pricelow7d;
    public float itemDemand;
    public float itemSupply;

    public void SetItemQuantity()
    {
        itemQuantityText.SetText(itemQuantity.ToString());
    }

    public void SetMarketItem(Card itemCard)
    {
        if(itemCard != null)
        {
            itemName = itemCard.itemName;
            itemNameText.SetText(itemName);
            itemImage.sprite = itemCard.cardSprite;
            itemIndex = GameManager.MM.itemsInMarket.Count;
            itemDemand = itemCard.itemDemand;
            itemSupply = itemCard.itemSupply;
            priceCurrent = itemCard.itemPrice;
            itemQuantity = 0;
            itemQuantityText.SetText(itemQuantity.ToString());
            allItemPrices.Add(priceCurrent);
            UpdateLowHighPrices();
            UpdateMarketItem();
        }
    }

    public void UpdateMarketItem()
    {
        priceText.SetText("₴ " + priceCurrent.ToString());
        pricelow3dText.SetText("₴ " + pricelow3d.ToString());
        pricehigh3dText.SetText("₴ " + pricehigh3d.ToString());
        pricelow5dText.SetText("₴ " + pricelow5d.ToString());
        pricehigh5dText.SetText("₴ " + pricehigh5d.ToString());
        pricelow7dText.SetText("₴ " + pricelow7d.ToString());
        pricehigh7dText.SetText("₴ " + pricehigh7d.ToString());
        demandText.SetText(FormatNumber(itemDemand));
        supplyText.SetText(FormatNumber(itemSupply));
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
