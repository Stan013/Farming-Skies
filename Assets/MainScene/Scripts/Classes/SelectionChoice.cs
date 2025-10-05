using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionChoice : MonoBehaviour
{
    [SerializeField] private List<Sprite> selectionTypeSprites = new List<Sprite>();
    [SerializeField] private Image selectionImage;
    [SerializeField] private TMP_Text selectionText;
    
    private int[] values = { 1, 2, 3, 4, 5 };
    private int[] weights = { 30, 25, 20, 15, 10 };

    public string selectionType;
    public SelectionPicker pickerParent;
    public Sprite selectionCardSprite;


    public void SetupSelectionChoice(string type, SelectionPicker picker)
    {
        pickerParent = picker;
        selectionType = type;
        switch(type)
        {
            case "Crop":
                selectionImage.sprite = selectionTypeSprites[0];
                selectionText.text = "Crop";
                break;
            case "Utility":
                selectionImage.sprite = selectionTypeSprites[1];
                selectionText.text = "Utility";
                break;
            case "Structure":
                selectionImage.sprite = selectionTypeSprites[2];
                selectionText.text = "Structure";
                break;
        }
    }

    public void ChooseType()
    {
        pickerParent.selectionText.text = selectionType;
        
        switch (selectionType)
        {
            case "Crop":
                pickerParent.GenerateCard(GameManager.SM.cropCards);
                break;
            case "Utility":
                pickerParent.GenerateCard(GameManager.SM.utilityCards);
                break;
            case "Structure":
                pickerParent.GenerateCard(GameManager.SM.structureCards);
                break;
        }

        foreach (SelectionChoice choice in pickerParent.selectionChoices)
        {
            Destroy(choice.gameObject);
        }
    }
}
