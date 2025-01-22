using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardInspect : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (GameManager.CM.inspectCard == null)
            {
                GameManager.CM.inspectCard = GetComponent<Card>();
                GameManager.CM.inspectCard.ToggleState(Card.CardState.Inspect, Card.CardState.InHand);
                GameManager.CM.inspectCard.GetComponent<CardDrag>().enabled = false;
            }
            else
            {
                if (GameManager.CM.inspectCard == GetComponent<Card>())
                {
                    GameManager.CM.CheckCardInspectTutorial(GameManager.CM.inspectCard);
                    GameManager.CM.inspectCard.ToggleState(Card.CardState.InHand, Card.CardState.Inspect);
                    GameManager.CM.inspectCard.GetComponent<CardDrag>().enabled = true;
                    GameManager.CM.inspectCard = null;
                }
            }
        }
    }
}
