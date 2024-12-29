using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialMenu;
    public Button nextButton;
    public bool tutorial;
    public int tutorialCount;

    public void StartTutorial()
    {
        tutorialMenu.SetActive(true);
    }

    public void NextQuest()
    {
        if(tutorialCount == 0)
        {
            tutorialMenu.SetActive(false);
            GameManager.UM.OpenQuestMenu();
        }
        tutorialCount++;
        switch (tutorialCount)
        {
            case 1:
                GameManager.UM.UpdateQuest("Construct a floating island!", "Lets start off with getting some land up here since crops don't grow in the clouds. Press your B key to go into build mode and hover over an island. Now hold down your left mouse button and wait until the island is constructed. Your first island will be free but even up here buying land cost money so you better save up.");
                break;
            case 2:
                GameManager.HM.SetStartingHand();
                GameManager.UM.UpdateQuest("Cultivated your island!", "Now that you have some land it is time to start growing crops. But first off let me give you some tools and crops to help you out. Now in order to plant crops you land will have to be cultivated. This is done by pressing your M key to go into manage mode and by using your mouse drag the cultivator card to your uncultivated land.");
                break;
        }
    }
}
