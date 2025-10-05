using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [Header("Game windows")]
    public bool inMenu;
    public GameObject mainWindow;
    public GameObject gameWindow;
    public GameObject settingsWindow;
    public GameObject tutorialWindow;
    public GameObject questWindow;
    public GameObject advanceWindow;
    public GameObject inventoryWindow;
    public GameObject manageWindow;
    public GameObject craftWindow;
    public GameObject marketWindow;
    public GameObject expenseWindow;
    public GameObject eventWindow;
    public GameObject selectionWindow;

    [Header("Quest window variables")]
    public float windowTransitionDuration = 0.5f;

    public void StartGame(bool loadGame)
    {
        if (loadGame)
        {
            GameManager.DPM.LoadGame();
        }
    }

    public void SelectGameMode(string gameMode)
    {
        switch (gameMode)
        {
            case "Career":
                GameManager.CurrentMode = GameManager.GameMode.Career;
                GameManager.DPM.NewGame();
                break;
            case "Scenario":
                GameManager.CurrentMode = GameManager.GameMode.Scenario;
                GameManager.DPM.NewGame();
                break;
            case "Creative":
                GameManager.CurrentMode = GameManager.GameMode.Creative;
                GameManager.DPM.NewGame();
                break;
        }
    }

    public void OpenWindow()
    {
        inMenu = true;
        settingsWindow.SetActive(false);
        inventoryWindow.SetActive(false);
        manageWindow.SetActive(false);
        craftWindow.SetActive(false);
        marketWindow.SetActive(false);
        expenseWindow.SetActive(false);
    }

    public void CloseWindow()
    {
        inMenu = false;
    }

    public void OpenQuestWindow()
    {
        if (questWindow == null) return;

        RectTransform rect = questWindow.GetComponent<RectTransform>();
        questWindow.SetActive(true);
        StartCoroutine(MoveDown(rect, 415f));
    }

    private IEnumerator MoveDown(RectTransform window, float targetY)
    {
        float startY = window.anchoredPosition.y;
        float elapsedTime = 0f;

        while (elapsedTime < windowTransitionDuration)
        {
            float newY = Mathf.Lerp(startY, targetY, elapsedTime / windowTransitionDuration);
            window.anchoredPosition = new Vector2(window.anchoredPosition.x, newY);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        window.anchoredPosition = new Vector2(window.anchoredPosition.x, targetY);
    }

    public void OpenSettings()
    {
        if(settingsWindow.activeSelf)
        {
            settingsWindow.SetActive(false);
            inMenu = false;
        }
        else
        {
            GameManager.HM.HideCardsInHand(false);
            settingsWindow.SetActive(true);
            inMenu = true;
        }
    }

    public void OpenSelectionWindow()
    {
        if (selectionWindow.activeSelf)
        {
            selectionWindow.SetActive(false);
            inMenu = false;
        }
        else
        {
            GameManager.SM.SetupSelection();
            selectionWindow.SetActive(true);
            inMenu = true;
        }
    }
}
