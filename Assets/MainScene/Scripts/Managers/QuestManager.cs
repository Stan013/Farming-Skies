using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour, IDataPersistence
{
    [Header("Quest UI")]
    public TMP_Text questTitleText;
    public TMP_Text questDescriptionText;
    public Image questCompletedIcon;

    [Header("Quest variables")]
    public bool questActive;
    public int questCount;

    [Header("Quest cards")]
    private Card cardCultivator;
    private Card cardWateringCan;
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

    public void InitializeQuestCards()
    {
        if (questCount > 2)
        {
            cardCultivator = GameManager.HM.FindCardInHandById("CardCultivatorUtility");
            cardWateringCan = GameManager.HM.FindCardInHandById("CardWateringCanUtility");
            cardGreenBean = GameManager.HM.FindCardInHandById("CardGreenBeanPlant");
            cardChive = GameManager.HM.FindCardInHandById("CardChivePlant");
            cardChard = GameManager.HM.FindCardInHandById("CardChardPlant");
            cardRice = GameManager.HM.FindCardInHandById("CardRicePlant");
        }
        if(questCount > 11)
        {
            GameManager.UM.selectionUI.SetActive(true);
        }
    }

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
                GameManager.ISM.starterIsland.currentState = Island.IslandState.Highlighted;
                UpdateQuest("Move around!", "Let's start with moving around a little bit. You can move by using WASD and you can go up with Shift and down with Control. Try to find the highlighted island and move in all directions.");
                break;
            case 2:
                UpdateQuest("Build your first island!", "Now it is time to get your first island build. Hover over the highlighted island and hold down your right mouse button until the island is fully build.");
                break;
            case 3:
                GameManager.HM.SetCardsInHand();
                InitializeQuestCards();
                cardCultivator.GetComponent<CardDrag>().enabled = true;
                UpdateQuest("Start cultivating!", "It is time to start getting your island ready for planting. Hover over the cultivator card and hold down your left mouse button. Now move towards your island to cultivate it.");
                break;
            case 4:
                cardWateringCan.GetComponent<CardDrag>().enabled = true;
                UpdateQuest("Water your soil!", "Your island has been cultivated, now let's water it so we can start planting. Hover over the watering can card and hold down your left mouse button again. Now move towards your soil to water it.");
                break;
            case 5:
                cardGreenBean.GetComponent<CardInspect>().enabled = true;
                UpdateQuest("Inspect your plant!", "Your soil is ready for planting, but first let's see what crops you have and what they need. Click with your right mouse button on the green bean plant card and click again to put the card away.");
                break;
            case 6:
                cardChive.GetComponent<CardInspect>().enabled = true;
                UpdateQuest("Inspect another!", "Right click again with your mouse on the chive plant card this time. And as you can see, the plant size, water, and nutrient needs are different. This will result in a different yield.");
                break;
            case 7:
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
                if (cardChard != null)
                {
                    cardChard.GetComponent<CardDrag>().enabled = true;
                }
                cardRice.GetComponent<CardDrag>().enabled = true;
                UpdateQuest("Plant all your crops!", "You are well on your way to becoming a farmer, so let's plant your last plant cards as well. With the last two plant cards in your hand, do the same drag motions as before.");
                break;
            case 11:
                GameManager.IPM.nextWeekEnabled = true;
                UpdateQuest("Time to advance!", "If you are ready, hold down your space bar and wait until the arrow on the top right fills up. Now watch as the week goes by and let your crops do the work.");
                break;
            case 12:
                GameManager.IPM.nextWeekEnabled = false;
                GameManager.UM.selectionUI.SetActive(true);
                GameManager.UM.selectionUI.transform.GetChild(0).GetComponent<Button>().interactable = true;
                UpdateQuest("Look at your spoils!", "You watched your harvest come in so let's find out how much you got by opening up your inventory. Click with your left mouse button on the boxes icon at the right side of your screen.");
                break;
            case 13:
                GameManager.INM.closeButton.interactable = false;
                UpdateQuest("Inspect thoroughly!", "If you want to see more details about the items in your inventory click on the green button with an arrow pointing downwards. Let's do this with the chard you have in your inventory.");
                break;
            case 14:
                GameManager.INM.closeButton.interactable = true;
                UpdateQuest("Close the details!", "It also shows you an estimate harvest for next week it might be low as you haven't improved your soil yet. That is the next thing you are going to work on by closing your inventory with the red cross on the top left.");
                break;
            case 15:
                GameManager.UM.selectionUI.transform.GetChild(0).GetComponent<Button>().interactable = false;
                GameManager.UM.selectionUI.transform.GetChild(1).GetComponent<Button>().interactable = true;
                UpdateQuest("Inspect your island!", "You first need to find out what nutrients you are missing by inspecting your soil. So let's left click on the cogwheel at the right side of your screen to open up your island management.");
                break;
            case 16:
                GameManager.ISM.closeButton.interactable = false;
                UpdateQuest("Select available!", "Now click with your left mouse button on the green tab at the top that says available. You currently only have one island but you can select the island you want to look at with the arrows.");
                break;
            case 17:
                UpdateQuest("Check required!", "You can now see what nutrients your island has, but how much do your crops require. With your left mouse button click on the green tab that says required and look at your crops needs.");
                break;
            case 18:
                GameManager.ISM.closeButton.interactable = true;
                UpdateQuest("Let's improve!", "With this information you should now know that the nutrients, nitrogen, phosphorus and potassium in our soil are low. This has reduced your yield so lets improve this by clicking on the red close icon.");
                break;
            case 19:
                GameManager.UM.selectionUI.transform.GetChild(1).GetComponent<Button>().interactable = false;
                GameManager.UM.selectionUI.transform.GetChild(2).GetComponent<Button>().interactable = true;
                UpdateQuest("Checkout crafting!", "First you need some fertiliser cards to use, so click with your left mouse button on the anvil icon at the right side of your screen. Here you can craft any card you need if you have enough resources.");
                break;
            case 20:
                GameManager.CRM.closeButton.interactable = false;
                UpdateQuest("Find nitrogen!", "One of the nutrients that was low was nitrogen so you need to craft some nitrogen fertiliser. Let's use the filters on top and click the green tab that says utilities and then find the nitrogen.");
                break;
            case 21:
                GameManager.UM.Balance += 50;
                GameManager.UM.Water += 10;
                GameManager.UM.Fertiliser += 25;
                GameManager.CRM.FindCraftItemByID("CardNitrogenUtility").craftAmountInput.interactable = true;
                UpdateQuest("Quick craft!", "I have given you some resources so that you can now quick craft a nitrogen card. Put 1 into the craft amount and the red label will turn green which means you have the resources available to craft that amount.");
                break;
            case 22:
                UpdateQuest("Hold to craft!", "You can now use your left mouse button and hold down on the craft label. This should turn the amount label yellow and then eventually back to red when the craft is completed.");
                break;
            case 23:
                GameManager.UM.Balance += 50;
                GameManager.UM.Water += 10;
                GameManager.UM.Fertiliser += 25;
                GameManager.CRM.FindCraftItemByID("CardNitrogenUtility").craftAmountInput.interactable = false;
                GameManager.CRM.FindCraftItemByID("CardPhosphorusUtility").expandButton.interactable = true;
                UpdateQuest("Craft another!", "You crafted 1 nitrogen card, however you weren't able to see how many resources that cost you. So let's now craft the next nutrient phosphorus by clicking on the green button with the arrow pointing downwards again.");
                break;
            case 24:
                UpdateQuest("Crafting costs!", "You can now see how much your craft cost this amount will of course be dependent on the amount of cards you craft. You only need 1 so put that amount in, you can also click plus or the max button.");
                break;
            case 25:
                UpdateQuest("Hold again!", "The same as the quick craft you will need to hold down your left mouse on the green craft button. If the input is valid the button will turn yellow and if held long enough your craft will be succesful.");
                break;
            case 26:
                GameManager.CRM.expandedCraftItem.CollapseCraftItem();
                GameManager.UM.Balance += 50;
                GameManager.UM.Water += 10;
                GameManager.UM.Fertiliser += 25;
                GameManager.CRM.expandedCraftItem.craftAmountInput.interactable = false;
                GameManager.CRM.FindCraftItemByID("CardPhosphorusUtility").expandButton.interactable = false;
                GameManager.CRM.FindCraftItemByID("CardPotassiumUtility").expandButton.interactable = true;
                UpdateQuest("Select next one!", "Do this one more time for the last nutrient you need which is potassium. Search for potassium and click the green button with the arrow pointing downwards again. It should now expand that item.");
                break;
            case 27:
                GameManager.CRM.expandedCraftItem.craftAmountInput.interactable = true;
                UpdateQuest("Input amount again!", "This will be the last time I give you some resources after that you will have to generate them yourself. You again need one card so input 1 or click the plus or max button.");
                break;
            case 28:
                UpdateQuest("The last nutrient!", "Let's do the same thing again and hold the craft button until your card is crafted. The craft button should turn yellow and when finished back to red.");
                break;
            case 29:
                GameManager.CRM.closeButton.interactable = true;
                UpdateQuest("Enough crafting!", "You should now have 3 fertiliser cards in your deck for each of the nutrients you need. So let's use these cards to improve your soil, close the crafting window by clicking on the red cross.");
                break;
            case 30:
                GameManager.UM.selectionUI.transform.GetChild(2).GetComponent<Button>().interactable = false;
                UpdateQuest("Time to improve!", "It is time to improve your soil with the fertiliser cards you just crafted. Hover over them and while holding down your left mouse button drag them to you land. Do this with all your fertiliser cards.");
                break;
            case 31:
                GameManager.UM.selectionUI.transform.GetChild(0).GetComponent<Button>().interactable = true;
                UpdateQuest("Crop yield!", "Your soil is looking better and the warning icon has disappeared. So let's see how much your estimated harvest will be now. Open up your inventory again and expand one of your items to see the improvement.");
                break;
            case 32:
                GameManager.INM.closeButton.interactable = true;
                UpdateQuest("Earn some money!", "With this in mind it is time to see what you can do with your harvest. You can either reinvest it into more plant cards or in this case sell them. Close your inventory again and on we go.");
                break;
            case 33:
                GameManager.UM.selectionUI.transform.GetChild(3).GetComponent<Button>().interactable = true;
                UpdateQuest("Look at the market!", "Let's go to the market and figure out how you can sell your harvest. Open up the market window by left clicking on the market stall at the right side of your screen.");
                break;
            case 34:
                UpdateQuest("Find rice market!", "As you can see this is the place where you can buy and sell all your items. Let's use the filters on top and click the green tab that says grains and then find the market for rice.");
                break;
            case 35:
                GameManager.MM.FindMarketItemByID("CardRicePlant").transactionAmountInput.interactable = true;
                UpdateQuest("Quick sell rice!", "The same as crafting you can quick sell or buy an item. It will then sell or buy the given quantity for the current market price. Let's do this with the rice we harvested and put in 5 as the sell amount.");
                break;
            case 36:
                UpdateQuest("Sell your rice!", "As you can see the label turns green again which means you put in a valid amount. Now use your left mouse button and hold down on the sell label to earn some money.");
                break;
            case 37:
                GameManager.MM.FindMarketItemByID("CardRicePlant").transactionAmountInput.interactable = false;
                GameManager.MM.FindMarketItemByID("CardRicePlant").expandButton.interactable = true;
                UpdateQuest("Market details!", "The problem again is that we don't know how much the rice sold for. Besides we don't know what the supply and demand is for rice plus how much we still have left. So let's click on the green button again to expand.");
                break;
            case 38:
                GameManager.MM.expandedMarketItem.transactionButton.interactable = false;
                UpdateQuest("Get rice back!", "We can now see the demand and the supply of the rice plus the current price. These numbers will change everyday so sell and buy at the right time. Since we will need rice put in the same amount as we sold so 10.");
                break;
            case 39:
                UpdateQuest("Buy the rice!", "The label will turn green again and you can now buy 5 rice. Hover your mouse over the green buy button and the same as before hold it down to buy back the rice we just sold.");
                break;
            case 40:
                GameManager.MM.closeButton.interactable = true;
                EnableFreeMarket();
                UpdateQuest("Make 500₴ yourself!", "These are the basics of managing your crops and generating money. So how about you show me you can make 500₴ and I will tell you some more about expanding your farm.");
                break;
            case 41:
                GameManager.ISM.FindIslandByID("(0,-1)").currentState = Island.IslandState.Highlighted;
                UpdateQuest("Expansion time!", "Well done on earning yourself some money as promised I will help you to expand. First of let's buy another island move towards the blue highlighted island and buy it.");
                break;
            case 42:
                if(GameManager.HM.FindCardInHandById("CardConcreteBagUtility") == null)
                {
                    GameManager.DM.AddCardToDeck("CardConcreteBagUtility");
                }
                GameManager.HM.SetCardsInHand();
                UpdateQuest("Paving time!", "Before you get more crops make sure you can maintain the ones you have. This is done by getting some buildables but you can only place these on a paved island. So hover of the concrete bag and drag it towards your island.");
                break;
            case 43:
                if (GameManager.HM.FindCardInHandById("CardWaterBarrelBuildable") == null)
                {
                    GameManager.DM.AddCardToDeck("CardWaterBarrelBuildable");
                }
                if (GameManager.HM.FindCardInHandById("CardCompostBinBuildable") == null)
                {
                    GameManager.DM.AddCardToDeck("CardCompostBinBuildable");
                }
                GameManager.HM.SetCardsInHand();
                UpdateQuest("Let's refill!", "Now that your island is paved you can place buildables on it. These are buildings that will replenish the resources you need for crafting cards. Inspect the cards that I gave you to see what each buildable card does.");
                break;
            case 44:
                GameManager.HM.FindCardInHandById("CardWaterBarrelBuildable").GetComponent<CardDrag>().enabled = true;
                GameManager.HM.FindCardInHandById("CardCompostBinBuildable").GetComponent<CardDrag>().enabled = true;
                UpdateQuest("Place them down!", "You know what they do so let's place them down and get your resources storage replenished. Hover over the cards again and drag them towards your island as you can see they are the same size as a medium plant.");
                break;
            case 45:
                GameManager.UM.selectionUI.transform.GetChild(4).GetComponent<Button>().interactable = true;
                UpdateQuest("Check your stats!", "These buildables and the new island you bought will up your farm value and as you might have noticed you levelled up. Let's check out your farm statistics by clicking on the calculator at the right side of your screen.");
                break;
            case 46:
                UpdateQuest("Inspect your expenses!", "Here you can see some stats about your farm but that is not what we are looking for right now. Click on the green label on top which says expenses to see all the expenses you will need to pay for.");
                break;
            case 47:
                UpdateQuest("Close expenses!", "So the more islands you buy and the more buildables you get the more you will have to pay. Let's close the expense window and have a look at when you will need to pay this and some other events you might encounter.");
                break;
            case 48:
                GameManager.UM.selectionUI.transform.GetChild(5).GetComponent<Button>().interactable = true;
                UpdateQuest("Look at calendar!", "To see all the upcoming events which includes when you need to pay but also when your soil gets refilled or when you will get new plant cards. Click with your left mouse button on the calendar icon at the right side of your screen.");
                break;
            case 49:

                UpdateQuest("Check the events!", "Let's use the filters on top again and click the green tab that says New Cards to see when you will receive new plant cards. After that click the green tab that says Payments these will be the weeks you need to pay your expenses.");
                break;
        }

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
                    if (GameManager.TM.Weeks == 1)
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
                    if (!GameManager.WM.inventoryWindow.activeSelf)
                    {
                        QuestCompleted();
                    }
                    break;
                case 15:
                    if (GameManager.WM.manageWindow.activeSelf)
                    {
                        QuestCompleted();
                    }
                    break;
                case 16:
                    if (GameManager.ISM.islandManageTab == "Available")
                    {
                        QuestCompleted();
                    }
                    break;
                case 17:
                    if (GameManager.ISM.islandManageTab == "Required")
                    {
                        QuestCompleted();
                    }
                    break;
                case 18:
                    if (!GameManager.WM.manageWindow.activeSelf)
                    {
                        QuestCompleted();
                    }
                    break;
                case 19:
                    if (GameManager.WM.craftWindow.activeSelf)
                    {
                        QuestCompleted();
                    }
                    break;
                case 20:
                    if (GameManager.CRM.craftingTab == "Utilities")
                    {
                        QuestCompleted();
                    }
                    break;
                case 21:
                    if (GameManager.CRM.FindCraftItemByID("CardNitrogenUtility").craftAmount == 1)
                    {
                        QuestCompleted();
                    }
                    break;
                case 22:
                    if (GameManager.DM.cardsInDeck.Count == 1)
                    {
                        QuestCompleted();
                    }
                    break;
                case 23:
                    if (GameManager.CRM.expandedCraftItem.gameObject.activeSelf)
                    {
                        QuestCompleted();
                    }
                    break;
                case 24:
                    if (GameManager.CRM.expandedCraftItem.craftAmount == 1 && GameManager.CRM.expandedCraftItem.collapsedItem.attachedItemCard.itemName == "Phosphorus")
                    {
                        QuestCompleted();
                    }
                    break;
                case 25:
                    if (GameManager.DM.cardsInDeck.Count == 2)
                    {
                        QuestCompleted();
                    }
                    break;
                case 26:
                    if (GameManager.CRM.expandedCraftItem.collapsedItem.attachedItemCard.itemName == "Potassium")
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
                    if (!GameManager.WM.craftWindow.activeSelf)
                    {
                        QuestCompleted();
                    }
                    break;
                case 30:
                    if (!GameManager.ISM.starterIsland.warningIcon.activeSelf)
                    {
                        QuestCompleted();
                    }
                    break;
                case 31:
                    if(GameManager.WM.inventoryWindow.activeSelf && GameManager.INM.expandedInventoryItem.gameObject.activeSelf)
                    {
                        QuestCompleted();
                    }
                    break;
                case 32:
                    if (!GameManager.WM.inventoryWindow.activeSelf)
                    {
                        QuestCompleted();
                    }
                    break;
                case 33:
                    if (GameManager.WM.marketWindow.activeSelf)
                    {
                        QuestCompleted();
                    }
                    break;
                case 34:
                    if (GameManager.MM.marketTab == "Grains")
                    {
                        QuestCompleted();
                    }
                    break;
                case 35:
                    if (GameManager.MM.FindMarketItemByID("CardRicePlant").transactionAmount == 5)
                    {
                        QuestCompleted();
                    }
                    break;
                case 36:
                    if (GameManager.UM.Balance != 0)
                    {
                        QuestCompleted();
                    }
                    break;
                case 37:
                    if(GameManager.MM.expandedMarketItem.gameObject.activeSelf)
                    {
                        QuestCompleted();
                    }
                    break;
                case 38:
                    if (GameManager.MM.expandedMarketItem.transactionAmount == 5)
                    {
                        QuestCompleted();
                    }
                    break;
                case 39:
                    if(GameManager.UM.Balance == 0)
                    {
                        QuestCompleted();
                    }
                    break;
                case 40:
                    if (GameManager.UM.Balance >= 500)
                    {
                        QuestCompleted();
                    }
                    break;
                case 41:
                    if (GameManager.ISM.FindIslandByID("(0,-1)").islandBought)
                    {
                        QuestCompleted();
                    }
                    break;
                case 42:
                    if (GameManager.ISM.FindIslandByID("(0,-1)").currentState == Island.IslandState.Paved && !GameManager.HM.dragging)
                    {
                        QuestCompleted();
                    }
                    break;
                case 43:
                    if (GameManager.HM.FindCardInHandById("CardWaterBarrelBuildable").hasBeenInspected && GameManager.HM.FindCardInHandById("CardCompostBinBuildable").hasBeenInspected)
                    {
                        QuestCompleted();
                    }
                    break;
                case 44:
                    if (GameManager.ISM.FindIslandByID("(0,-1)").itemsOnIsland.Count == 2)
                    {
                        QuestCompleted();
                    }
                    break;
                case 45:
                    if (GameManager.WM.expenseWindow.activeSelf)
                    {
                        QuestCompleted();
                    }
                    break;
                case 46:
                    if (GameManager.EM.statsTab == "Expenses") 
                    {
                        QuestCompleted();
                    }
                    break;
                case 47:
                    if (!GameManager.WM.expenseWindow.activeSelf)
                    {
                        QuestCompleted();
                    }
                    break;
                case 48:
                    break;
            }
        }
    }

    public void EnableFreeMarket()
    {
        GameManager.UM.selectionUI.transform.GetChild(0).GetComponent<Button>().interactable = true;
        GameManager.INM.closeButton.interactable = true;
        GameManager.UM.selectionUI.transform.GetChild(1).GetComponent<Button>().interactable = true;
        GameManager.CRM.closeButton.interactable = true;
        GameManager.UM.selectionUI.transform.GetChild(2).GetComponent<Button>().interactable = true;
        GameManager.ISM.closeButton.interactable = true;
        GameManager.UM.selectionUI.transform.GetChild(3).GetComponent<Button>().interactable = true;
        GameManager.MM.closeButton.interactable = true;
        GameManager.IPM.nextWeekEnabled = true;

        foreach (MarketItem item in GameManager.MM.itemsInMarket)
        {
            item.transactionAmountInput.interactable = true;
            item.expandButton.interactable = true;
        }
    }

    private void UpdateQuest(string questTitle, string questDescription)
    {
        questTitleText.SetText(questTitle);
        questDescriptionText.SetText(questDescription);
    }

    public void LoadData(GameData data)
    {
        questCount = data.questCount;
        if (data.questActive)
        {
            InitializeQuestCards();
            GameManager.WM.OpenQuestWindow();
            questCount -= 1;
            NextQuest();
        }
    }

    public void SaveData(ref GameData data)
    {
        data.questActive = questActive;
        data.questCount = questCount;
    }
}