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

    public Image eventItemBackground;
    public Sprite defaultBackground;
    public Sprite eventBackground;

    public Sprite defaultIcon;
    public Sprite newCardIcon;
    public Sprite nutrientRefillIcon;
    public Sprite payTaxesIcon;

    public void SetupEventItem(string eventType, int weekCount)
    {
        weekText.text = "Week " + weekCount;
        eventItemType = eventType;
        switch (eventType)
        {
            case "NewCards": // New cards
                eventItemBackground.sprite = eventBackground;
                eventIcon.sprite = newCardIcon;
                eventText.text = "Recieve cards";
                break;
            case "RefillNutrients": // Island nutrients refill
                eventItemBackground.sprite = eventBackground;
                eventIcon.sprite = nutrientRefillIcon;
                eventText.text = "Refill nutrients";
                break;
            case "PayExpenses": // Pay taxes
                eventItemBackground.sprite = eventBackground;
                eventIcon.sprite = payTaxesIcon;
                eventText.text = "Pay expenses";
                break;
            default:
                eventItemBackground.sprite = defaultBackground;
                eventIcon.sprite = defaultIcon;
                eventText.text = "Casual day";
                break;
        }
    }
}
