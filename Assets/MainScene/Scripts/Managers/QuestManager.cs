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
                    questMenuImage.color = new Color(0.8f, 1.0f, 0.4f, 0.8f);
                    StartCoroutine(ResetQuestCompletedAfterDelay());
                }
                else
                {
                    questCompletedIcon.gameObject.SetActive(false);
                    questMenuImage.color = new Color(0.9f, 0.9f, 0.9f, 0.8f);
                    GameManager.UM.expenseAmountText.transform.parent.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
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
                    if (GameManager.UM.UIbutton.GetComponent<Image>().color == Color.white)
                    {
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
                    if (GameManager.ISM.FindIslandByID("Starter(0,0)").currentState == Island.IslandState.Cultivated)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 7:
                    if (GameManager.ISM.FindIslandByID("Starter(0,0)").currentState == Island.IslandState.Watered)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 8:
                    if (GameManager.CM.inspectCard != null)
                    {
                        if(GameManager.CM.inspectCard.GetComponent<Image>().color == Color.green)
                        {
                            GameManager.CM.inspectCard.GetComponent<Image>().color = new Color(0.74f, 0.74f, 0.74f); // Gray
                        }
                    }
                    if (GameManager.HM.FindCardInHandById("CardGreenBeanPlant").GetComponent<Image>().color != Color.green)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 9:
                    if (GameManager.CM.inspectCard != null)
                    {
                        if (GameManager.CM.inspectCard.GetComponent<Image>().color == Color.green)
                        {
                            GameManager.CM.inspectCard.GetComponent<Image>().color = new Color(0.74f, 0.74f, 0.74f); // Gray
                        }
                    }
                    if (GameManager.HM.FindCardInHandById("CardChivePlant").GetComponent<Image>().color != Color.green)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 10:
                    if (GameManager.CM.inspectCard != null)
                    {
                        if (GameManager.CM.inspectCard.GetComponent<Image>().color == Color.green)
                        {
                            GameManager.CM.inspectCard.GetComponent<Image>().color = new Color(0.74f, 0.74f, 0.74f); // Gray
                        }
                    }
                    if (GameManager.HM.FindCardInHandById("CardChardPlant").GetComponent<Image>().color != Color.green && GameManager.HM.FindCardInHandById("CardRicePlant").GetComponent<Image>().color != Color.green)
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
                    if(GameManager.ISM.islandMenu.activeSelf == true)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 18:
                    if (GameManager.ISM.islandMenu.GetComponent<IslandInfoUI>().availableInfo.activeSelf == true)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 19:
                    if (GameManager.ISM.islandMenu.activeSelf != true)
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
                    if (GameManager.CRM.selectedCard == GameManager.CM.FindCardById("CardNitrogenFertilizerUtility"))
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 22:
                    if (GameManager.UM.UIMenu.activeSelf == true)
                    {
                        QuestCompleted = true;
                    }
                    break;
            }
        }
    }
}