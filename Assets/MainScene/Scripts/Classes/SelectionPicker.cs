using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionPicker : MonoBehaviour
{
    [SerializeField] private SelectionChoice selectionChoiceTemplate;

    public List<SelectionChoice> selectionChoices = new List<SelectionChoice>();
    private int[] values = { 1, 2, 3, 4, 5 };
    private int[] weights = { 30, 25, 20, 15, 10 };

    public void SetupPicker(SelectionPicker picker)
    {
        selectionChoices.Clear();
        int[] selectionAmounts = GetWeightedRandom();
        List<string> selectionTypes = GetRandomTypes();

        for (int i = 0; i < 3; i++)
        {
            SelectionChoice choice = Instantiate(selectionChoiceTemplate, Vector3.zero, Quaternion.identity, picker.transform);
            choice.transform.localPosition = new Vector3(0, 125 - (175*i), 0);
            choice.transform.localRotation = Quaternion.Euler(0, 0, 0);
            choice.SetupSelectionChoice(selectionAmounts[i], selectionTypes[i]);
            selectionChoices.Add(choice);
        }
    }

    private int[] GetWeightedRandom()
    {
        int[] results = new int[3];

        for (int j = 0; j < results.Length; j++)
        {
            int totalWeight = 0;
            foreach (int weight in weights)
                totalWeight += weight;

            int random = Random.Range(0, totalWeight);
            int sum = 0;

            for (int i = 0; i < values.Length; i++)
            {
                sum += weights[i];
                if (random < sum)
                {
                    results[j] = values[i];
                    break;
                }
            }
        }

        return results;
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
}
