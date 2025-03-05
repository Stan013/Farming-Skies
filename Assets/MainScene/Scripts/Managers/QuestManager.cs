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
                    GameManager.UM.taxAmountText.transform.parent.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
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
                    if (GameManager.UM.openUIButton.GetComponent<Image>().color == Color.white)
                    {
                        QuestCompleted = true;
                    }
                    break;
                case 4:
                    if (GameManager.UM.taxAmountText.transform.parent.GetComponent<Image>().color == Color.white)
                    {
                        QuestCompleted = true;
                    }
                    break;
            }
        }
    }
}