using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenStartMenu : MonoBehaviour
{
    public Button startButton;
    public GameObject startWindow;

    public void OpenGameSelection()
    {
        startWindow.SetActive(true);
    }

    public void LoadGame()
    {

    }

    public void SelectGameMode(string gameMode)
    {
        switch (gameMode)
        {
            case "Campaign":
                GameManager.CurrentMode = GameManager.GameMode.Campaign;
                break;
            case "Creative":
                GameManager.CurrentMode = GameManager.GameMode.Creative;
                break;
            case "Scenario":
                GameManager.CurrentMode = GameManager.GameMode.Scenario;
                break;
        }
        GameManager.IPM.ToggleState(GameManager.GameState.ManageMode, GameManager.GameState.MainMenuMode);
        GameManager.DPM.NewGame();
    }
}
