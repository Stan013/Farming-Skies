using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public TMP_Text tutorialTitleText;
    public TMP_Text tutorialInfoText;
    public GameObject startTutorialMenu;
    public Button nextButton;
    public bool tutorial;
    public int tutorialCount;
    public Card cardGreenBean;
    public Card cardChive;
    public Card cardChard;
    public Card cardRice;

    public void UpdateTutorial(string tutorialTitle, string tutorialInfo)
    {
        tutorialTitleText.SetText(tutorialTitle);
        tutorialInfoText.SetText(tutorialInfo);
    }

    public void StartTutorial()
    {
        startTutorialMenu.SetActive(true);
    }

    public void NextTutorial()
    {
        if(tutorialCount == 0)
        {
            GameManager.UM.UpdateUI();
            startTutorialMenu.SetActive(false);
            GameManager.UM.questButton.OpenQuestMenu();
            GameManager.IPM.ToggleState(GameManager.GameState.Default, GameManager.GameState.SettingsMode);
            GameManager.ISM.FindIslandByID("Starter(0,0)").ToggleState(Island.IslandState.Highlighted, Island.IslandState.Default);
        }
        tutorialCount++;
        switch (tutorialCount)
        {
            case 1:
                GameManager.IPM.manageModeEnabled = true;
                UpdateTutorial("Manage your islands!", "Press Q to go into manage mode. In this mode you can build, inspect and manage your farm. You can see which mode you are in on the bottom right.");
                break;
            case 2:
                GameManager.IPM.defaultModeEnabled = false;
                UpdateTutorial("Build your first island!", "Hover with your mouse over the highlighted island and hold down your right mouse button until the island is built.");
                break;
            case 3:
                GameManager.UM.SetUIButtons(true, GameManager.UM.UIbutton.GetComponent<Button>());
                GameManager.UM.UIbutton.GetComponent<Image>().color = Color.green;
                UpdateTutorial("Look at your stats!", "Use your mouse and click on the highlighted green button on the left side of your screen. To see all the stats of your farm.");
                break;
            case 4:
                GameManager.UM.expenseAmountText.transform.parent.GetComponent<Image>().color = Color.green;
                UpdateTutorial("Check your expenses!", "Now, use your mouse again and left-click on the green label that shows your expenses. You can see what you need to pay for.");
                break;
            case 5:
                GameManager.UM.dateAmountText.transform.parent.GetComponent<Image>().color = Color.green;
                UpdateTutorial("Check upcoming events!", "Do the same thing again and left click on the green label that shows the date. Here you can see all the events each month.");
                break;
            case 6:
                GameManager.HM.SetStartingHand();
                Card cardCultivator = GameManager.HM.FindCardInHandById("CardCultivatorUtility");
                cardCultivator.GetComponent<Image>().color = Color.green;
                cardCultivator.GetComponent<CardDrag>().enabled = true;
                UpdateTutorial("Start cultivating!", "It is time to start getting your land ready for planting. Hover over the green highlighted card, hold the left mouse button down, and move towards your land.");
                break;
            case 7:
                Card cardWateringCan = GameManager.HM.FindCardInHandById("CardWateringCanUtility");
                cardWateringCan.GetComponent<Image>().color = Color.green;
                cardWateringCan.GetComponent<CardDrag>().enabled = true;
                UpdateTutorial("Water your soil!", "Your land has been cultivated now, let's water it so we can plant it. Hover over the green highlighted card again, hold the left mouse button down, and move towards your land.");
                break;
            case 8:
                cardGreenBean = GameManager.HM.FindCardInHandById("CardGreenBeanPlant");
                cardGreenBean.GetComponent<Image>().color = Color.green;
                cardGreenBean.GetComponent<CardInspect>().enabled = true;
                UpdateTutorial("Inspect your plant!", "Your land is ready for planting, but first let's see what crops you have and what they need. Click with your right mouse button on the green highlighted card and click again to close.");
                break;
            case 9:
                cardChive = GameManager.HM.FindCardInHandById("CardChivePlant");
                cardChive.GetComponent<Image>().color = Color.green;
                cardChive.GetComponent<CardInspect>().enabled = true;
                UpdateTutorial("Inspect another!", "Right click with your mouse on the other green highlighted card. And as you can see, the plant size, water, and nutrient needs are different. This will result in a different yield.");
                break;
            case 10  :
                cardChard = GameManager.HM.FindCardInHandById("CardChardPlant");
                cardRice = GameManager.HM.FindCardInHandById("CardRicePlant");
                cardChard.GetComponent<Image>().color = Color.green;
                cardRice.GetComponent<Image>().color = Color.green;
                cardChard.GetComponent<CardInspect>().enabled = true;
                cardRice.GetComponent<CardInspect>().enabled = true;
                UpdateTutorial("Compare all of them!", "I hope you noticed the difference now, inspect the remaining plants. Each month, the nutrients in your soil will refill, so making good combinations can be very beneficial.");
                break;
            case 11:
                cardGreenBean.GetComponent<Image>().color = Color.green;
                cardGreenBean.GetComponent<CardDrag>().enabled = true;
                UpdateTutorial("Plant your first crop!", "Let's hover over the green highlighted card and this time hold down your left mouse button. Then move the plant towards your soil and once snapped in place let go of your mouse.");
                break;
            case 12:
                cardChive.GetComponent<Image>().color = Color.green;
                cardChive.GetComponent<CardDrag>().enabled = true;
                UpdateTutorial("Plant another crop!", "Now do it again with the other green highlighted card. Since the plant size is smaller, more plots available. You can however only have one plant for each plot.");
                break;
            case 13:
                cardChard.GetComponent<Image>().color = Color.green;
                cardRice.GetComponent<Image>().color = Color.green;
                cardChard.GetComponent<CardDrag>().enabled = true;
                cardRice.GetComponent<CardDrag>().enabled = true;
                UpdateTutorial("Plant all your crops!", "You are well on your way to becoming a farmer, so let's plant your last crops as well. With the last 2 highlighted plant cards, do the same drag motions as before.");
                break;
            case 14:
                GameManager.IPM.timeModeEnabled = true;
                UpdateTutorial("Time to advance!", "If you are ready, hold down your space bar and wait until the bar on the top right fills up. Now watch as the week goes by and let your crops do the work.");
                break;
                /*case 8:
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
                    //GameManager.DM.AddCardToDeck("CardCultivatorUtility"); //Testing
                    //GameManager.DM.AddCardToDeck("CardWateringCanUtility"); //Testing
                    //GameManager.DM.AddCardToDeck("CardGreenBeanPlant"); //Testing
                    //GameManager.DM.AddCardToDeck("CardChivePlant"); //Testing
                    //GameManager.DM.AddCardToDeck("CardChardPlant"); //Testing
                    //GameManager.UM.SetUIButtons(true, GameManager.UM.openUIButton); //Testing
                    //GameManager.UM.SetUIButtons(true, GameManager.UM.nextDayButton); //Testing
                    //GameManager.DM.CheckRefillHand(); //Testing
                    UpdateQuest("Improve your soil!", "With these newly crafted fertilizer cards it is time to improve your soil. <b>Drag</b> each of your crafted fertilizer cards towards the soil and it will add <b>50</b> of that nutrient to it. Than inspect your land by <b>double clicking</b> and check if all the nutrients are available again. The warning icon should also disappear since your crops needs are met for the next day. If you don't meet these needs the same as previously you have a <b>80%</b> chance to lose 1,2,3 or 4 drops based on the plants base yield.");
                    break;
                case 17:
                    GameManager.UM.SetUIButtons(true, GameManager.UM.openMarketButton);
                    GameManager.UM.openMarketButton.GetComponent<Image>().color = Color.green;
                    UpdateQuest("Time to earn!", "Your soil has been improved and is ready for another harvest but lets first see what we can do with the previous harvest. In manage mode <b>click</b> on the green icon again or press your <b>Q key</b> to go into market mode. Here you will see all the items you can sell, the demand and supply and the prices of this item in the last few days. You won't see all the items in the game just yet since you need to unlock these crops first. You will however see every product that can be made with your items.");
                    break;
                case 18:
                    UpdateQuest("Learn the market!", "To earn money you will need to sell your harvest from today. As you can see the prices are currently all the same but this will change each day. A new day means a new demand and a new supply. More demand equals a higher price but more supply means a lower price. You can also see what the lowest and highest price was each week. <b>Scroll</b> down and try to find the market for green beans. Then <b>click</b> the highlighted max button or input the max amount of green beans you have.");
                    break;
                case 19:
                    UpdateQuest("Let's make so money!", "The market allows you to buy and sell items but do be mindful since the market fluctuates. Big fluctuations in supply and demand are less likely but can still happen. A increase in demand will make the price go up but a increase in supply will make the price go down. But for now let's just make some money and sell the green beans you got. <b>Click</b> the sell button and watch your money go up. You don't always have to sell your harvest immediately something holding on to it can earn you more.");
                    break;
                case 20:
                    GameManager.ISM.FindIslandByID("Ring1(-1,0)").ToggleState(Island.IslandState.Highlighted, Island.IslandState.Default);
                    UpdateQuest("Expand some more!", "You are almost ready to start your farming empire. The last thing you need to learn is how to refill your storages as crafting requires water and fertilizer. This is done by buildables but lets first start off with buying another island now that you got the money for it. Go into manage mode again and then <b>hover</b> over the highlighted island. Now remember <b>hold down</b> your <b>right mouse button</b> until the island has fully been build.");
                    break;
                case 21:
                    UpdateQuest("Time to build!", "You are almost ready to start your farming empire. The last thing you need to learn is how to refill your storages as crafting requires water and fertilizer. This is done by buildables but lets first start off with buying another island now that you got the money for it. Make sure you are in manage mode again and then <b>hover</b> over the highlighted island. Now remember <b>hold down</b> your <b>right mouse button</b> until the island has fully been build.");
                    break;*/
        }
    }
}