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
        yield return new WaitForSeconds(1f);
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
                UpdateQuest("Construct a floating island!", "Lets start off with getting some land up here since crops don't grow in the clouds. Press SPACE to go into manage mode and hover over an island. Now hold down your right mouse button and wait until the island is constructed. Your first island will be free but even up here buying land cost money so you better save up.");
                break;
            case 2:
                GameManager.UM.OpenUIMenu();
                GameManager.UM.SetUIButtons(true, GameManager.UM.openUIButton);
                GameManager.UM.taxAmountText.transform.parent.GetComponent<Image>().color = Color.green;
                GameManager.UM.dateAmountText.transform.parent.GetComponent<Image>().color = Color.green;
                UpdateQuest("Watch your expenses!", "Your expenses will go up by having more land and by building things. If you click on your expenses you can see how much you will need to pay and for what. Then if you press on the date you can see when you need to pay so make sure you can cover your expenses at the end of the month or your farm won't survive.");
                break;
            case 3:
                GameManager.HM.SetStartingHand();
                Card cardCultivator = GameManager.HM.FindCardInHandById("CardCultivatorUtility");
                cardCultivator.cardBackground.GetComponent<Image>().color = Color.green;
                cardCultivator.GetComponent<CardDrag>().enabled = true;
                UpdateQuest("Cultivate your island!", "Now that you have some land it is time to start growing crops. But first off let me give you some tools and plant cards to help you out. To use the plant cards your land will have to be cultivated first. This is done by making sure you are in manage mode and then by using your mouse to drag the cultivator card to your uncultivated land.");
                break;
            case 4:
                Card cardWateringCan = GameManager.HM.FindCardInHandById("CardWateringCanUtility");
                cardWateringCan.cardBackground.GetComponent<Image>().color = Color.green;
                cardWateringCan.GetComponent<CardDrag>().enabled = true;
                UpdateQuest("Water your island!", "As you can see your island has now been cultivated. We however need to do some more things before we can plant. Use your mouse again and this time drag your watering can card over your cultivated land. Make sure to keep your plants watered otherwise they won't make it. The more plants you have and the bigger they are the more water the land needs.");
                break;
            case 5:
                GameManager.UM.waterAmountText.transform.parent.GetComponent<Image>().color = Color.green;
                UpdateQuest("Maintain your water supply!", "Of course your land won't stay watered forever after each month your land will dry up. This is when you need to use a watering can again however throughout the month all you have to do is store enough water. If you press on your water storage you can see how much each land takes and how much you are collecting.");
                break;
            case 6:
                foreach(Card card in GameManager.HM.cardsInHand)
                {
                    if(card.cardType == "PlantSmall" || card.cardType == "PlantMedium" || card.cardType == "PlantBig")
                    {
                        card.cardBackground.GetComponent<Image>().color = Color.green;
                    }
                }
                UpdateQuest("Check your plants needs!", "Besides water your lands will need to have the right minerals for your plants. These minerals are nitrogen, phosphorus and potassium each plant has a different need so make sure your land has that. Use your mouse and right click on one of your plant cards to see what this plant needs and also check how big it is. Right click again to put the plant back in your hand.");
                break;
            case 7:
                Card cardPeanut = GameManager.HM.FindCardInHandById("CardPeanutPlant");
                cardPeanut.cardBackground.GetComponent<Image>().color = Color.green;
                cardPeanut.GetComponent<CardDrag>().enabled = true;
                UpdateQuest("Plant your first plant!", "Now that you have got your land ready and know what each plant needs and how big they are, it is time to plant. Hover over the plant card you have chosen and hold left mouse button. Now move your mouse to whichever spot you want to plant there a 36 spaces for small plants, 9 for medium plants and 1 space for a big plant. So make sure your plant snaps to the right spot.");
                break;
            case 8:
                foreach (Card card in GameManager.HM.cardsInHand)
                {
                    if (card.cardType == "PlantSmall" || card.cardType == "PlantMedium" || card.cardType == "PlantBig")
                    {
                        card.cardBackground.GetComponent<Image>().color = Color.green;
                        card.GetComponent<CardDrag>().enabled = true;
                    }
                }
                UpdateQuest("Plant everything you have!", "It seems like you know what you are doing so how about you plant some more. This time you will see that some spots are already taken by the plant you previously used. Don't worry though if you don't have enough space you can always buy more land. You can also rotate your plants by moving your mouse around the spot your plant has snapped to. ");
                break;
            case 9:
                GameManager.UM.SetUIButtons(true, GameManager.UM.nextDayButton);
                UpdateQuest("It is time to advance!", "Good job farmer all your plants are in the ground and your land is looking healthy. Press the next day button and watch the harvest from that day go into your storage. You will of course also see you water storage and your fertilizer storage go down but no worries this will be the next thing I will tell you about.");
                GameManager.UM.nextDayButton.GetComponent<Image>().color = Color.green;
                break;
            case 10:
                GameManager.UM.SetUIButtons(true, GameManager.UM.openInventoryButton);
                UpdateQuest("Look at your harvest!", "You did the work and got your farm up and running so now it is time to see what rewards you got. As you might have seen it is a new day so press TAB to go into inventory mode and see at what you got from your plants. You can sort these drops by using the tabs at the top of the window. You can also sell these drops if you want or use them to make more plants or products.");
                break;
            case 11:
                UpdateQuest("Check your land health!", "");
                break;
            case 12:
                UpdateQuest("Improve your yield!", "");
                break;
        }
    }
}