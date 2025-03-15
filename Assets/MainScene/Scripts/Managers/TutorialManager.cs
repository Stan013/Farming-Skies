using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public Island expansionIsland;
    public Card cardWaterBarrel;
    public Card cardCompost;
    public MarketItem riceMarketItem;

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
                UpdateTutorial("Look at your resources!", "Use your mouse and click on the arrow at the left side of your screen. To see all your resources, the date and how many cards you have in your deck.");
                break;
            case 4:
                GameManager.UM.expenseAmountText.transform.parent.GetComponent<Image>().color = new Color(0.68f, 0.9f, 0.5f);
                UpdateTutorial("Check your expenses!", "Now, use your mouse again and left click on the green label that shows your expenses. This shows what you need to pay for.");
                break;
            case 5:
                GameManager.UM.dateAmountText.transform.parent.GetComponent<Image>().color = new Color(0.68f, 0.9f, 0.5f);
                UpdateTutorial("Check upcoming events!", "Do the same thing again and left click on the green label that shows the date. Here you can see all the events each month.");
                break;
            case 6:
                GameManager.UM.UIbutton.GetComponent<OpenUIButton>().OpenUIMenu();
                GameManager.HM.SetStartingHand();
                Card cardCultivator = GameManager.HM.FindCardInHandById("CardCultivatorUtility");
                cardCultivator.GetComponent<CardDrag>().enabled = true;
                UpdateTutorial("Start cultivating!", "It is time to start getting your land ready for planting. Hover over the cultivator card and hold down your left mouse button. Now move towards your land to cultivate it.");
                break;
            case 7:
                Card cardWateringCan = GameManager.HM.FindCardInHandById("CardWateringCanUtility");
                cardWateringCan.GetComponent<CardDrag>().enabled = true;
                UpdateTutorial("Water your land!", "Your land has been cultivated, let's water it so we can start planting. Hover over the watering can card and hold down your left mouse button again. Now move towards your land to water it.");
                break;
            case 8:
                cardGreenBean = GameManager.HM.FindCardInHandById("CardGreenBeanPlant");
                cardGreenBean.GetComponent<CardInspect>().enabled = true;
                UpdateTutorial("Inspect your plant!", "Your land is ready for planting, but first let's see what crops you have and what they need. Click with your right mouse button on the green bean plant card and click again to put the card away.");
                break;
            case 9:
                cardChive = GameManager.HM.FindCardInHandById("CardChivePlant");
                cardChive.GetComponent<CardInspect>().enabled = true;
                UpdateTutorial("Inspect another!", "Right click again with your mouse on the chive plant card this time. And as you can see, the plant size, water, and nutrient needs are different. This will result in a different yield.");
                break;
            case 10  :
                cardChard = GameManager.HM.FindCardInHandById("CardChardPlant");
                cardRice = GameManager.HM.FindCardInHandById("CardRicePlant");
                cardChard.GetComponent<CardInspect>().enabled = true;
                cardRice.GetComponent<CardInspect>().enabled = true;
                UpdateTutorial("Compare all of them!", "I hope you noticed the difference, so now inspect the remaining plant cards. Each month, the nutrients in your land will refill, so making good combinations can be very beneficial.");
                break;
            case 11:
                cardGreenBean.GetComponent<CardDrag>().enabled = true;
                UpdateTutorial("Plant your first crop!", "Let's hover over your green bean plant card, but this time hold down your left mouse button. Then move the plant towards your land and once snapped in place let go of your mouse.");
                break;
            case 12:
                cardChive.GetComponent<CardDrag>().enabled = true;
                UpdateTutorial("Plant another crop!", "Now do it again with your chive plant card. Since the plant size is smaller, more plots are available. You can, however, only have one plant for each plot, so use your space efficiently.");
                break;
            case 13:
                cardChard.GetComponent<CardDrag>().enabled = true;
                cardRice.GetComponent<CardDrag>().enabled = true;
                UpdateTutorial("Plant all your crops!", "You are well on your way to becoming a farmer, so let's plant your last plant cards as well. With the last two plant cards in your hand, do the same drag motions as before.");
                break;
            case 14:
                GameManager.IPM.timeModeEnabled = true;
                UpdateTutorial("Time to advance!", "If you are ready, hold down your space bar and wait until the bar on the top right fills up. Now watch as the week goes by and let your crops do the work.");
                break;
            case 15:
                GameManager.IPM.timeModeEnabled = false;                
                GameManager.IPM.inventoryModeEnabled = true;
                GameManager.UM.SetUIButtons(true, GameManager.UM.questButton.GetComponent<Button>());
                UpdateTutorial("Look at your spoils!", "You watched your harvest, now open up your inventory by pressing E and find out how much you got. Your yield can increase or decrease based on whether the crops' needs are met.");
                break;
            case 16:
                UpdateTutorial("Inspect your land!", "Go out of inventory mode by pressing E and notice that the quality of your land has changed. A warning icon has appeared as well, let's press Q to go into manage mode and see what's up.");
                break;
            case 17:
                GameManager.IPM.inventoryModeEnabled = false;
                GameManager.IPM.islandInspectEnabled = true;
                UpdateTutorial("Select your land!", "With your mouse hover over the land that has a warning icon on top of it. Now click with your left mouse button and an information panel should appear on the right side of your screen.");
                break;
            case 18:
                UpdateTutorial("Check your crop needs!", "You can see how much water and nutrients your crops require. Don't run out of water because your land will dry up. Now click with your left mouse button on the green switch icon.");
                break;
            case 19:
                UpdateTutorial("Your island reserves!", "It now shows how much your land still has. Some of these reserves are too low for the next harvest so let's help your land out. With your left mouse button click on the red close icon.");
                break;
            case 20:
                GameManager.IPM.islandInspectEnabled = false;
                GameManager.IPM.craftModeEnabled = true;
                UpdateTutorial("Open craft mode!", "First we need some cards to use, so open the crafting menu by pressing C. Here you can craft any card you need. You can filter the different card types with the green labels on top.");
                break;
            case 21:
                GameManager.IPM.craftModeEnabled = false;
                GameManager.CRM.craftUI.plusButton.enabled = false;
                GameManager.CRM.craftUI.maxButton.enabled = false;
                UpdateTutorial("Find nitrogen fertiliser!", "The first nutrient that was low was Nitrogen, so by using the green arrow buttons. Cycle through the cards until the nitrogen fertiliser card is in the center.");
                break;
            case 22:
                GameManager.CRM.craftUI.plusButton.enabled = true;
                GameManager.CRM.craftUI.maxButton.enabled = true;
                GameManager.CRM.craftUI.leftButton.enabled = false;
                GameManager.CRM.craftUI.rightButton.enabled = false;
                UpdateTutorial("Select amount!", "Click on the plus button on the left and set the amount of cards you want to craft to one. You can then craft that amount if you have the required resources.");
                break;
            case 23:
                GameManager.CRM.craftUI.minusButton.enabled = false;
                GameManager.CRM.craftUI.minButton.enabled = false;
                GameManager.CRM.craftButton.enabled = false;
                UpdateTutorial("Check your storage!", "On the right side of the craft button, it now shows how much your craft costs. Open up your resources again by clicking on the arrow at the left side of the screen and check if you have enough.");
                break;
            case 24:
                GameManager.CRM.craftButton.enabled = true;
                UpdateTutorial("Let's craft!", "It seems like you have just enough. Let's go back to the craft button and hold down your left mouse button on it. Keep holding down your mouse button until the card is fully crafted.");
                break;
            case 25:
                GameManager.CRM.craftUI.plusButton.enabled = false;
                GameManager.CRM.craftUI.maxButton.enabled = false;
                GameManager.CRM.craftUI.leftButton.enabled = true;
                GameManager.CRM.craftUI.rightButton.enabled = true;
                GameManager.UM.fertiliser += 25;
                UpdateTutorial("Next card!", "Okay, next up is Phosphorus so let's find that fertiliser card. Cycle through the cards again by using the green arrow buttons until the phosphorus fertiliser card is in the center.");
                break;
            case 26:
                GameManager.CRM.craftUI.plusButton.enabled = true;
                GameManager.CRM.craftUI.maxButton.enabled = true;
                GameManager.CRM.craftUI.leftButton.enabled = false;
                GameManager.CRM.craftUI.rightButton.enabled = false;
                UpdateTutorial("Input the amount!", "We also need one of these, so click on the plus button again. You can also type in the amount you need or click the max button to get the max amount of craftable cards.");
                break;
            case 27:
                GameManager.CRM.craftUI.minusButton.enabled = false;
                GameManager.CRM.craftUI.minButton.enabled = false;
                UpdateTutorial("Craft another one!", "Hold down your left mouse on the craft button again and watch your resources get used. If you stop holding down, the crafting process will be cancelled and your resources get returned.");
                break;
            case 28:
                GameManager.CRM.craftUI.plusButton.enabled = false;
                GameManager.CRM.craftUI.maxButton.enabled = false;
                GameManager.CRM.craftUI.leftButton.enabled = true;
                GameManager.CRM.craftUI.rightButton.enabled = true;
                GameManager.UM.water += 5;
                GameManager.UM.fertiliser += 25;
                UpdateTutorial("Last fertiliser card!", "Last nutrient we were missing was Potassium, so let's see if we can find the fertiliser card for that one as well. I will give you some extra resources, but this will be the last time.");
                break;
            case 29:
                GameManager.CRM.craftUI.plusButton.enabled = true;
                GameManager.CRM.craftUI.maxButton.enabled = true;
                GameManager.CRM.craftUI.leftButton.enabled = false;
                GameManager.CRM.craftUI.rightButton.enabled = false;
                UpdateTutorial("Input one once more!", "Now let's do the same thing one more time. Click the plus button and put the craft amount to one once again. You should also note that if you craft more cards you might get a discount.");
                break;
            case 30:
                GameManager.CRM.craftUI.minusButton.enabled = false;
                GameManager.CRM.craftUI.minButton.enabled = false;
                UpdateTutorial("One more craft!", "Click and hold the craft button one last time and add the last fertiliser card to your deck. We now have all the fertilisers we need to get the nutrients in your land back.");
                break;
            case 31:
                GameManager.IPM.craftModeEnabled = true;
                UpdateTutorial("Enough crafting!", "We crafted all the cards we needed so let's get out of crafting mode by pressing C. And then press Q again to get back into manage mode so we can use these cards.");
                break;
            case 32:
                GameManager.IPM.craftModeEnabled = false;
                GameManager.HM.FindCardInHandById("CardPhosphorusFertiliserUtility").GetComponent<CardDrag>().enabled = false;
                GameManager.HM.FindCardInHandById("CardPotassiumFertiliserUtility").GetComponent<CardDrag>().enabled = false;
                UpdateTutorial("Fertilise your land!", "With your newly crafted cards in your hand hover over your nitrogen fertiliser card. Now hold down your left mouse button and drag the fertiliser to your land.");
                break;
            case 33:
                GameManager.HM.FindCardInHandById("CardPhosphorusFertiliserUtility").GetComponent<CardDrag>().enabled = true;
                GameManager.HM.FindCardInHandById("CardPotassiumFertiliserUtility").GetComponent<CardDrag>().enabled = true;
                UpdateTutorial("Fertilise some more!", "Since your land is also missing Phosphorus and Potassium, use those cards as well. One by one, drag them to your land and watch your land improve.");
                break;
            case 34:
                GameManager.IPM.islandInspectEnabled = true;
                UpdateTutorial("Check your land!", "Now that we fertilised your land, let's see if we meet the nutrients required. Click with your left mouse button on your land again and switch to the available nutrients.");
                break;
            case 35:
                GameManager.IPM.marketModeEnabled = true;
                UpdateTutorial("Open the market!", "Let's close the information tab again and open up the market by pressing R. In this mode, we can buy and sell products. You can, however, only sell products from crops you have unlocked.");
                break;
            case 36:
                GameManager.IPM.marketModeEnabled = false;
                foreach (MarketItem marketItem in GameManager.MM.itemsInMarket)
                {
                    marketItem.sellUI.GetComponent<MarketButton>().transactionButton.enabled = false;
                    marketItem.sellUI.GetComponent<MarketButton>().plusButton.enabled = false;
                    marketItem.sellUI.GetComponent<MarketButton>().maxButton.enabled = false;
                    marketItem.sellUI.GetComponent<MarketButton>().inputAmountField.enabled = false;
                    marketItem.buyUI.GetComponent<MarketButton>().transactionButton.enabled = false;
                    marketItem.buyUI.GetComponent<MarketButton>().plusButton.enabled = false;
                    marketItem.buyUI.GetComponent<MarketButton>().maxButton.enabled = false;
                    marketItem.buyUI.GetComponent<MarketButton>().inputAmountField.enabled = false;
                }
                UpdateTutorial("Find rice market!", "Each product has its own price, supply and demand, which changes every week. With the green labels on top, you can sort between products. Now let's scroll down and find the rice market.");
                break;
            case 37:
                riceMarketItem = GameManager.MM.FindMarketItemByName("Rice");
                riceMarketItem.sellUI.GetComponent<MarketButton>().plusButton.enabled = true;
                riceMarketItem.sellUI.GetComponent<MarketButton>().maxButton.enabled = true;
                riceMarketItem.sellUI.GetComponent<MarketButton>().inputAmountField.enabled = true;
                UpdateTutorial("Input the max!", "It is time to earn some money and sell part of your harvest. For now, we are just selling rice, so click on the max button or input the max amount on the sell side of the market.");
                break;
            case 38:
                riceMarketItem = GameManager.MM.FindMarketItemByName("Rice");
                riceMarketItem.sellUI.GetComponent<MarketButton>().minButton.enabled = false;
                riceMarketItem.sellUI.GetComponent<MarketButton>().minusButton.enabled = false;
                riceMarketItem.sellUI.GetComponent<MarketButton>().inputAmountField.enabled = false;
                riceMarketItem.sellUI.GetComponent<MarketButton>().transactionButton.enabled = true;
                UpdateTutorial("Make that money!", "Now click on sell and watch your money come in. With the market fluctuations, you can also earn money by buying when products are cheap and selling when they are expensive.");
                break;
            case 39:
                GameManager.IPM.marketModeEnabled = true;
                UpdateTutorial("Exit the market!", "You got some money in your pocket and are ready to expand. Press R again to exit market mode and Q to go into manage mode. Open up your resources and look at your balance.");
                break;
            case 40:
                GameManager.IPM.defaultModeEnabled = true;
                GameManager.IPM.marketModeEnabled = false;
                expansionIsland = GameManager.ISM.FindIslandByID("Ring1(0,-1)");
                expansionIsland.ToggleState(Island.IslandState.Highlighted, Island.IslandState.Default);
                expansionIsland.islandAvailable = false;
                UpdateTutorial("Start moving!", "In order for you to expand, you need to buy more land. So press Q to exit manage mode and you can now move. Use your WASD keys to move to the newly highlighted island.");
                break;
            case 41:
                UpdateTutorial("Move some more!", "You can also move up by pressing left shift and down by pressing left control. Try to go up a little bit so you have a better view of the highlighted island.");
                break;
            case 42:
                expansionIsland.islandAvailable = true;
                UpdateTutorial("Buy another island!", "You are above your future island, you only need to buy it. Go into manage mode again and hover over the highlighted island. Now hold down your left mouse button until the island is built.");
                break;
            case 43:
                GameManager.DM.AddCardToDeck(GameManager.CM.FindCardById("CardCompostBuildable").cardId);
                GameManager.DM.AddCardToDeck(GameManager.CM.FindCardById("CardWaterBarrelBuildable").cardId);
                GameManager.DM.CheckRefillHand();
                GameManager.HM.FindCardInHandById("CardCompostBuildable").GetComponent<CardDrag>().enabled = false;
                GameManager.HM.FindCardInHandById("CardWaterBarrelBuildable").GetComponent<CardDrag>().enabled = false;
                UpdateTutorial("Inspect new cards!", "Since you ran out of resources crafting cards and just paid for a new island, I will help you and give you some cards. Inspect these cards and see what they do by left clicking on them.");
                break;
            case 44:
                GameManager.HM.FindCardInHandById("CardCompostBuildable").GetComponent<CardDrag>().enabled = true;
                GameManager.HM.FindCardInHandById("CardWaterBarrelBuildable").GetComponent<CardDrag>().enabled = true;
                UpdateTutorial("Use the cards!", "Drag the water barrel and compost cards towards your newly acquired land. These cards will create buildables that generate resources so you can start filling up your storage again.");
                break;
            case 45:
                UpdateTutorial("Check your expenses!", "These buildables will cost money to maintain, so make sure you have some, otherwise they won't give you any. Let’s open up your resources again and have a look at your expenses.");
                break;
            case 46:
                GameManager.IPM.marketModeEnabled = true;
                UpdateTutorial("Shopping time!", "Last up I will show you how to get more plant cards, so you can plant some more crops and expand. Start with pressing R to go into market mode and find the market for chard.");
                break;
            case 47:
                UpdateTutorial("Buy some chard!", "Now buy as much chard as you need until you have a total of 50 chard in your inventory. We are going to need this for crafting. After that, exit market mode by pressing R again.");
                break;
            case 48:
                UpdateTutorial("More crafting!", "Let’s craft some more plant cards, start off pressing C to open up crafting mode again. This time, switch to the plant tab with the green tabs on top and find the chard plant card.");
                break;
            case 49:
                UpdateTutorial("Under construction!", "You made it to the end of the demo. More things will be added and a new version will be out very soon!");
                break;
        }
    }
}