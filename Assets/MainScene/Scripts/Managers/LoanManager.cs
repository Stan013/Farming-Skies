using UnityEngine;
using System.Collections.Generic;

public class LoanManager : MonoBehaviour
{
    [Header("Lending variables")]
    public string[] farmNames;
    public Sprite[] farmIcons;
    public int farmIndex;

    [Header("Loan variables")]
    [SerializeField] private LoanOption loanOptionShort;
    [SerializeField] private LoanOption loanOptionMid;
    [SerializeField] private LoanOption loanOptionLong;
    public int loanReputation;
    public string interestPlan;
    
    public void GenerateLoanOptions()
    {
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < farmNames.Length; i++)
        {
            availableIndices.Add(i);
        }

        for (int i = availableIndices.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (availableIndices[i], availableIndices[j]) = (availableIndices[j], availableIndices[i]);
        }

        LoanOption[] loanOptions = { loanOptionShort, loanOptionMid, loanOptionLong };

        for (int i = 0; i < 3 && i < availableIndices.Count; i++)
        {
            loanOptions[i].GenerateLoanOption(availableIndices[i]);
        }
    }
}
