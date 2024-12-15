using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextDayButton : MonoBehaviour
{
    [SerializeField] private Button openButton;
    void Start()
    {
        openButton.onClick.AddListener(OnButtonClick);
    }
    private void OnButtonClick()
    {
        GameManager.TM.NextDay();
        GameManager.PM.Harvest();
        GameManager.MM.UpdatePrices();
    }
}
