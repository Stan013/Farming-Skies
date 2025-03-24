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
    private Card cardCompost;

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
                UpdateQuest("Move around!", "Let's start with moving around a little bit. You can move by using WASD and you can go up with Shift and down with Control. Try the to find the highlighted island.");
                break;
            case 2:
                UpdateQuest("Build your first island!", "Now it is time to get your first island build. Hover over the highlighted island and hold down your right mouse button until the island is fully build.");
                break;
            case 3:
                GameManager.HM.SetCardsInHand();
                Card cardCultivator = GameManager.HM.FindCardInHandById("CardCultivatorUtility");
                cardCultivator.GetComponent<CardDrag>().enabled = true;
                UpdateQuest("Start cultivating!", "It is time to start getting your land ready for planting. Hover over the cultivator card and hold down your left mouse button. Now move towards your land to cultivate it.");
                break;
            case 4:
                Card cardWateringCan = GameManager.HM.FindCardInHandById("CardWateringCanUtility");
                cardWateringCan.GetComponent<CardDrag>().enabled = true;
                UpdateQuest("Water your land!", "Your land has been cultivated, let's water it so we can start planting. Hover over the watering can card and hold down your left mouse button again. Now move towards your land to water it.");
                break;
            case 5:
                cardGreenBean = GameManager.HM.FindCardInHandById("CardGreenBeanPlant");
                cardGreenBean.GetComponent<CardInspect>().enabled = true;
                UpdateQuest("Inspect your plant!", "Your land is ready for planting, but first let's see what crops you have and what they need. Click with your right mouse button on the green bean plant card and click again to put the card away.");
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
                UpdateQuest("Plant your first crop!", "Let's hover over your green bean plant card, but this time hold down your left mouse button. Then move the plant towards your land and once snapped in place let go of your mouse.");
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
                UpdateQuest("Inspect thoroughly!", "If you want to see some extra details about the items in your inventory you can click on the green button with an arrow. Let's do this with the chard you have in your inventory.");
                break;
            case 14:
                GameManager.UM.selectionUI.SetActive(true);
                UpdateQuest("Close the details!", "Your inventory can also be filtered with the green buttons on top. But for now let's close the extra details by clicking the green button with the arrow located at the top right. ");
                break;
            case 15:
                GameManager.UM.selectionUI.SetActive(true);
                UpdateQuest("Crop yield importance!", "Depending on if the nutrient needs of a crop are met your yield has a chance to either increase or decrease. Let's close your inventory and find out if we meet those needs.");
                break;
            case 16:
                UpdateQuest("Inspect your land!", "As you can see the quality of your land has changed and a warning icon has appeared. Left click on the cogwheel at the right side of your screen to open up your farm management.");
                break;
            case 17:
                UpdateQuest("Select your land!", "With your mouse hover over the land that has a warning icon on top of it. Now click with your left mouse button and an information panel should appear on the right side of your screen.");
                break;
            case 18:
                UpdateQuest("Check your crop needs!", "You can see how much water and nutrients your crops require. Don't run out of water because your land will dry up. Now click with your left mouse button on the green switch icon.");
                break;
            case 19:
                UpdateQuest("Your island reserves!", "It now shows how much your land still has. Some of these reserves are too low for the next harvest so let's help your land out. With your left mouse button click on the red close icon.");
                break;
            case 20:
                UpdateQuest("Open craft mode!", "First we need some cards to use, so open the crafting menu by pressing C. Here you can craft any card you need. You can filter the different card types with the green labels on top.");
                break;
            case 21:
                GameManager.CRM.craftUI.plusButton.enabled = false;
                GameManager.CRM.craftUI.maxButton.enabled = false;
                UpdateQuest("Find nitrogen fertiliser!", "The first nutrient that was low was Nitrogen, so by using the green arrow buttons. Cycle through the cards until the nitrogen fertiliser card is in the center.");
                break;
            case 22:
                GameManager.CRM.craftUI.plusButton.enabled = true;
                GameManager.CRM.craftUI.maxButton.enabled = true;
                GameManager.CRM.craftUI.leftButton.enabled = false;
                GameManager.CRM.craftUI.rightButton.enabled = false;
                UpdateQuest("Select amount!", "Click on the plus button on the left and set the amount of cards you want to craft to one. You can then craft that amount if you have the required resources.");
                break;
            case 23:
                GameManager.CRM.craftUI.minusButton.enabled = false;
                GameManager.CRM.craftUI.minButton.enabled = false;
                GameManager.CRM.craftButton.enabled = false;
                UpdateQuest("Check your storage!", "On the right side of the craft button, it now shows how much your craft costs. Open up your resources again by clicking on the arrow at the left side of the screen and check if you have enough.");
                break;
            case 24:
                GameManager.CRM.craftButton.enabled = true;
                UpdateQuest("Let's craft!", "It seems like you have just enough. Let's go back to the craft button and hold down your left mouse button on it. Keep holding down your mouse button until the card is fully crafted.");
                break;
            case 25:
                GameManager.CRM.craftUI.plusButton.enabled = false;
                GameManager.CRM.craftUI.maxButton.enabled = false;
                GameManager.CRM.craftUI.leftButton.enabled = true;
                GameManager.CRM.craftUI.rightButton.enabled = true;
                GameManager.UM.fertiliser += 25;
                UpdateQuest("Next card!", "Okay, next up is Phosphorus so let's find that fertiliser card. Cycle through the cards again by using the green arrow buttons until the phosphorus fertiliser card is in the center.");
                break;
            case 26:
                GameManager.CRM.craftUI.plusButton.enabled = true;
                GameManager.CRM.craftUI.maxButton.enabled = true;
                GameManager.CRM.craftUI.leftButton.enabled = false;
                GameManager.CRM.craftUI.rightButton.enabled = false;
                UpdateQuest("Input the amount!", "We also need one of these, so click on the plus button again. You can also type in the amount you need or click the max button to get the max amount of craftable cards.");
                break;
            case 27:
                GameManager.CRM.craftUI.minusButton.enabled = false;
                GameManager.CRM.craftUI.minButton.enabled = false;
                UpdateQuest("Craft another one!", "Hold down your left mouse on the craft button again and watch your resources get used. If you stop holding down, the crafting process will be cancelled and your resources get returned.");
                break;
            case 28:
                GameManager.CRM.craftUI.plusButton.enabled = false;
                GameManager.CRM.craftUI.maxButton.enabled = false;
                GameManager.CRM.craftUI.leftButton.enabled = true;
                GameManager.CRM.craftUI.rightButton.enabled = true;
                GameManager.UM.water += 5;
                GameManager.UM.fertiliser += 25;
                UpdateQuest("Last fertiliser card!", "Last nutrient we were missing was Potassium, so let's see if we can find the fertiliser card for that one as well. I will give you some extra resources, but this will be the last time.");
                break;
            case 29:
                GameManager.CRM.craftUI.plusButton.enabled = true;
                GameManager.CRM.craftUI.maxButton.enabled = true;
                GameManager.CRM.craftUI.leftButton.enabled = false;
                GameManager.CRM.craftUI.rightButton.enabled = false;
                UpdateQuest("Input one once more!", "Now let's do the same thing one more time. Click the plus button and put the craft amount to one once again. You should also note that if you craft more cards you might get a discount.");
                break;
            case 30:
                GameManager.CRM.craftUI.minusButton.enabled = false;
                GameManager.CRM.craftUI.minButton.enabled = false;
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
                    if (GameManager.UM.weeks == 1)
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
/*                    if (GameManager.CurrentState == GameManager.GameState.ManageMode)
                    {
                        QuestCompleted = true;
                    }*/
                    break;
                case 20:
/*                    if (GameManager.CurrentState == GameManager.GameState.CraftMode)
                    {
                        QuestCompleted = true;
                    }*/
                    break;
                case 21:
                    if (GameManager.CRM.selectedCard.cardName == "Nitrogen Fertiliser")
                    {
                        QuestCompleted();
                    }
                    break;
                case 22:
                    if (GameManager.CRM.cardCraftAmount == 1)
                    {
                        QuestCompleted();
                    }
                    break;
                case 24:
                    if (GameManager.DM.cardsInDeck.Count == 1)
                    {
                        QuestCompleted();
                    }
                    break;
                case 25:
                    if (GameManager.CRM.selectedCard.cardName == "Phosphorus Fertiliser")
                    {
                        QuestCompleted();
                    }
                    break;
                case 26:
                    if (GameManager.CRM.cardCraftAmount == 1)
                    {
                        QuestCompleted();
                    }
                    break;
                case 27:
                    if (GameManager.DM.cardsInDeck.Count == 2)
                    {
                        QuestCompleted();
                    }
                    break;
                case 28:
                    if (GameManager.CRM.selectedCard.cardName == "Potassium Fertiliser")
                    {
                        QuestCompleted();
                    }
                    break;
                case 29:
                    if (GameManager.CRM.cardCraftAmount == 1)
                    {
                        QuestCompleted();
                    }
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