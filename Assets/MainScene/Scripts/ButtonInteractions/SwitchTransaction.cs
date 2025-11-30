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
            if (!GameManager.MM.expandedMarketItem.gameObject.activeSelf)
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
            marketItem.transactionButtonBackground.sprite = marketItem.buyTransaction;
        }
        else
        {
            marketItem.transactionText.text = "Sell";
            marketItem.marketTransaction = "Sell";
            marketItem.transactionButtonBackground.sprite = marketItem.sellTransaction;
        }
        marketItem.CheckValidTransaction();
    }

    public void SwitchExpandedMarketTransaction(ExpandedMarketItem marketItem)
    {
        if (marketItem.marketTransaction == "Sell")
        {
            marketItem.transactionText.text = "Buy";
            marketItem.marketTransaction = "Buy";
            marketItem.transactionButtonBackground.sprite = marketItem.buyTransaction;
            marketItem.balanceChangeBackground.sprite = marketItem.balanceDeduction;
        }
        else
        {
            marketItem.transactionText.text = "Sell";
            marketItem.marketTransaction = "Sell";
            marketItem.transactionButtonBackground.sprite = marketItem.sellTransaction;
            marketItem.balanceChangeBackground.sprite = marketItem.balanceAddition;
        }
        marketItem.TransactionAmount = 0;
    }
}
