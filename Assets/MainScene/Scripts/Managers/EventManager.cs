using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour, IDataPersistence
{
    [Header("Event lists")]
    public List<EventItem> upcomingEvents;
    public List<EventItem> previousEvents;

    [Header("Event variables")]
    public GameObject eventContentArea;
    public EventItem eventItemTemplate;
    public string eventTab;
    public Button closeButton;
    public int eventCount;

    IEnumerator Delay(System.Action action)
    {
        yield return new WaitForSeconds(0.3f);
        action?.Invoke();
    }

    public void SetupEvents(int lastEvent)
    {
        if(lastEvent != 0)
        {
            eventCount = lastEvent - 1;
        } 

        for(int i = 1; i <= 52; i++)
        {
            if(i >= GameManager.TM.Weeks)
            {
                EventItem eventItem = Instantiate(eventItemTemplate, Vector3.zero, Quaternion.identity, eventContentArea.transform);
                if (i % 4 == 0 && i != 0)
                {
                    eventCount++;
                    switch (eventCount)
                    {
                        case 1:
                            // eventItem.SetupEventItem("PayExpenses", i);
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
                    upcomingEvents.Add(eventItem);
                }
                else
                {
                    eventItem.SetupEventItem("Default", i);
                    upcomingEvents.Add(eventItem);
                }
                eventItem.transform.localRotation = Quaternion.identity;
                eventItem.transform.localPosition = new Vector3(eventItem.transform.localPosition.x, eventItem.transform.localPosition.y, 0);
            }
        }
    }

    public void FilterItemsInEvent(string filter)
    {
        eventTab = filter;
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

    public void CheckEvent()
    {
        EventItem pastEvent = upcomingEvents[0];
        upcomingEvents.RemoveAt(0);
        previousEvents.Add(pastEvent);
        switch (pastEvent.eventItemType)
        {
            case "NewCards":
                StartCoroutine(Delay(() =>
                {
                    GameManager.SM.SetupSelection();
                    GameManager.WM.OpenWindow(GameManager.WM.selectionWindow);
                }));
                break;
            case "RefillNutrients":
                StartCoroutine(Delay(() =>
                {
                    GameManager.WM.OpenWindow(GameManager.WM.manageWindow);
                    GameManager.ISM.OpenIslandManagement("Available");
                    StartCoroutine(GameManager.RM.RefillEvent());
                }));
                break;
            case "PayExpenses":
                break;
            default:
                GameManager.WM.inMenu = false;
                break;
        }
        Destroy(pastEvent);
    }

    public void LoadData(GameData data)
    {
        GameManager.DPM.ClearChildren(eventContentArea.transform);
        upcomingEvents.Clear();
        SetupEvents(data.lastEvent);
    }

    public void SaveData(ref GameData data)
    {
        foreach (EventItem eventItem in upcomingEvents)
        {
            switch (eventItem.eventItemType)
            {
                case "NewCards":
                    data.lastEvent = 1;
                    return;
                case "RefillNutrients":
                    data.lastEvent = 2;
                    return;
                case "PayExpenses":
                    data.lastEvent = 3;
                    return;
                case "Default":
                    break;
            }
        }
    }
}
