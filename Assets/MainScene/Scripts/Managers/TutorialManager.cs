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
                    StartCoroutine(ResetQuestCompletedAfterDelay());
                }
                else
                {
                    questCompletedIcon.gameObject.SetActive(false);
                    questMenuImage.color = new Color(0.9f, 0.9f, 0.9f, 0.8f);
                    GameManager.UM.taxAmountText.transform.parent.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
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
                UpdateQuest("Construct a floating island!", "Lets start off with getting some land up here since crops don't grow in the clouds. Press space to go into build mode and hover over an island. Now hold down your right mouse button and wait until the island is constructed. Your first island will be free but even up here buying land cost money so you better save up.");
                break;
            case 2:
                GameManager.UM.OpenUIMenu();
                GameManager.UM.SetUIButtons(true, GameManager.UM.openUIButton);
                GameManager.UM.taxAmountText.transform.parent.GetComponent<Image>().color = Color.green;
                GameManager.UM.dateAmountText.transform.parent.GetComponent<Image>().color = Color.green;
                UpdateQuest("Watch your expenses!", "Your expenses will go up by having more land and by building machines. If you click on your expenses you can see how much you will need to pay and for what. Then if you press on the date you can see when you need to pay so make sure you can cover your expenses at the end of the month or your farm won't survive.");
                break;
            case 3:
                GameManager.HM.SetStartingHand();
                UpdateQuest("Cultivated your island!", "Now that you have some land it is time to start growing crops. But first off let me give you some tools and seeds to help you out. To plant your seeds your land will have to be cultivated first. This is done by pressing your M key to go into manage mode and by using your mouse to drag the cultivator card to your uncultivated land.");
                break;
            case 4:
                UpdateQuest("Water your island!", "As you can see your land has now been cultivated. We however need to do one more thing before we can plant some seeds. Use your mouse again and this time drag your watering can card over your cultivated land. Make sure to keep your plants watered otherwise they won't make it. The more plants you have and the bigger they are the more water the land needs.");
                break;
            case 5:
                UpdateQuest("Choose a plant!", "You are ready to make put your first plant into the ground. The plants I gave you are all good starter plants so you can plant whichever one you want. The difference between some of the p");
                break;
        }
    }
}
