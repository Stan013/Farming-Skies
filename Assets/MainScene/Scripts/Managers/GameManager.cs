﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IDataPersistence
{
    //Managers
    public static CardManager CM { get; private set; }
    public static HandManager HM { get; private set; }
    public static InventoryManager INM { get; private set; }
    public static DeckManager DM { get; private set; }
    public static IslandManager ISM { get; private set; }
    public static TimeManager TM { get; private set; }
    public static UIManager UM { get; private set; }
    public static InputManager IPM { get; private set; }
    public static MarketManager MM { get; private set; }
    public static PlantManager PM { get; private set; }
    public static EndRoundManager ERM { get; private set; }
    public static TutorialManager TTM { get; private set; }
    public static DataPersistenceManager DPM { get; private set; }
    public static Camera cam { get; private set; }
    public static Vector3 staticPos;
    public GameObject mainMenu;
    private static GameObject staticMainMenu;
    public GameObject gameMenu;
    private static GameObject staticGameMenu;
    public enum GameMode
    {
        Default,
        Campaign,
        Freeplay,
    }
    public static GameMode CurrentMode { get; private set; }
    public enum GameState
    {
        MainMenuMode,
        Default,
        ManageMode,
        MenuMode,
        EndRoundMode,
        ShopMode,
        InventoryMode,
        MarketMode,
    }
    public static GameState CurrentState { get; private set; }

    private void Awake()
    {
        HM = GetComponent<HandManager>();
        ERM = GetComponent<EndRoundManager>();
        CM = GetComponent<CardManager>();
        DM = GetComponent<DeckManager>();
        ISM = GetComponent<IslandManager>();
        TM = GetComponent<TimeManager>();
        UM = GetComponent<UIManager>();
        IPM = GetComponent<InputManager>();
        MM = GetComponent<MarketManager>();
        INM = GetComponent<InventoryManager>();
        PM = GetComponent<PlantManager>();
        TTM = GetComponent<TutorialManager>();
        DPM = GetComponent<DataPersistenceManager>();
        cam = FindAnyObjectByType<Camera>();
        staticPos = cam.transform.position;
        CurrentState = GameState.MainMenuMode;
        staticMainMenu = mainMenu;
        staticGameMenu = gameMenu;
    }

    public void StartGame(string gameAction)
    {
        IPM.ToggleState(GameState.Default, GameState.Default);
        if (gameAction == "LoadGame")
        {
            DPM.LoadGame();
        }
        else
        {
            if (gameAction == "NewGame")
            {
                if(TTM.tutorial)
                {
                    TTM.StartTutorial();
                }
                else
                {
                    HM.SetStartingHand();
                    UM.SetUIButtons(true, UM.openUIButton);
                    ISM.SetPurchasableIslands(true);
                }
                DPM.NewGame();
                SetState(GameState.ManageMode);
            }
        }
    }

    private static void MainMenuStatus(bool mainMenuStatus)
    {
        staticMainMenu.SetActive(mainMenuStatus);
        staticGameMenu.SetActive(!mainMenuStatus);
    }

    private void Update()
    {
        if (CurrentState != GameState.MainMenuMode) 
        {
            IPM.HandleGameStatesSwitchInput();
            IPM.HandleKeyboard(CurrentState);
            IPM.HandleMouse(CurrentState);
        }
    }

    public static void SetState(GameState newState)
    {
        ExitState(CurrentState);
        CurrentState = newState;
        EnterState(CurrentState);
    }

    private static void EnterState(GameState state)
    {
        UM.UpdateUI();
        staticPos = cam.transform.position;
        switch (state)
        {
            case GameState.MainMenuMode:
                MainMenuStatus(true);
                Cursor.visible = true;
                break;
            case GameState.Default:
                break;
            case GameState.ManageMode:
                Cursor.visible = true;
                IPM.rb.velocity = Vector3.zero;
                IPM.rb.angularVelocity = Vector3.zero;
                if (!TTM.tutorial)
                {
                    ISM.SetPurchasableIslands(true);
                }
                else
                {
                    ISM.availableIslands[0].islandCanBought = true;
                }
                break;
            case GameState.MenuMode:
                Cursor.visible = true;
                break;
            case GameState.EndRoundMode:
                HM.ClearCardsInHand();
                ERM.GeneratePickWindow();
                break;
            case GameState.ShopMode:
                break; 
            case GameState.InventoryMode:
                Cursor.visible = true;
                UM.openQuestButton.transform.gameObject.SetActive(false);
                UM.openUIButton.transform.gameObject.SetActive(false);
                INM.UpdateInventoryItems();
                break;
            case GameState.MarketMode:
                UM.openMarketButton.onClick.Invoke();
                UM.openQuestButton.transform.gameObject.SetActive(false);
                UM.openUIButton.transform.gameObject.SetActive(false);
                MM.UpdateMarketItems();
                break;
        }
    }

    private static void ExitState(GameState state)
    {
        switch (state)
        {
            case GameState.MainMenuMode:
                MainMenuStatus(false);
                break;
            case GameState.Default:
                break;
            case GameState.ManageMode:
                Cursor.visible = false;
                ISM.SetPurchasableIslands(false);
                break;
            case GameState.MenuMode:
                break;
            case GameState.EndRoundMode:
                break;
            case GameState.ShopMode:
                break;
            case GameState.InventoryMode:
                Cursor.visible = false;
                UM.openQuestButton.transform.gameObject.SetActive(true);
                UM.openUIButton.transform.gameObject.SetActive(true);
                INM.CloseWindow();
                break;
            case GameState.MarketMode:
                Cursor.visible = false;
                UM.openQuestButton.transform.gameObject.SetActive(true);
                UM.openUIButton.transform.gameObject.SetActive(true);
                MM.CloseWindow();
                break;
        }
    }

    public void LoadData(GameData data)
    {
        CurrentState = data.CurrentState;
        staticPos = data.playerPosition;
        cam.transform.forward = data.cameraDirection;
    }

    public void SaveData(ref GameData data)
    {
        data.CurrentState = CurrentState;
        data.playerPosition = staticPos;
        data.cameraDirection = cam.transform.forward;
    }
}