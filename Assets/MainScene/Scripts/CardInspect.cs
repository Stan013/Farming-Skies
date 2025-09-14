using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardInspect : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && GameManager.HM.dragging != true)
        {
            if (GameManager.CM.inspectCard == null)
            {
                GameManager.WM.inMenu = true;
                GameManager.CM.inspectCard = GetComponent<Card>();
                GameManager.CM.inspectCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 525);
                GameManager.CM.inspectCard.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
                if(GameManager.CM.inspectCard.hasBeenInspected != true)
                {
                    GameManager.CM.inspectCard.hasBeenInspected = true;
                }
            }
            else
            {
                if (GameManager.CM.inspectCard == GetComponent<Card>())
                {
                    GameManager.WM.inMenu = false;
                    GameManager.CM.inspectCard.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    GameManager.CM.inspectCard.transform.localScale = new Vector3(0.5f,0.5f,1f);
                    GameManager.CM.inspectCard = null;
                }
            }
        }
    }
}
