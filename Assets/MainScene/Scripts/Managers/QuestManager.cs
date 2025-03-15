using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public GameObject questMenu;
    public Image questCompletedIcon;
    private bool _questCompleted;
    public bool QuestCompleted
    {
        get { return _questCompleted; }
        set
        {
            if (_questCompleted != value)
            {
                _questCompleted = value;
                OnQuestCompletedChanged();
            }
        }
    }

    private IEnumerator ResetQuestCompletedAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        QuestCompleted = false;
        GameManager.TTM.NextTutorial();
    }

    private void OnQuestCompletedChanged()
    {
        if (questMenu != null)
        {
            Image questMenuImage = questMenu.GetComponent<Image>();
            if (questMenuImage != null)
            {
                if (QuestCompleted)
                {
                    questCompletedIcon.gameObject.SetActive(true);
                    StartCoroutine(ResetQuestCompletedAfterDelay());
                }
                else
                {
                    questCompletedIcon.gameObject.SetActive(false);
                }
            }
        }
    }

    public void QuestCheck()
    {
        if (GameManager.TTM.tutorial)
        {
            switch (GameManager.TTM.tutorialCount)
            {
                case 1:
                    if(GameManager.CurrentState == GameManager.GameState.ManageMode)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 2:
                    if(GameManager.ISM.FindIslandByID("Starter(0,0)").islandBought)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 3:
                    if (GameManager.UM.UIMenu.activeSelf)
                    {
                        GameManager.TTM.tutorialCount = 45; // Testing
                        QuestCompleted = true;
                    }
                    break;
                case 4:
                    if (GameManager.UM.expenseAmountText.transform.parent.GetComponent<Image>().color == Color.white)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 5:
                    if (GameManager.UM.dateAmountText.transform.parent.GetComponent<Image>().color == Color.white)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 6:
                    if (GameManager.ISM.FindIslandByID("Starter(0,0)").currentState == Island.IslandState.Cultivated && !GameManager.HM.dragging)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 7:
                    if (GameManager.ISM.FindIslandByID("Starter(0,0)").currentState == Island.IslandState.Watered && !GameManager.HM.dragging)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 8:
                    if (GameManager.HM.FindCardInHandById("CardGreenBeanPlant").hasBeenInspected)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 9:
                    if (GameManager.HM.FindCardInHandById("CardChivePlant").hasBeenInspected)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 10:
                    if (GameManager.HM.FindCardInHandById("CardChardPlant").hasBeenInspected && GameManager.HM.FindCardInHandById("CardRicePlant").hasBeenInspected)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 11:
                    if (GameManager.ISM.FindIslandByID("Starter(0,0)").itemsOnIsland.Count == 1)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 12:
                    if (GameManager.ISM.FindIslandByID("Starter(0,0)").itemsOnIsland.Count == 2)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 13:
                    if (GameManager.ISM.FindIslandByID("Starter(0,0)").itemsOnIsland.Count == 4)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 14:
                    if (GameManager.TM.GetDate() == "08-01-2025")
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 15:
                    if(GameManager.CurrentState == GameManager.GameState.InventoryMode)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 16:
                    if (GameManager.CurrentState == GameManager.GameState.ManageMode)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 17:
                    if(GameManager.ISM.islandMenu.activeSelf)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 18:
                    if (GameManager.ISM.islandMenu.GetComponent<IslandInfoUI>().availableInfo.activeSelf)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 19:
                    if (!GameManager.ISM.islandMenu.activeSelf)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 20:
                    if (GameManager.CurrentState == GameManager.GameState.CraftMode)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 21:
                    if (GameManager.CRM.selectedCard.cardName == "Nitrogen Fertiliser")
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 22:
                    if (GameManager.CRM.cardCraftAmount == 1)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 23:
                    if (GameManager.UM.UIMenu.activeSelf)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 24:
                    if (GameManager.DM.cardsInDeck.Count == 1)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 25:
                    if (GameManager.CRM.selectedCard.cardName == "Phosphorus Fertiliser")
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 26:
                    if (GameManager.CRM.cardCraftAmount == 1)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 27:
                    if (GameManager.DM.cardsInDeck.Count == 2)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 28:
                    if (GameManager.CRM.selectedCard.cardName == "Potassium Fertiliser")
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 29:
                    if (GameManager.CRM.cardCraftAmount == 1)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 30:
                    if (GameManager.DM.cardsInDeck.Count == 3)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 31:
                    if (GameManager.CurrentState == GameManager.GameState.ManageMode)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 32:
                    if (GameManager.ISM.starterIsland.nutrientsAvailable[1] != 0)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 33:
                    if (GameManager.ISM.starterIsland.nutrientsAvailable[2] != 0 && GameManager.ISM.starterIsland.nutrientsAvailable[3] != 0)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 34:
                    if (GameManager.ISM.islandMenu.activeSelf)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 35:
                    if (GameManager.CurrentState == GameManager.GameState.MarketMode)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 36:
                    if (GameManager.MM.marketScrollbar.value < 0.1)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 37:
                    if (GameManager.MM.FindMarketItemByName("Rice").sellUI.inputAmount == GameManager.MM.FindMarketItemByName("Rice").sellUI.GetComponent<MarketButton>().maxAmount)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 38:
                    if (GameManager.MM.FindMarketItemByName("Rice").attachedItemCard.itemQuantity == 0)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 39:
                    if (GameManager.CurrentState == GameManager.GameState.ManageMode && GameManager.UM.UIMenu.activeSelf)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 40:
                    if (GameManager.cam.transform.position.z < -6)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 41:
                    if (GameManager.cam.transform.position.y > 10)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 42:
                    if (GameManager.ISM.FindIslandByID("Ring1(0,-1)").islandBought)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 43:
                    if (GameManager.HM.FindCardInHandById("CardWaterBarrelBuildable") != null && GameManager.HM.FindCardInHandById("CardCompostBuildable") != null)
                    {
                        if (GameManager.HM.FindCardInHandById("CardWaterBarrelBuildable").hasBeenInspected && GameManager.HM.FindCardInHandById("CardCompostBuildable").hasBeenInspected)
                        {
                            QuestCompleted = true;
                        }
                    }
                    break;
                case 44:
                    if (GameManager.ISM.FindIslandByID("Ring1(0,-1)").itemsOnIsland.Count == 2)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 45:
                    if (GameManager.UM.infoMenu.activeSelf)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 46:
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
                    break;
            }
        }
    }
}