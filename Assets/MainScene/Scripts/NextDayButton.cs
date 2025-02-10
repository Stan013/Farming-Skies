using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NextDayButton : MonoBehaviour
{
    [SerializeField] private Button openButton;
    void Start()
    {
        openButton.onClick.AddListener(OnButtonClick);
    }
    private void OnButtonClick()
    {
        if (GameManager.TTM.tutorialCount == 8)
        {
            GameManager.TTM.QuestCompleted = true;
            GameManager.UM.nextDayButton.GetComponent<Image>().color = Color.white;
        }
        GameManager.PM.Harvest(); //Includes Water and nutrients Check
        GameManager.MM.UpdatePrices(); // Includes Market Update
        GameManager.TM.NextDay(); //Includes Update UI
        EventSystem.current.SetSelectedGameObject(null);
    }
}
