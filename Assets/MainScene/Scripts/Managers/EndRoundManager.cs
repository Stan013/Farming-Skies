using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRoundManager : MonoBehaviour
{
    [SerializeField] private GameObject m_CardPickWindow;
    public GameObject cardPickWindow => m_CardPickWindow;
    [SerializeField] private GameObject pickSlotParent;
    [SerializeField] private List<CardSlot> m_PickSlots;
    public List<CardSlot> pickSlots { get { return m_PickSlots; } }

    public void GeneratePickWindow()
    {
        if (GameManager.CurrentState == GameManager.GameState.EndRoundMode)
        {
            cardPickWindow.SetActive(true);
            while (pickSlots.Count < 3)
            {
                CardSlot newSlot = Instantiate(GameManager.HM.cardSlotPrefab, new Vector3(-600f + (pickSlots.Count * 600f), -75f, 0f), Quaternion.identity);
                newSlot.transform.SetParent(pickSlotParent.transform, false);
                newSlot.transform.localScale = Vector3.one;
                pickSlots.Add(newSlot);
                Card randomCard = Instantiate(GameManager.CM.availableCards[Random.Range(0, GameManager.CM.availableCards.Count)], Vector3.zero, Quaternion.identity);
                randomCard.ToggleState(Card.CardState.InChoosing, Card.CardState.Destroy);
                newSlot.AddCardToSlot(pickSlots.Count, randomCard);
            }
        }
        else
        {
            cardPickWindow.SetActive(false);
        }
    }

    public void ClosePickWindow()
    {
        cardPickWindow.SetActive(false);
        foreach (CardSlot slot in pickSlots)
        {
            Destroy(slot.transform.GetChild(0).gameObject);
            Destroy(slot.gameObject);
        }
        pickSlots.Clear();
    }
}
