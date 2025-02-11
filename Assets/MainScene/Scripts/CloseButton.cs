using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CloseButton : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject closeWindow;

    public void Start()
    {
        closeButton.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        closeWindow.SetActive(false);
        switch (closeButton.name)
        {
            case "InventoryButton":
                if (GameManager.TTM.tutorialCount == 9)
                {
                    GameManager.TTM.QuestCompleted = true;
                    GameManager.UM.openInventoryButton.GetComponent<Image>().color = Color.white;
                }
                break;
        }
        EventSystem.current.SetSelectedGameObject(null);
    }
}
