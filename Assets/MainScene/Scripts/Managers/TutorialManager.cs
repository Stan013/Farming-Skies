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
            GameManager.IPM.ToggleState(GameManager.GameState.Default, GameManager.GameState.SettingsMode);
        }
        tutorialCount++;
        switch (tutorialCount)
        {
            case 1:
                UpdateQuest("Construct a floating island!", "Lets start off with getting some land up here since crops don't grow in the clouds. Press TAB to go into manage mode and hover over an island. Now hold down your right mouse button and wait until the island is constructed. Your first island will be free but even up here buying land cost money so you better save up.");
                break;
            case 2:
                GameManager.UM.SetUIButtons(true, GameManager.UM.openUIButton);
                GameManager.UM.openUIButton.GetComponent<Image>().color = Color.green;
                GameManager.UM.taxAmountText.transform.parent.GetComponent<Image>().color = Color.green;
                GameManager.UM.dateAmountText.transform.parent.GetComponent<Image>().color = Color.green;
                UpdateQuest("Watch your expenses!", "Your expenses will go up by having more land and by building things. If you click on your expenses you can see how much you will need to pay and for what. Then if you press on the date you can see when you need to pay so make sure you can cover your expenses at the end of the month or your farm won't survive.");
                break;
            case 3:
                GameManager.HM.SetStartingHand();
                Card cardCultivator = GameManager.HM.FindCardInHandById("CardCultivatorUtility");
                cardCultivator.cardBackground.GetComponent<Image>().color = Color.green;
                cardCultivator.GetComponent<CardDrag>().enabled = true;
                UpdateQuest("Cultivate your island!", "Now that you have some land it is time to start growing crops. But first off let me give you some tools and plant cards to help you out. To use these plant cards your land will have to be cultivated and watered first. This is done by making sure you are in manage mode and then by holding down your left mouse button to drag it towards your land.");
                break;
            case 4:
                Card cardWateringCan = GameManager.HM.FindCardInHandById("CardWateringCanUtility");
                cardWateringCan.cardBackground.GetComponent<Image>().color = Color.green;
                cardWateringCan.GetComponent<CardDrag>().enabled = true;
                UpdateQuest("Water your island!", "As you can see your land has now been cultivated. Now we go onto the second step making sure your land is watered. Use your mouse again and this time drag your watering can card over your cultivated land. Using this card will add 50 water to the soil make sure this number doesn't run out our the soil will dry up and your crops will die.");
                break;
            case 5:
                foreach(Card card in GameManager.HM.cardsInHand)
                {
                    if(card.cardType == "PlantSmall" || card.cardType == "PlantMedium" || card.cardType == "PlantBig")
                    {
                        card.cardBackground.GetComponent<Image>().color = Color.green;
                    }
                }
                UpdateQuest("Check your crop needs!", "Besides water your lands will need to have the right nutrients for your crops. These nutrients are nitrogen, phosphorus and potassium each crop has a different need so make sure your soil has plenty of nutrients. Use your mouse and right click on one of your plant cards to see what this crop needs and also check how big it is. Right click again to put the plant card back in your hand.");
                break;
            case 6:
                Card cardGreenBean = GameManager.HM.FindCardInHandById("CardGreenBeanPlant");
                cardGreenBean.cardBackground.GetComponent<Image>().color = Color.green;
                cardGreenBean.GetComponent<CardDrag>().enabled = true;
                UpdateQuest("Plant your first crop!", "Now that you have got your land ready and know what each crop needs and how big they are, it is time to plant. Hover over the highlighted plant card and drag that card over your soil. Now move your mouse to whichever spot and the crop should snap into place. Each land has space for 36 small plots, 9 medium plots or 1 big plot. So be sure to plan your placement carefully");
                break;
            case 7:
                foreach (Card card in GameManager.HM.cardsInHand)
                {
                    if (card.cardType == "PlantSmall" || card.cardType == "PlantMedium" || card.cardType == "PlantBig")
                    {
                        card.cardBackground.GetComponent<Image>().color = Color.green;
                        card.GetComponent<CardDrag>().enabled = true;
                    }
                }
                UpdateQuest("Plant everything you have!", "It seems like you know what you are doing so how about you plant some more. This time you will see that some spots are already taken by the crop you previously used. Don't worry though if you don't have enough space you can always buy more land. You can also rotate your crops by moving your mouse around the spot it has snapped to. But this is just purely for the aesthetic.");
                break;
            case 8:
                GameManager.UM.SetUIButtons(true, GameManager.UM.nextDayButton);
                UpdateQuest("It is time to advance!", "Good job farmer all your crops are in the ground and your land is looking healthy. Press the next day button and watch the harvest from the day go right into your storage. You might see that some of your crops give a different yield this is depending on the size of crop, the plant itself but also the soil quality. Don't worry though the prices of the drops of each crop are adjusted accordingly.");
                GameManager.UM.nextDayButton.GetComponent<Image>().color = Color.green;
                break;
            case 9:
                GameManager.UM.SetUIButtons(true, GameManager.UM.openInventoryButton);
                GameManager.UM.openInventoryButton.GetComponent<Image>().color = Color.green;
                UpdateQuest("Look at your harvest!", "You did the work and got your farm up and running so now it is time to see what rewards you got. As you might have seen it is a new day so press E to go into inventory mode or click on the highlighted icon and see what drops you got from your crops. You can sort these drops by using the tabs at the top of the window. You can also sell these drops if you want or use them to make more plant cards or products.");
                break;
            case 10:
                UpdateQuest("Check your soil health!", "As you can see after that harvest your land is looking a bit rough. This is because the crops you planted are taking nutrients from the soil. There also might be a warning icon if your soil doesn't have enough nutrients or water for the next day. By double clicking on your land with your left mouse button and by using your scroll wheel to zoom in on the sign. You can see the total amount of nutrients and water all the crops are using each day.");
                break;
            case 11:
                UpdateQuest("Move back to your land!", "In order to improve the nutrients in your soil we will have to move above your land again and use some cards on it. Press TAB to go out of manage mode and now you can freely move around your farm with WASD. You can also move UP by pressing SHIFT and DOWN by pressing CTRL. Your WASD movement is based on the direction you are looking in. So try to get above your land so we can improve your soil.");
                break;
            case 12:
                GameManager.UM.SetUIButtons(true, GameManager.UM.openCraftButton);
                GameManager.UM.openCraftButton.GetComponent<Image>().color = Color.green;
                UpdateQuest("Make some cards!", "To add nutrients to the soil we need to use the appropriate fertilizer card. But we first need to craft these cards you already got some fertilizer saved up so you can craft some fertilizer cards. In manage mode click with your left mouse button on the highlighted icon or press your C key to go into crafting mode. Now you can select which card you want and craft them. Each card will cost a certain amount of resources to craft.");
                break;
            case 13:
                GameManager.UM.SetUIButtons(true, GameManager.UM.openQuestButton);
                GameManager.UM.openQuestButton.GetComponent<Image>().color = Color.green;
                UpdateQuest("Craft all the fertilizers!", "As you noticed when inspecting your soil the follwing nutrients where scarce: Nitrogen, Phosphorus and Potassium. So now that your are in crafting mode select each fertilizer and craft 1 of each fertilizer card. Then go back to manage mode and with your left mouse button drag these fertilizer card onto your soil. This should make the warning icon go away and up the yield of your crops again.");
                break;
        }
    }
}