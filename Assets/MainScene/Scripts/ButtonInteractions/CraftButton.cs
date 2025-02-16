using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class CraftButton : MonoBehaviour
{
    public GameObject craftWindow;
    public Button craftButton;

    void Start()
    {
        craftButton.onClick.AddListener(StartCrafting);
    }

    void StartCrafting()
    {
        if (GameManager.CRM == null) return;
        foreach (var slot in GameManager.CRM.selectionSlots)
        {
            slot.SetActive(false);
        }

        GameObject centerSlot = GameManager.CRM.selectionSlots[2];
        centerSlot.SetActive(true);
        GameManager.CRM.AssignCardToSlot(centerSlot, GameManager.CRM.selectedCard,
            new Vector3(0.6f, 0.6f, 1f), Vector3.zero, false);
    }
}
