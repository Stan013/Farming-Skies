using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public GameObject questMenu;
    public TMP_Text questTitleText;
    public TMP_Text questInfoText;
    public Image questCompletedIcon;
    public GameObject tutorialMenu;
    public Button nextButton;
    public bool tutorial;
    public int tutorialCount;
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

    private void OnQuestCompletedChanged()
    {
        if (questMenu != null)
        {
            Image questMenuImage = questMenu.GetComponent<Image>();
            if (questMenuImage != null)
            {
                if (questCompleted)
                {
                    questCompletedIcon.gameObject.SetActive(true);
                    questMenuImage.color = new Color(0.8f, 1.0f, 0.4f, 0.8f);
                    GameManager.UM.taxLabel.color = new Color(0.6f, 1f, 0.75f, 1f);
                    StartCoroutine(ResetQuestCompletedAfterDelay());
                }
                else
                {
                    questCompletedIcon.gameObject.SetActive(false);
                    questMenuImage.color = new Color(0.9f, 0.9f, 0.9f, 0.8f);
                    GameManager.UM.taxLabel.color = new Color(1f, 1f, 1f, 1f);
                }
            }
        }
    }

    private IEnumerator ResetQuestCompletedAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        QuestCompleted = false;
        NextQuest();
    }

    public void UpdateQuest(string questTitle, string questInfo)
    {
        questTitleText.SetText(questTitle);
        questInfoText.SetText(questInfo);
    }

    public void StartTutorial()
    {
        tutorialMenu.SetActive(true);
    }

    public void NextQuest()
    {
        if(tutorialCount == 0)
        {
            GameManager.UM.UpdateUI();
            tutorialMenu.SetActive(false);
            GameManager.UM.OpenQuestMenu();
            GameManager.IPM.ToggleState(GameManager.GameState.Default, GameManager.GameState.MenuMode);
            GameManager.ISM.availableIslands[0].ToggleState(Island.IslandState.Highlighted, Island.IslandState.Default);
        }
        tutorialCount++;
        switch (tutorialCount)
        {
            case 1:
                UpdateQuest("Construct a floating island!", "Lets start off with getting some land up here since crops don't grow in the clouds. Press your B key to go into build mode and hover over an island. Now hold down your left mouse button and wait until the island is constructed. Your first island will be free but even up here buying land cost money so you better save up.");
                break;
            case 2:
                GameManager.UM.OpenUIMenu();
                GameManager.UM.SetUIButtons(true, "UI");
                UpdateQuest("Watch your expenses!", "As you can see your expenses go up by having more land and by building machines. If you click on your expenses you can see how much you will need to pay and for what. Then if you press on the date you can see when you need to pay so make sure you cna cover your expenses at the end of the month or your farm won't survive.");
                break;
            case 3:
                GameManager.HM.SetStartingHand();
                UpdateQuest("Cultivated your island!", "Now that you have some land it is time to start growing crops. But first off let me give you some tools and crops to help you out. Now in order to plant crops you land will have to be cultivated. This is done by pressing your M key to go into manage mode and by using your mouse drag the cultivator card to your uncultivated land.");
                break;
        }
    }
}
