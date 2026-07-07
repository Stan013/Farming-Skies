using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoanOption : MonoBehaviour
{
    [Header("Lending farm variables")]
    [SerializeField] private Image loanIcon;
    [SerializeField] private TMP_Text  loanName;
    [SerializeField] private GameObject loanCompany;
    [SerializeField] private Image loanCompanyIcon;
    [SerializeField] private GameObject loanUnavailable;
    

    [Header("Loan variables")]
    public string loanType;
    public int loanTerm;
    [SerializeField] private TMP_Text loanTermText;
    public int loanAmount;
    [SerializeField] private TMP_Text loanAmountText;
    public float loanInterest;
    [SerializeField] private TMP_Text loanInterestText;
    [SerializeField] private Sprite loanCompanyBackground;
    [SerializeField] private Sprite loanCompanyBackgroundAccepted;
    [SerializeField] private GameObject loanDetails;
    [SerializeField] private GameObject loanAgreement;
    [SerializeField] private GameObject loanLeftDetails;
    [SerializeField] private GameObject loanRepayExtra;

    [Header("Repay loan UI variables")]
    [SerializeField] private TMP_Text weeksLeftPastText;
    [SerializeField] private TMP_Text weeksLeftDueText;
    [SerializeField] private Slider weeksLeftSlider;
    [SerializeField] private TMP_Text loanAmountLeftText;
    [SerializeField] private TMP_Text loanInterestLeftText;

    [SerializeField] private TMP_Text totalLoanLeftText;
    [SerializeField] private Image repayAmountBackground;
    [SerializeField] private Sprite repayAmountValid;
    [SerializeField] private Sprite repayAmountInvalid;
    [SerializeField] private TMP_InputField repayAmountInput;


    [Header("Repay loan variables")]
    public int weeksLeftPast;
    private int weeksLeftDue;
    public int loanAmountLeft;
    public int loanInterestLeft;
    public int totalLoanLeft;
    private int loanAmountDue;
    private int repayAmount;
    private int repayAmountStep = 10;
    private int maxRepayAmount;



    public void GenerateLoanOption(int loanIndex)
    {
        loanName.text = GameManager.LM.farmNames[loanIndex];
        loanIcon.sprite = GameManager.LM.farmIcons[loanIndex];
        loanUnavailable.SetActive(false);
        loanCompany.SetActive(true);
        loanDetails.SetActive(true);
        loanAgreement.SetActive(true);
        loanCompanyIcon.sprite = loanCompanyBackground;

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
        loanDetails.SetActive(false);
        loanAgreement.SetActive(false);
        loanLeftDetails.SetActive(true);
        loanRepayExtra.SetActive(true);
        loanCompanyIcon.sprite = loanCompanyBackgroundAccepted;
        GameManager.UM.Balance += loanAmount;
                
        weeksLeftPast = 0;
        weeksLeftDue = loanTerm;
        loanAmountLeft = loanAmount;
        loanInterestLeft = (int)(loanAmount * (loanInterest / 100f));
        loanAmountDue = loanAmount /  loanTerm;
        totalLoanLeft = loanAmountLeft + loanInterestLeft;
        
        weeksLeftPastText.text = weeksLeftPast.ToString();
        weeksLeftDueText.text = weeksLeftDue.ToString();
        weeksLeftSlider.minValue = 0;
        weeksLeftSlider.maxValue = weeksLeftDue;
        loanAmountLeftText.text = loanAmountLeft + " ₴";
        loanInterestLeftText.text = "+ " + loanInterestLeft + " ₴";
        totalLoanLeftText.text = totalLoanLeft + " ₴";
    }

    public void UpdateRepayLoan()
    {
        weeksLeftPast++;
        loanAmountLeft -= loanAmountDue;
        weeksLeftPastText.text = weeksLeftPast.ToString();
        weeksLeftDueText.text = weeksLeftDue.ToString();
    }

    public void CheckValidRepayAmount()
    {
        int input;
        if (repayAmountInput.text == "")
        {
            input = 0;
        }
        else
        {
            input = int.Parse(repayAmountInput.text);
        }

        if (input <= 0)
        {
            repayAmount = 0;
            repayAmountBackground.sprite = repayAmountInvalid;
            return;
        }

        CalculateMaxRepayExtra();
        if (maxRepayAmount <= 0)
        {
            repayAmount = 0;
            repayAmountBackground.sprite = repayAmountInvalid;
        }
        else
        {
            repayAmount = Mathf.Min(input, maxRepayAmount);
            repayAmountBackground.sprite = repayAmountValid;
        }

        repayAmountInput.text = repayAmount.ToString();
    }

    private void CalculateMaxRepayExtra()
    {
        // can't repay more than what's owed, or more than the player has
        maxRepayAmount = Mathf.Min(totalLoanLeft, (int)GameManager.UM.Balance);
    }

    public void IncreaseRepayExtra()
    {
        CalculateMaxRepayExtra();
        repayAmount = Mathf.Min(repayAmount + repayAmountStep, maxRepayAmount);
        repayAmountInput.text = repayAmount.ToString();
        CheckValidRepayAmount();
    }

    public void DecreaseRepayExtra()
    {
        repayAmount = Mathf.Max(repayAmount - repayAmountStep, 0);
        repayAmountInput.text = repayAmount > 0 ? repayAmount.ToString() : "";
        CheckValidRepayAmount();
    }

    public void RepayLoanExtra()
    {
        int amountToRepay = Mathf.Min(repayAmount, totalLoanLeft, (int)GameManager.UM.Balance);
        if (amountToRepay <= 0) return;

        GameManager.UM.Balance -= amountToRepay;
        loanAmountLeft -= amountToRepay;

        loanInterestLeft = (int)(loanAmountLeft * (loanInterest / 100f));
        totalLoanLeft = loanAmountLeft + loanInterestLeft;

        loanAmountLeftText.text = loanAmountLeft + " ₴";
        loanInterestLeftText.text = "+ " + loanInterestLeft + " ₴";
        totalLoanLeftText.text = totalLoanLeft + " ₴";

        ResetRepayExtra();

        if (totalLoanLeft <= 0)
        {
            loanLeftDetails.SetActive(false);
            loanRepayExtra.SetActive(false);
            loanUnavailable.SetActive(true);
        }
    }

    private void ResetRepayExtra()
    {
        repayAmount = 0;
        repayAmountInput.text = "";
        repayAmountBackground.sprite = repayAmountInvalid;
    }
}
