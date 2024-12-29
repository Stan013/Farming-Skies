using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject UIMenu;
    [SerializeField] private GameObject questMenu;
    [SerializeField] private Button openUIButton;
    [SerializeField] private Button openQuestButton;
    [SerializeField] private TMP_Text openUIText;
    [SerializeField] private TMP_Text openQuestText;
    [SerializeField] private TMP_Text questTitleText;
    [SerializeField] private TMP_Text questInfoText;
    [SerializeField] private TMP_Text taxAmountText;
    [SerializeField] private TMP_Text balanceAmountText;
    [SerializeField] private TMP_Text waterAmountText;
    [SerializeField] private TMP_Text cardAmountText;
    [SerializeField] private TMP_Text islandBuildCostText;
    [SerializeField] private TMP_Text dateAmountText;

    //UI On-screen
    public float tax;
    public float balance;
    public float water;

    //UI Off-screen
    private bool UIActive = false;
    private bool questActive = false;
    private float rentPercentage;
    private bool questCompleted;
    public bool QuestCompleted
    {
        get { return questCompleted; }
        set
        {
            if (questCompleted != value)
            {
                questCompleted = value;
                OnQuestCompletedChanged();
            }
        }
    }

    //UI Island Build
    [SerializeField] private Slider transparencySlider;
    public Image constructionLabel;

    private void OnQuestCompletedChanged()
    {
        if (questMenu != null)
        {
            Image questMenuImage = questMenu.GetComponent<Image>();
            if (questMenuImage != null)
            {
                if (questCompleted)
                {
                    questMenuImage.color = new Color(0.8f, 1.0f, 0.4f, 0.8f);
                    StartCoroutine(ResetQuestCompletedAfterDelay(2f));
                }
                else
                {
                    questMenuImage.color = new Color(0.9f, 0.9f, 0.9f, 0.8f);
                }
            }
        }
    }

    private IEnumerator ResetQuestCompletedAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        QuestCompleted = false;
    }

    public void SetUIActive()
    {
        UIMenu.SetActive(true);
        questMenu.SetActive(true);
    }

    public void OpenUIMenu()
    {
        if (!UIActive)
        {
            openUIText.SetText("<");
            openUIButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-500, openUIButton.GetComponent<RectTransform>().anchoredPosition.y);
            UIMenu.SetActive(true);
            UIActive = true;
        }
        else
        {
            openUIText.SetText(">");
            openUIButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-925, openUIButton.GetComponent<RectTransform>().anchoredPosition.y);
            UIMenu.SetActive(false);
            UIActive = false;
        }
    }

    public void OpenQuestMenu()
    {
        if (!questActive)
        {
            openQuestText.SetText("ʌ");
            openQuestButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(openQuestButton.GetComponent<RectTransform>().anchoredPosition.x, 88);
            questMenu.SetActive(true);
            questActive = true;
        }
        else
        {
            openQuestText.SetText("v");
            openQuestButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(openQuestButton.GetComponent<RectTransform>().anchoredPosition.x, 505);
            questMenu.SetActive(false);
            questActive = false;
        }
    }

    public void UpdateUI()
    {
        dateAmountText.SetText(GameManager.TM.UpdateDate());
        taxAmountText.SetText(tax.ToString() + " ₴");
        balanceAmountText.SetText(balance.ToString() + " ₴");
        waterAmountText.SetText(water.ToString() + " L");
        cardAmountText.SetText(GameManager.DM.cardsInDeck.Count.ToString() + " x");
    }

    public void UpdateQuest(string questTitle, string questInfo)
    {
        questTitleText.SetText(questTitle);
        questInfoText.SetText(questInfo);
    }

    public void SetBuildIslandSlider()
    {
        islandBuildCostText.SetText(GameManager.IPM.clickedIsland.islandBuildCost.ToString());
        if (constructionLabel != null)
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector2 anchoredPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(constructionLabel.canvas.transform as RectTransform, mousePosition, constructionLabel.canvas.worldCamera, out anchoredPosition);
            constructionLabel.rectTransform.anchoredPosition = anchoredPosition;
        }
    }

    public void UpdateBuildIslandSlider(Island island)
    {
        if (island.topMat != null && island.bottomMat != null)
        {
            float alphaValue = Mathf.Clamp01(island.topMat.color.a);
            transparencySlider.value = alphaValue;
            if (alphaValue == 1f && island.islandStatus == "Unbought")
            {
                balance -= island.islandBuildCost;
                tax += (island.islandBuildCost / 100 * rentPercentage);
                GameManager.ISM.AddIslandToBought(island);
                island.islandStatus = "Bought";
                constructionLabel.gameObject.SetActive(false);
                GameManager.UM.UpdateUI();
            }
        }
    }

    public void LoadData(GameData data)
    {
        tax = data.tax;
        balance = data.balance;
        water = data.water;
    }

    public void SaveData(ref GameData data)
    {
        data.tax = tax;
        data.balance = balance;
        data.water = water;
    }
}
