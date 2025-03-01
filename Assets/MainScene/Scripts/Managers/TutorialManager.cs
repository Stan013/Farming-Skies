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
                UpdateQuest("Construct a floating island!", "Lets start off with getting some land up here since crops don't grow in the clouds. Press <b>TAB</b> to go into manage mode and hover over an island. Now hold down your <b>right mouse button</b> and wait until the island is constructed. Your first island will be free but even up here buying land cost money so you better save up.");
                tutorialCount = 15; //Testing
                break;
            case 2:
                GameManager.UM.SetUIButtons(true, GameManager.UM.openUIButton);
                GameManager.UM.openUIButton.GetComponent<Image>().color = Color.green;
                GameManager.UM.taxAmountText.transform.parent.GetComponent<Image>().color = Color.green;
                GameManager.UM.dateAmountText.transform.parent.GetComponent<Image>().color = Color.green;
                UpdateQuest("Watch your expenses!", "Your expenses will go up by having more land and by building things. Open your UI menu by <b>left clicking</b> on the green button on the left side off your screen. Now <b>left click</b> on your expenses and you will be able to see how much you need to pay and for what. Then if you <b>left click</b> on the date you can see when you need to pay to make sure your farm survives.");
                break;
            case 3:
                GameManager.HM.SetStartingHand();
                Card cardCultivator = GameManager.HM.FindCardInHandById("CardCultivatorUtility");
                cardCultivator.GetComponent<Image>().color = Color.green;
                cardCultivator.GetComponent<CardDrag>().enabled = true;
                UpdateQuest("Cultivate your island!", "Now that you have some land it is time to start growing crops. But first off let me give you some tools and plant cards to help you out. To use these plant cards your land will have to be cultivated and watered first. This is done by making sure you are in manage mode and then by <b>holding down left mouse button</b> over the highlighted card to <b>drag</b> it towards your land.");
                break;
            case 4:
                Card cardWateringCan = GameManager.HM.FindCardInHandById("CardWateringCanUtility");
                cardWateringCan.GetComponent<Image>().color = Color.green;
                cardWateringCan.GetComponent<CardDrag>().enabled = true;
                UpdateQuest("Water your island!", "As you can see your land has now been cultivated. Now we go onto the second step making sure your land is watered. Use your <b>left mouse button</b> again and this time <b>drag</b> your watering can card over your cultivated land. Using this card will add <color=blue><b>50 water</b></color> to the soil make sure this number doesn't run out our the soil will dry up and your crops will die.");
                break;
            case 5:
                foreach(Card card in GameManager.HM.cardsInHand)
                {
                    if(card.cardType == "PlantSmall" || card.cardType == "PlantMedium" || card.cardType == "PlantBig")
                    {
                        card.GetComponent<Image>().color = Color.green;
                    }
                }
                UpdateQuest("Check your crop needs!", "Besides water your lands will need to have the right nutrients for your crops. These nutrients are <color=orange><b>Nitrogen (N)</b></color>, <color=green><b>Phosphorus (P)</b></color> and <color=red><b>Potassium (K)</b></color> each crop has a different need so make sure your soil has plenty of nutrients. Use your mouse and <b>right click</b> on one of your plant cards to see what this crop needs and also check how big it is. <b>Right click</b> again to put the plant card back in your hand.");
                break;
            case 6:
                Card cardGreenBean = GameManager.HM.FindCardInHandById("CardGreenBeanPlant");
                cardGreenBean.GetComponent<Image>().color = Color.green;
                cardGreenBean.GetComponent<CardDrag>().enabled = true;
                UpdateQuest("Plant your first crop!", "Now that you have got your land ready and know what each crop needs and how big they are, it is time to plant. Hover over the highlighted plant card and by <b>holding left mouse button</b> you can <b>drag</b> that card over your soil. Now move your mouse to whichever spot and the crop should snap into place. Each land has space for <b>36 small plots, 9 medium plots or 1 big plot</b>. So be sure to plan your placement carefully");
                break;
            case 7:
                foreach (Card card in GameManager.HM.cardsInHand)
                {
                    if (card.cardType == "PlantSmall" || card.cardType == "PlantMedium" || card.cardType == "PlantBig")
                    {
                        card.GetComponent<Image>().color = Color.green;
                        card.GetComponent<CardDrag>().enabled = true;
                    }
                }
                UpdateQuest("Plant everything you have!", "It seems like you know what you are doing so how about you plant some more. This time you will see that some spots are already taken by the crop you previously used. Don't worry though if you don't have enough space you can always buy more land. You can also rotate your crops by moving your mouse around the spot it has snapped to. But this is just purely for the aesthetic.");
                break;
            case 8:
                GameManager.UM.SetUIButtons(true, GameManager.UM.nextDayButton);
                UpdateQuest("It is time to advance!", "Good job farmer all your crops are in the ground and your land is looking healthy. <b>Click</b> with your <b>left mouse button</b> on the next day and watch the harvest from the day go right into your storage. You might see that some of your crops give a different yield this is depending on the size of crop, the plant itself but also the soil quality. Each nutrient need that is met gives you a <b>80%</b> chance to get 1,2,3 or 4 extra drops based on the plants base yield.");
                GameManager.UM.nextDayButton.GetComponent<Image>().color = Color.green;
                break;
            case 9:
                GameManager.UM.SetUIButtons(true, GameManager.UM.openInventoryButton);
                GameManager.UM.openInventoryButton.GetComponent<Image>().color = Color.green;
                UpdateQuest("Look at your harvest!", "You did the work and got your farm up and running so now it is time to see what you got from it. It is a new day so <b>press E</b> to go into inventory mode or in manage mode <b>click</b> on the green icon at the top right of your screen and see what drops you got from your crops. You can sort these drops by using the tabs at the top of the window. As you can see you can sell and craft as well but we will get to that for now exit inventory mode by <b>pressing E</b> or <b>click</b> the red close button");
                break;
            case 10:
                UpdateQuest("Check your soil health!", "As you can see after that harvest your land is looking a bit rough. This is because the crops you planted are taking nutrients from the soil. There also might be a warning icon if your soil doesn't have enough nutrients or water for the next day. By <b>double clicking</b> on your land with your <b>left mouse button</b> and by using your <b>scroll wheel</b> to zoom in on the sign. You can see the total amount of nutrients and water all the crops are using each day.");
                break;
            case 11:
                UpdateQuest("Move back to your land!", "In order to improve the nutrients in your soil we will have to move above your land again and use some cards on it. Press <b>TAB</b> to go out of manage mode and now you can freely move around your farm with your <b>WASD</b> keys. You can also move <b>UP</b> by pressing your <b>SHIFT</b> key and <b>DOWN</b> by pressing your <b>CTRL</b> key. Your WASD movement is based on the direction you are looking in. So try to get above your land so we can improve your soil.");
                break;
            case 12:
                GameManager.UM.SetUIButtons(true, GameManager.UM.openCraftButton);
                GameManager.UM.openCraftButton.GetComponent<Image>().color = Color.green;
                UpdateQuest("It is crafting time!", "To add nutrients to the soil we need to use the appropriate fertilizer card. But we first need to craft these cards you already got some fertilizer saved up so you can craft some fertilizer cards. In manage mode <b>click</b> on the green icon again or press your <b>C key</b> to go into crafting mode. Open up your quest menu again by <b>left clicking</b> the green button at the top of your screen and start crafting the cards that you need.");
                break;
            case 13:
                GameManager.UM.SetUIButtons(true, GameManager.UM.openQuestButton);
                UpdateQuest("Select nitrogen fertilizer!", "As you noticed when inspecting your soil the following nutrient was scarce <color=orange><b>Nitrogen (N)</b></color>. So now that your are in crafting mode select the green fertilizer card with the use of the left and right buttons. Once you have selected the right card the craft button will show up as invalid amount. This is because you need to <b>click</b> the plus button so you set how many cards you want to craft.");
                break;
            case 14:
                UpdateQuest("Craft the fertilizer card!", "Now that you have selected the right card and put in the right crafting amount the craft button should show up. You can only craft 1 <color=orange><b>Nitrogen (N)</b></color> fertilizer card currently however when you are crafting more cards your cost will update to the total crafting cost. To fully craft a card you <b>hold down left mouse button</b> on the craft button and watch as the card gets made. Once finished the card should be fully revealed and the craft button should say <b>card crafted.</b>");
                break;
            case 15:
                GameManager.UM.water += 5;
                GameManager.UM.fertilizer += 50;
                GameManager.CRM.HighlightCards();
                UpdateQuest("Craft some more!", "Okay lets craft the other fertilizer cards you will need as well. I gave you some extra resoucres so you should be able to craft them now. Selected the other green fertilizer cards <color=green><b>Phosphorus (P)</b></color> plus <color=red><b>Potassium (K)</b></color> and craft 1 of each. Once you have crafted these card exit crafting mode by <b>pressing C</b> again or <b>click</b> the red close button. Now you should have all the cards needed to improve your soil and get the yield of your crops up again.");
                break;
            case 16:
                GameManager.DM.AddCardToDeck("CardCultivatorUtility"); //Testing
                GameManager.DM.AddCardToDeck("CardWateringCanUtility"); //Testing
                GameManager.DM.AddCardToDeck("CardGreenBeanPlant"); //Testing
                GameManager.DM.AddCardToDeck("CardChivePlant"); //Testing
                GameManager.UM.SetUIButtons(true, GameManager.UM.openUIButton); //Testing
                GameManager.UM.SetUIButtons(true, GameManager.UM.nextDayButton); //Testing
                GameManager.DM.CheckRefillHand();
                UpdateQuest("Improve your soil!", "With these newly crafted fertilizer cards it is time to improve your soil. <b>Drag</b> each of your crafted fertilizer cards towards the soil and it will add <b>50</b> of that nutrient to it. Than inspect your land by <b>double clicking</b> and check if all the nutrients are available again. The warning icon should also disappear since your crops needs are met for the next day. If you don't meet these needs the same as previously you have a <b>80%</b> chance to lose 1,2,3 or 4 drops based on the plants base yield.");
                break;
            case 17:
                GameManager.UM.SetUIButtons(true, GameManager.UM.openMarketButton);
                GameManager.UM.openMarketButton.GetComponent<Image>().color = Color.green;
                UpdateQuest("Time to earn!", "Your soil has been improved and is ready for another harvest but lets first see what we can do with the previous harvest. In manage mode <b>click</b> on the green icon again or press your <b>Q key</b> to go into market mode. Here you will see all the drops you can sell, the demand and supply and the prices of this drop in the last few days. Each day the supply and demand can change which will of course result into a different price. So sometimes waiting to sell some of your harvest might turn out to be more profitable.");
                break;
            case 18:
                UpdateQuest("Learn the market!", "To earn money you will need to sell your harvest from today. As you can see the prices are currently all the same but this will change each day. A new day means a new demand/supply which correlates to a different price. More demand equals a higher price but more supply means a lower price. You can also see what the lowest and highest price was each week. <b>Scroll</b> down and try to find the market for <b>Rice</b>. Then <b>click</b> the highlighted max button or input the max amount of rice you have.");
                break;
            case 19:
                UpdateQuest("Let's make so money!", "Later on you can of course sell and buy however much you want. You can even buy when the price is low and sell when the price is high. But who knows what the next day will hold big supply and demand changes are less likely to happen. But if they do happen the price can change quite a bit so for now let's just take some guaranteed profit. Sell all the rice your have harvest by <b>clicking</b> the sell button and watch your money go up.");
                break;
            case 20:
                UpdateQuest("Let's expand!", "Because of the cards you have crafted your fertilizer and your water storage is low so lets try to fill those up again. First of buy another island by going into manage mode and <b>holding right click.</b>. Now ");
                break;
        }
    }
}