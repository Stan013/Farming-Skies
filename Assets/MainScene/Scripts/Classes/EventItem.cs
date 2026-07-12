using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventItem : MonoBehaviour
{
    public TMP_Text weekText;
    public Image eventIcon;
    public TMP_Text eventText;
    public string eventItemType;
    public Image weekName;
    public Image weekEvent;
    public Sprite weekNameEventBackground;
    public Sprite weekEventBackground;

    public Sprite defaultIcon;
    public Sprite newCardIcon;
    public Sprite nutrientRefillIcon;
    public Sprite newLoanIcon;
    public Sprite payTaxesIcon;

    public void SetupEventItem(string eventType, int weekCount)
    {
        weekText.text = "Week " + weekCount;
        eventItemType = eventType;
        
        if(eventType != "Default")
        {
            weekName.sprite = weekNameEventBackground;
            weekEvent.sprite = weekEventBackground;
        }

        switch (eventType)
        {
            case "NewCards": // New cards
                eventIcon.sprite = newCardIcon;
                eventText.text = "Recieve cards";
                break;
            case "RefillNutrients": // Island nutrients refill
                eventIcon.sprite = nutrientRefillIcon;
                eventText.text = "Refill nutrients";
                break;
            case "NewLoans": // Pay taxes
                eventIcon.sprite = newLoanIcon;
                eventText.text = "New loans";
                break;
            case "PayExpenses": // Pay taxes
                eventIcon.sprite = payTaxesIcon;
                eventText.text = "Pay expenses";
                break;
            default:
                eventIcon.sprite = defaultIcon;
                eventText.text = "Casual day";
                break;
        }
    }
}
