using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    [Header("Quest UI")]
    public TMP_Text questTitleText;
    public TMP_Text questDescriptionText;
    public Image questCompletedIcon;

    [Header("Quest variables")]
    public bool questActive;
    public int questCount;

    [Header("Quest cards")]
    private Card cardGreenBean;
    private Card cardChive;
    private Card cardChard;
    private Card cardRice;
    private Card cardWaterBarrel;
    private Card cardCompostBin;

    [Header("Quest islands")]
    private Island expansionIsland;

    [Header("Quest marketItems")]
    private MarketItem riceMarketItem;
    private MarketItem chardMarketItem;

    public void QuestCompleted()
    {
        if (questCompletedIcon.gameObject.activeSelf) return;

        questCompletedIcon.gameObject.SetActive(true);
        StartCoroutine(ResetQuestCompletedAfterDelay());
    }

    private IEnumerator ResetQuestCompletedAfterDelay()
    {
        yield return new WaitForSeconds(0.25f);
        questCompletedIcon.gameObject.SetActive(false);
        NextQuest();
    }

    private void NextQuest()
    {
        if (questCount == 0)
        {
            GameManager.WM.OpenQuestWindow();
        }

        questCount++;

        switch (questCount)
        {
            case 1:
                GameManager.ISM.starterIsland.SetIslandState(Island.IslandState.Highlighted);
                UpdateQuest("Move around!", "Let's start with moving around a little bit. You can move by using WASD and you can go up with Shift and down with Control. Try to find the highlighted island and move in all directions.");
                break;
            case 2:
                UpdateQuest("Build your first island!", "Now it is time to get your first island build. Hover over the highlighted island and hold down your right mouse button until the island is fully build.");
                break;
            case 3:
                GameManager.HM.SetCardsInHand();
                Card cardCultivator = GameManager.HM.FindCardInHandById("CardCultivatorUtility");
                cardCultivator.GetComponent<CardDrag>().enabled = true;
                UpdateQuest("Start cultivating!", "It is time to start getting your island ready for planting. Hover over the cultivator card and hold down your left mouse button. Now move towards your island to cultivate it.");
                break;
            case 4:
                Card cardWateringCan = GameManager.HM.FindCardInHandById("CardWateringCanUtility");
                cardWateringCan.GetComponent<CardDrag>().enabled = true;
                UpdateQuest("Water your soil!", "Your island has been cultivated, now let's water it so we can start planting. Hover over the watering can card and hold down your left mouse button again. Now move towards your soil to water it.");
                break;
            case 5:
                cardGreenBean = GameManager.HM.FindCardInHandById("CardGreenBeanPlant");
                cardGreenBean.GetComponent<CardInspect>().enabled = true;
                UpdateQuest("Inspect your plant!", "Your soil is ready for planting, but first let's see what crops you have and what they need. Click with your right mouse button on the green bean plant card and click again to put the card away.");
                break;
            case 6:
                cardChive = GameManager.HM.FindCardInHandById("CardChivePlant");
                cardChive.GetComponent<CardInspect>().enabled = true;
                UpdateQuest("Inspect another!", "Right click again with your mouse on the chive plant card this time. And as you can see, the plant size, water, and nutrient needs are different. This will result in a different yield.");
                break;
            case 7:
                cardChard = GameManager.HM.FindCardInHandById("CardChardPlant");
                cardRice = GameManager.HM.FindCardInHandById("CardRicePlant");
                cardChard.GetComponent<CardInspect>().enabled = true;
                cardRice.GetComponent<CardInspect>().enabled = true;
                UpdateQuest("Compare all of them!", "I hope you noticed the difference, so now inspect the remaining plant cards. Each month, the nutrients in your land will refill, so making good combinations can be very beneficial.");
                break;
            case 8:
                cardGreenBean.GetComponent<CardDrag>().enabled = true;
                UpdateQuest("Plant your first crop!", "Let's hover over your green bean plant card, but this time hold down your left mouse button. Then move the plant towards your soil and once snapped in place let go of your mouse.");
                break;
            case 9:
                cardChive.GetComponent<CardDrag>().enabled = true;
                UpdateQuest("Plant another crop!", "Now do it again with your chive plant card. Since the plant size is smaller, more plots are available. You can, however, only have one plant for each plot, so use your space efficiently.");
                break;
            case 10:
                cardChard.GetComponent<CardDrag>().enabled = true;
                cardRice.GetComponent<CardDrag>().enabled = true;
                UpdateQuest("Plant all your crops!", "You are well on your way to becoming a farmer, so let's plant your last plant cards as well. With the last two plant cards in your hand, do the same drag motions as before.");
                break;
            case 11:
                GameManager.IPM.nextWeekEnabled = true;
                UpdateQuest("Time to advance!", "If you are ready, hold down your space bar and wait until the arrow on the top right fills up. Now watch as the week goes by and let your crops do the work.");
                break;
            case 12:
                GameManager.UM.selectionUI.SetActive(true);
                UpdateQuest("Look at your spoils!", "You watched your harvest come in so let's find out how much you got by opening up your inventory. Click with your left mouse button on the boxes icon at the right side of your screen.");
                break;
            case 13:
                GameManager.UM.selectionUI.SetActive(true);
                UpdateQuest("Inspect thoroughly!", "If you want to see more details about the items in your inventory click on the green button with an arrow point downwards. Let's do this with the chard you have in your inventory.");
                break;
            case 14:
                GameManager.UM.selectionUI.SetActive(true);
                UpdateQuest("Close the details!", "Your inventory can also be filtered with the green tabs on top. But for now let's close the details by clicking the green button with an arrow pointing upwards, located at the top left.");
                break;
            case 15:
                GameManager.UM.selectionUI.SetActive(true);
                UpdateQuest("Crop yield importance!", "Depending on if the nutrient needs of a crop are met your yield has a chance to either increase or decrease. Let's close your inventory and find out if we meet those needs.");
                break;
            case 16:
                UpdateQuest("Inspect your island!", "As you can see the quality of your soil has changed and a warning icon has appeared. Left click on the cogwheel at the right side of your screen to open up your farm management.");
                break;
            case 17:
                UpdateQuest("Select available!", "Now click with your left mouse button on the green available tab at the top. You currently only have one island but you can select the island you want to look at with the arrows.");
                break;
            case 18:
                UpdateQuest("Check required!", "We can now see what nutrients your island has but how much do your crops require. With your left mouse button click on the green required tab and look at your island needs.");
                break;
            case 19:
                UpdateQuest("Let's improve!", "You can also see the in the plots tab, how many available plots there are. Plus how many crops are on your island but for now click on the red close icon so we can help your soil.");
                break;
            case 20:
                UpdateQuest("Checkout crafting!", "First we need some cards to use, so click with your left mouse button on the anvil icon at the right side of your screen. Here you can craft any card you need if you have enough resources.");
                break;
            case 21:
                UpdateQuest("Find nitrogen!", "One of the nutrients that was low was nitrogen so we need to craft some nitrogen fertiliser. Let's use the filters on top and click the green tab that says utilities and then find the nitrogen.");
                break;
            case 22:
                GameManager.UM.Balance += 50;
                GameManager.UM.Water += 10;
                GameManager.UM.Fertiliser += 25;
                UpdateQuest("Quick craft!", "I have given you some resources so that you can now quick craft a nitrogen card. The craft label is currently red since it doesn't have a valid input amount so let's put that to 1.");
                break;
            case 23:
                UpdateQuest("Hold to craft!", "The craft label should turn green and now you can hold down your left mouse button on the craft text. This should turn the label blue and then eventually back to green when the craft is completed.");
                break;
            case 24:
                UpdateQuest("Craft another!", "We crafted 1 nitrogen card, however we weren't able to see how many resources that cost us. So lets now craft the next nutrient phosphorus by clicking on the green button with the arrow pointing downwards again.");
                break;
            case 25:
                GameManager.UM.Balance += 50;
                GameManager.UM.Water += 10;
                GameManager.UM.Fertiliser += 25;
                UpdateQuest("Crafting costs!", "We can now see how much our craft cost this amount will of course be dependent on the amount of cards you craft. We only need 1 so put that amount in, you can also click plus or hit the max button.");
                break;
            case 26:
                UpdateQuest("Hold again!", "The same as the quick craft you will need to hold down your left mouse on the green craft button. If the input is valid the button will turn blue and if held long enough your craft will be succesful.");
                break;
            case 27:
                UpdateQuest("Select next one!", "Do this one more time for the last nutrient we need which is potassium. Search for potassium and click the green button with the arrow pointing downwards again. It should now expand that item.");
                break;
            case 28:
                GameManager.UM.Balance += 50;
                GameManager.UM.Water += 10;
                GameManager.UM.Fertiliser += 25;
                UpdateQuest("Input amount again!", "This will be the last time I give you some resources after that you will have to generate them yourself. We again need one so input 1 or click the plus or max button and hold craft.");
                break;
            case 29:
                UpdateQuest("Input one once more!", "Now let's do the same thing one more time. Click the plus button and put the craft amount to one once again. You should also note that if you craft more cards you might get a discount.");
                break;
            case 30:
                UpdateQuest("One more craft!", "Click and hold the craft button one last time and add the last fertiliser card to your deck. We now have all the fertilisers we need to get the nutrients in your land back.");
                break;
            case 31:
                UpdateQuest("Enough crafting!", "We crafted all the cards we needed so let's get out of crafting mode by pressing C. And then press Q again to get back into manage mode so we can use these cards.");
                break;
            case 32:
                GameManager.HM.FindCardInHandById("CardPhosphorusFertiliserUtility").GetComponent<CardDrag>().enabled = false;
                GameManager.HM.FindCardInHandById("CardPotassiumFertiliserUtility").GetComponent<CardDrag>().enabled = false;
                UpdateQuest("Fertilise your land!", "With your newly crafted cards in your hand hover over your nitrogen fertiliser card. Now hold down your left mouse button and drag the fertiliser to your land.");
                break;
            case 33:
                GameManager.HM.FindCardInHandById("CardPhosphorusFertiliserUtility").GetComponent<CardDrag>().enabled = true;
                GameManager.HM.FindCardInHandById("CardPotassiumFertiliserUtility").GetComponent<CardDrag>().enabled = true;
                UpdateQuest("Fertilise some more!", "Since your land is also missing Phosphorus and Potassium, use those cards as well. One by one, drag them to your land and watch your land improve.");
                break;
            case 34:
                UpdateQuest("Check your land!", "Now that we fertilised your land, let's see if we meet the nutrients required. Click with your left mouse button on your land again and switch to the available nutrients.");
                break;
            case 35:
                UpdateQuest("Open the market!", "Let's close the information tab again and open up the market by pressing R. In this mode, we can buy and sell products. You can, however, only sell products from crops you have unlocked.");
                break;
            case 36:
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
                UpdateQuest("Find rice market!", "Each product has its own price, supply and demand, which changes every week. With the green labels on top, you can sort between products. Now let's scroll down and find the rice market.");
                break;
            case 37:
                riceMarketItem = GameManager.MM.FindMarketItemByName("Rice");
                riceMarketItem.sellUI.GetComponent<MarketButton>().plusButton.enabled = true;
                riceMarketItem.sellUI.GetComponent<MarketButton>().maxButton.enabled = true;
                riceMarketItem.sellUI.GetComponent<MarketButton>().inputAmountField.enabled = true;
                UpdateQuest("Input the max!", "It is time to earn some money and sell part of your harvest. For now, we are just selling rice, so click on the max button or input the max amount on the sell side of the market.");
                break;
            case 38:
                riceMarketItem = GameManager.MM.FindMarketItemByName("Rice");
                riceMarketItem.sellUI.GetComponent<MarketButton>().minButton.enabled = false;
                riceMarketItem.sellUI.GetComponent<MarketButton>().minusButton.enabled = false;
                riceMarketItem.sellUI.GetComponent<MarketButton>().inputAmountField.enabled = false;
                riceMarketItem.sellUI.GetComponent<MarketButton>().transactionButton.enabled = true;
                UpdateQuest("Make that money!", "Now click on sell and watch your money come in. With the market fluctuations, you can also earn money by buying when products are cheap and selling when they are expensive.");
                break;
            case 39:
                UpdateQuest("Exit the market!", "You got some money in your pocket and are ready to expand. Press R again to exit market mode and Q to go into manage mode. Open up your resources and look at your balance.");
                break;
            case 40:
                expansionIsland = GameManager.ISM.FindIslandByID("(0,-1)");
                expansionIsland.SetIslandState(Island.IslandState.Highlighted);
                UpdateQuest("Start moving!", "In order for you to expand, you need to buy more land. So press Q to exit manage mode and you can now move. Use your WASD keys to move to the newly highlighted island.");
                break;
            case 41:
                UpdateQuest("Move some more!", "You can also move up by pressing left shift and down by pressing left control. Try to go up a little bit so you have a better view of the highlighted island.");
                break;
            case 42:
                UpdateQuest("Buy another island!", "You are above your future island, you only need to buy it. Go into manage mode again and hover over the highlighted island. Now hold down your left mouse button until the island is built.");
                break;
            case 43:
                GameManager.DM.AddCardToDeck(GameManager.CM.FindCardByID("CardCompostBuildable").cardId);
                GameManager.DM.AddCardToDeck(GameManager.CM.FindCardByID("CardWaterBarrelBuildable").cardId);
                GameManager.HM.SetCardsInHand();
                GameManager.HM.FindCardInHandById("CardCompostBuildable").GetComponent<CardDrag>().enabled = false;
                GameManager.HM.FindCardInHandById("CardWaterBarrelBuildable").GetComponent<CardDrag>().enabled = false;
                UpdateQuest("Inspect new cards!", "Since you ran out of resources crafting cards and just paid for a new island, I will help you and give you some cards. Inspect these cards and see what they do by left clicking on them.");
                break;
            case 44:
                GameManager.HM.FindCardInHandById("CardCompostBuildable").GetComponent<CardDrag>().enabled = true;
                GameManager.HM.FindCardInHandById("CardWaterBarrelBuildable").GetComponent<CardDrag>().enabled = true;
                UpdateQuest("Use the cards!", "Drag the water barrel and compost cards towards your newly acquired land. These cards will create buildables that generate resources so you can start filling up your storage again.");
                break;
            case 45:
                UpdateQuest("Check your expenses!", "These buildables will cost money to maintain, so make sure you have some, otherwise they won't give you any. Let’s open up your resources again and have a look at your expenses.");
                break;
            case 46:
                UpdateQuest("Shopping time!", "Last up I will show you how to get more plant cards, so you can plant some more crops and expand. Start with pressing R to go into market mode and find the market for chard.");
                break;
            case 47:
                chardMarketItem = GameManager.MM.FindMarketItemByName("Chard");
                chardMarketItem.buyUI.GetComponent<MarketButton>().plusButton.enabled = true;
                chardMarketItem.buyUI.GetComponent<MarketButton>().maxButton.enabled = true;
                chardMarketItem.buyUI.GetComponent<MarketButton>().inputAmountField.enabled = true;
                UpdateQuest("Buy some chard!", "Now buy as much chard as you need until you have a total of 50 chard in your inventory. We are going to need this for crafting. After that, exit market mode by pressing R again.");
                break;
            //case 48:
            //    UpdateTutorial("More crafting!", "Let’s craft some more plant cards, start off pressing C to open up crafting mode again. This time, switch to the plant tab with the green tabs on top and find the chard plant card.");
            //    break;
            case 48:
                UpdateQuest("Under construction!", "You made it to the end of the demo. More things will be added and a new version will be out very soon!");
                break;
        }
    }

    private void UpdateQuest(string questTitle, string questDescription)
    {
        questTitleText.SetText(questTitle);
        questDescriptionText.SetText(questDescription);
    }

    public void QuestCheck()
    {
        if (questActive)
        {
            switch (questCount)
            {
                case 1:
                    if(GameManager.IPM.cam.transform.position.x != 0 && GameManager.IPM.cam.transform.position.z != 0)
                    {
                        QuestCompleted();
                    } 
                    break;
                case 2:
                    if(GameManager.ISM.starterIsland.islandBought)
                    {
                        QuestCompleted();
                    }
                    break; 
                case 3:
                    if (GameManager.ISM.starterIsland.currentState == Island.IslandState.Cultivated && !GameManager.HM.dragging)
                    {
                        QuestCompleted();
                    }
                    break;
                case 4:
                    if (GameManager.ISM.starterIsland.currentState == Island.IslandState.Watered && !GameManager.HM.dragging)
                    {
                        QuestCompleted();
                    }
                    break;
                case 5:
                    if (GameManager.HM.FindCardInHandById("CardGreenBeanPlant").hasBeenInspected)
                    {
                        QuestCompleted();
                    }
                    break;
                case 6:
                    if (GameManager.HM.FindCardInHandById("CardChivePlant").hasBeenInspected)
                    {
                        QuestCompleted();
                    }
                    break;
                case 7:
                    if (GameManager.HM.FindCardInHandById("CardChardPlant").hasBeenInspected && GameManager.HM.FindCardInHandById("CardRicePlant").hasBeenInspected)
                    {
                        QuestCompleted();
                    }
                    break;
                case 8:
                    if (GameManager.ISM.starterIsland.itemsOnIsland.Count == 1)
                    {
                        QuestCompleted();
                    }
                    break;
                case 9:
                    if (GameManager.ISM.starterIsland.itemsOnIsland.Count == 2)
                    {
                        QuestCompleted();
                    }
                    break;
                case 10:
                    if (GameManager.ISM.starterIsland.itemsOnIsland.Count == 4)
                    {
                        QuestCompleted();
                    }
                    break;
                case 11:
                    if (GameManager.UM.Weeks == 1)
                    {
                        QuestCompleted();
                    }
                    break;
                case 12:
                    if (GameManager.WM.inventoryWindow.activeSelf)
                    {
                        QuestCompleted();
                    }
                    break;
                case 13:
                    if (GameManager.INM.expandedInventoryItem.gameObject.activeSelf)
                    {
                        QuestCompleted();
                    }
                    break;
                case 14:
                    if (!GameManager.INM.expandedInventoryItem.gameObject.activeSelf)
                    {
                        QuestCompleted();
                    }
                    break;
                case 15:
                    if (!GameManager.WM.inventoryWindow.activeSelf)
                    {
                        QuestCompleted();
                    }
                    break;
                case 16:
                    if (GameManager.WM.manageWindow.activeSelf)
                    {
                        QuestCompleted();
                    }
                    break;
                case 17:
                    if (GameManager.ISM.islandManageTab == "Available")
                    {
                        QuestCompleted();
                    }
                    break;
                case 18:
                    if (GameManager.ISM.islandManageTab == "Required")
                    {
                        QuestCompleted();
                    }
                    break;
                case 19:
                    if (!GameManager.WM.manageWindow.activeSelf)
                    {
                        QuestCompleted();
                    }
                    break;
                case 20:
                    if (GameManager.WM.craftWindow.activeSelf)
                    {
                        QuestCompleted();
                    }
                    break;
                case 21:
                    if (GameManager.CRM.craftingTab == "Utilities")
                    {
                        QuestCompleted();
                    }
                    break;
                case 22:
                    if (GameManager.CRM.GetCraftItemByID("CardNitrogenFertiliserUtility").craftAmount == 1)
                    {
                        QuestCompleted();
                    }
                    break;
                case 23:
                    if (GameManager.DM.cardsInDeck.Count == 1)
                    {
                        QuestCompleted();
                    }
                    break;
                case 24:
                    if (GameManager.CRM.expandedCraftItem.gameObject.activeSelf)
                    {
                        QuestCompleted();
                    }
                    break;
                case 25:
                    if (GameManager.CRM.expandedCraftItem.craftAmount == 1 && GameManager.CRM.expandedCraftItem.collapsedItem.attachedItemCard.itemName == "Phosphorus")
                    {
                        QuestCompleted();
                    }
                    break;
                case 26:
                    if (GameManager.DM.cardsInDeck.Count == 2)
                    {
                        QuestCompleted();
                    }
                    break;
                case 27:
                    if (GameManager.CRM.expandedCraftItem.craftAmount == 1 && GameManager.CRM.expandedCraftItem.collapsedItem.attachedItemCard.itemName == "Potassium")
                    {
                        QuestCompleted();
                    }
                    break;
                case 28:
                    if (GameManager.DM.cardsInDeck.Count == 3)
                    {
                        QuestCompleted();
                    }
                    break;
                case 29:
                    break;
                case 30:
                    if (GameManager.DM.cardsInDeck.Count == 3)
                    {
                        QuestCompleted();
                    }
                    break;
                case 31:
/*                    if (GameManager.CurrentState == GameManager.GameState.ManageMode)
                    {
                        QuestCompleted = true;
                    }*/
                    break;
                case 32:
                    if (GameManager.ISM.starterIsland.nutrientsAvailable[1] != 0)
                    {
                        QuestCompleted();
                    }
                    break;
                case 33:
                    if (GameManager.ISM.starterIsland.nutrientsAvailable[2] != 0 && GameManager.ISM.starterIsland.nutrientsAvailable[3] != 0)
                    {
                        QuestCompleted();
                    }
                    break;
                case 35:
/*                    if (GameManager.CurrentState == GameManager.GameState.MarketMode)
                    {
                        QuestCompleted = true;
                    }*/
                    break;
                case 36:
                    if (GameManager.MM.marketScrollbar.value < 0.1)
                    {
                        QuestCompleted();
                    }
                    break;
                case 37:
                    if (GameManager.MM.FindMarketItemByName("Rice").sellUI.inputAmount == GameManager.MM.FindMarketItemByName("Rice").sellUI.GetComponent<MarketButton>().maxAmount)
                    {
                        QuestCompleted();
                    }
                    break;
                case 38:
                    if (GameManager.MM.FindMarketItemByName("Rice").attachedItemCard.itemQuantity == 0)
                    {
                        QuestCompleted();
                    }
                    break;
                case 39:
/*                    if (GameManager.CurrentState == GameManager.GameState.ManageMode && GameManager.UM.UIMenu.activeSelf)
                    {
                        QuestCompleted = true;
                    }*/
                    break;
                case 40:
                    if (GameManager.IPM.cam.transform.position.z < -6)
                    {
                        QuestCompleted();
                    }
                    break;
                case 41:
                    if (GameManager.IPM.cam.transform.position.y > 10)
                    {
                        QuestCompleted();
                    }
                    break;
                case 42:
                    if (GameManager.ISM.FindIslandByID("(0,-1)").islandBought)
                    {
                        QuestCompleted();
                    }
                    break;
                case 43:
                    if (GameManager.HM.FindCardInHandById("CardWaterBarrelBuildable") != null && GameManager.HM.FindCardInHandById("CardCompostBuildable") != null)
                    {
                        if (GameManager.HM.FindCardInHandById("CardWaterBarrelBuildable").hasBeenInspected && GameManager.HM.FindCardInHandById("CardCompostBuildable").hasBeenInspected)
                        {
                            QuestCompleted();
                        }
                    }
                    break;
                case 44:
                    if (GameManager.ISM.FindIslandByID("Ring1(0,-1)").itemsOnIsland.Count == 2)
                    {
                        QuestCompleted();
                    }
                    break;
/*                case 46:
                    if (GameManager.CurrentState == GameManager.GameState.MarketMode)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 47:
                    if (GameManager.CurrentState == GameManager.GameState.Default && GameManager.CM.FindCardById("CardChardPlant").itemQuantity >= 50)
                    {
                        QuestCompleted = true;
                    }
                    break;*/
                case 48:
                    break;
            }
        }
    }
}