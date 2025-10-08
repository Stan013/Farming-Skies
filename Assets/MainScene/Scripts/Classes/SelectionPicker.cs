using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionPicker : MonoBehaviour
{
    [SerializeField] private SelectionChoice selectionChoiceTemplate;
    [SerializeField] private CardSlot cardSlot;
    [SerializeField] private Sprite cardSelectionSprite;
    [SerializeField] private GameObject cardTypeBackground;
    [SerializeField] private Sprite cardTypeSprite;
    [SerializeField] private GameObject pickCardButton;
    [SerializeField] private GameObject cardAmount;
    [SerializeField] private TMP_Text cardAmountText;


    public TMP_Text selectionText;
    public List<SelectionChoice> selectionChoices = new List<SelectionChoice>();
    private int[] values = { 1, 2, 3};
    private int[] weights = { 50, 35, 15};

    public void SetupPicker(SelectionPicker picker)
    {
        List<string> selectionTypes = GetRandomTypes();

        for (int i = 0; i < 3; i++)
        {
            SelectionChoice choice = Instantiate(selectionChoiceTemplate, Vector3.zero, Quaternion.identity, picker.transform);
            choice.transform.localPosition = new Vector3(0, 125 - (175*i), 0);
            choice.transform.localRotation = Quaternion.Euler(0, 0, 0);
            choice.SetupSelectionChoice(selectionTypes[i], picker);
            selectionChoices.Add(choice);
        }
    }

    private int GetWeightedRandom()
    {
        int totalWeight = 0;
        for (int i = 0; i < weights.Length; i++)
            totalWeight += weights[i];

        int random = Random.Range(0, totalWeight);

        int cumulative = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            cumulative += weights[i];
            if (random < cumulative)
            {
                return values[i];
            }
        }

        return values[values.Length - 1];
    }

    private List<string> GetRandomTypes()
    {
        List<string> availableTypes = new List<string>(GameManager.CM.cardTypes);
        List<string> chosenTypes = new List<string>();

        for (int i = 0; i < 3 && availableTypes.Count > 0; i++)
        {
            int index = Random.Range(0, availableTypes.Count);
            chosenTypes.Add(availableTypes[index]);
            availableTypes.RemoveAt(index);
        }

        chosenTypes.Sort();

        return chosenTypes;
    }

    public void GenerateCard(List<Card> cardList)
    {
        int randomIndex = Random.Range(0, cardList.Count);
        Card randomCard = Instantiate(cardList[randomIndex], Vector3.zero, Quaternion.identity, cardSlot.transform);
        GameManager.CM.InitializeCard(randomCard);
        randomCard.transform.localPosition = Vector3.zero;
        randomCard.transform.localRotation = Quaternion.Euler(0, 0, 0);
        randomCard.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        randomCard.GetComponent<Image>().sprite = cardSelectionSprite;
        cardSlot.cardInSlot = randomCard;
        cardTypeBackground.GetComponent<Image>().sprite = cardTypeSprite;
        pickCardButton.SetActive(true);
        cardAmount.SetActive(true);
        cardAmountText.text = "x" + GetWeightedRandom().ToString();
    }

    public void PickCard()
    {
        int amount = int.Parse(cardAmountText.text.Replace("x", ""));
        for (int i = 0; i < amount; i++)
        {
            Card addCard = cardSlot.cardInSlot;
            GameManager.DM.AddCardToDeck(addCard.cardId);
        }
    }
}
