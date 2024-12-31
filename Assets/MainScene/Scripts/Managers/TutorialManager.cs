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
                    questMenuImage.color = new Color(0.8f, 1.0f, 0.4f, 0.8f);
                    StartCoroutine(AnimateQuestCompletedIcon(2f));
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
        questCompleted = false;
    }

    private IEnumerator AnimateQuestCompletedIcon(float duration)
    {
        float elapsedTime = 0f;
        Vector3 originalScale = questCompletedIcon.transform.localScale;
        Vector3 targetScale = originalScale;
        while (elapsedTime < duration)
        {
            questCompletedIcon.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        questCompletedIcon.transform.localScale = originalScale;
        yield return new WaitForSeconds(0.5f);
        questCompletedIcon.transform.localScale = originalScale;
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
        }
        tutorialCount++;
        switch (tutorialCount)
        {
            case 1:
                GameManager.UM.OpenUIMenu();
                UpdateQuest("Construct a floating island!", "Lets start off with getting some land up here since crops don't grow in the clouds. Press your B key to go into build mode and hover over an island. Now hold down your left mouse button and wait until the island is constructed. Your first island will be free but even up here buying land cost money so you better save up.");
                break;
            case 2:
                GameManager.HM.SetStartingHand();
                UpdateQuest("Cultivated your island!", "Now that you have some land it is time to start growing crops. But first off let me give you some tools and crops to help you out. Now in order to plant crops you land will have to be cultivated. This is done by pressing your M key to go into manage mode and by using your mouse drag the cultivator card to your uncultivated land.");
                break;
        }
    }
}
