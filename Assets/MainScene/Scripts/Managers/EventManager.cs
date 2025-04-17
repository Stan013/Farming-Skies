using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    [Header("Event lists")]
    public List<EventItem> upcomingEvents;
    public List<EventItem> previousEvents;

    [Header("Event variables")]
    public GameObject eventContentArea;
    public EventItem eventItemTemplate;
    public Button closeButton;
    public int eventCount;

    public void SetupEvents()
    {
        for(int i = 1; i <= 52; i++)
        {
            EventItem eventItem = Instantiate(eventItemTemplate, Vector3.zero, Quaternion.identity, eventContentArea.transform);
            if (i % 4 == 0 && i != 0)
            {
                eventCount++;
                switch (eventCount)
                {
                    case 1:
                        eventItem.SetupEventItem("NewCards", i);
                        break;
                    case 2:
                        eventItem.SetupEventItem("RefillNutrients", i);
                        break;
                    case 3:
                        eventItem.SetupEventItem("PayExpenses", i);
                        eventCount = 0;
                        break;
                }
            }
            else
            {
                eventItem.SetupEventItem("Default", i);
            }
            eventItem.transform.localRotation = Quaternion.identity;
            eventItem.transform.localPosition = new Vector3(eventItem.transform.localPosition.x, eventItem.transform.localPosition.y, 0);
            upcomingEvents.Add(eventItem);
        }
    }

    public void FilterItemsInEvent(string filter)
    {
        if (filter == "Default")
        {
            foreach (EventItem eventItem in upcomingEvents)
            {
                eventItem.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (EventItem eventItem in upcomingEvents)
            {
                if (eventItem.eventItemType != filter)
                {
                    eventItem.gameObject.SetActive(false);
                }
                else
                {
                    eventItem.gameObject.SetActive(true);
                }
            }
        }
    }
}
