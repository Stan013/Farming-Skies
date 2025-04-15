using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwitchTransaction : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if(!GameManager.MM.expandedMarketItem.gameObject.activeSelf)
            {
                SwitchMarketTransaction(this.transform.parent.parent.GetComponent<MarketItem>());
            }
            else
            {
                SwitchExpandedMarketTransaction(this.transform.parent.parent.GetComponent<ExpandedMarketItem>());
            }
        }
    }

    public void SwitchMarketTransaction(MarketItem marketItem)
    {
        if (marketItem.marketTransaction == "Sell")
        {
            marketItem.transactionText.text = "Buy";
            marketItem.marketTransaction = "Buy";
        }
        else
        {
            marketItem.transactionText.text = "Sell";
            marketItem.marketTransaction = "Sell";
        }
        marketItem.CheckValidTransactionAmount("0");
    }

    public void SwitchExpandedMarketTransaction(ExpandedMarketItem marketItem)
    {
        if (marketItem.marketTransaction == "Sell")
        {
            marketItem.transactionText.text = "Buy";
            marketItem.marketTransaction = "Buy";
        }
        else
        {
            marketItem.transactionText.text = "Sell";
            marketItem.marketTransaction = "Sell";
        }
        marketItem.CheckValidTransactionAmount("0");
    }
}
