using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionChoice : MonoBehaviour
{
    [SerializeField] private List<Sprite> selectionTypeSprites = new List<Sprite>();
    [SerializeField] private TMP_Text selectionAmountText;
    [SerializeField] private Image selectionImage;
    [SerializeField] private TMP_Text selectionText;

    public string selectionType;
    public int selectionAmount;

    public void SetupSelectionChoice(int amount, string type)
    {
        selectionAmount = amount;
        selectionType = type;
        selectionAmountText.text = "x" + amount;
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
}
