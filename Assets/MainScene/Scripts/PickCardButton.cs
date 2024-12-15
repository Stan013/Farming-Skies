using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickCardButton : MonoBehaviour
{
    [SerializeField] private Button pickButton;
    [SerializeField] private Card card;
    void Start()
    {
        pickButton.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        for(int i = 0; i < card.cardAmount; i++)
        {
            GameManager.DM.AddCardToDeck(card.cardId);
        }
        GameManager.ERM.ClosePickWindow();
    }
}
