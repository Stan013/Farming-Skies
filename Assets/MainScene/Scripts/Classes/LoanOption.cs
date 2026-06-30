using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoanOption : MonoBehaviour
{
    [Header("Lending farm variables")]
    [SerializeField] private Image loanIcon;
    [SerializeField] private TMP_Text  loanName;

    [Header("Loan variables")]
    public string loanType;
    public int loanTerm;
    [SerializeField] private TMP_Text loanTermText;
    public int loanAmount;
    [SerializeField] private TMP_Text loanAmountText;
    public float loanInterest;
    [SerializeField] private TMP_Text loanInterestText;

    public void GenerateLoanOption(int loanIndex)
    {
        loanName.text = GameManager.LM.farmNames[loanIndex];
        loanIcon.sprite = GameManager.LM.farmIcons[loanIndex];

        if (loanType == "ShortTerm")
        {
            loanTerm = Random.Range(4,9);
            loanInterest = Random.Range(90,121) / 10f;
            loanAmount = Random.Range(250 * GameManager.FM.FarmLevel, 500 * GameManager.FM.FarmLevel + 1);
            loanTermText.text = loanTerm + " weeks";
            loanAmountText.text = GameManager.UM.FormatNumber(loanAmount, true) + " ₴";
            loanInterestText.text = loanInterest + " %";
        }
        else if (loanType == "MidTerm")
        {
            loanTerm = Random.Range(10,15);
            loanInterest = Random.Range(60,91) / 10f;
            loanAmount = Random.Range(250 * GameManager.FM.FarmLevel, 500 * GameManager.FM.FarmLevel + 1);
            loanTermText.text = loanTerm + " weeks";
            loanAmountText.text = GameManager.UM.FormatNumber(loanAmount, true) + " ₴";
            loanInterestText.text = loanInterest + " %";
        }
        else
        {
            loanTerm = Random.Range(16,21);
            loanInterest = Random.Range(30,61) / 10f;
            loanAmount = Random.Range(250 * GameManager.FM.FarmLevel, 500 * GameManager.FM.FarmLevel + 1);
            loanTermText.text = loanTerm + " weeks";
            loanAmountText.text = GameManager.UM.FormatNumber(loanAmount, true) + " ₴";
            loanInterestText.text = loanInterest + " %";
        }
    }

    public void AcceptLoan()
    {
        
    }

    public void ChangeLoan()
    {
        
    }
}
