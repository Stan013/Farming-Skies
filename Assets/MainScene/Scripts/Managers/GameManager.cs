using System.Collections;
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
    public static SelectionManager SM { get; private set; }
    public static TutorialManager TTM { get; private set; }
    public static DataPersistenceManager DPM { get; private set; }
    public static CraftManager CRM { get; private set; }
    public static QuestManager QM { get; private set; }
    public static Camera cam { get; private set; }
    public static Vector3 staticPos;
    public GameObject mainMenu;
    private static GameObject staticMainMenu;
    public GameObject gameMenu;
    private static GameObject staticGameMenu;
    public enum GameMode
    {
        Campaign,
        Creative,
        Scenario,
    }
    public static GameMode CurrentMode { get; set; }
    public enum GameState
    {
        MainMenuMode,
        SettingsMode,
        TimeMode,
        Default,
        ManageMode,
        InventoryMode,
        MarketMode,
        CraftMode,
        SelectionMode,
    }
    public static GameState CurrentState { get; private set; }

    private void Awake()
    {
        HM = GetComponent<HandManager>();
        SM = GetComponent<SelectionManager>();
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
        CRM = GetComponent<CraftManager>();
        QM = GetComponent<QuestManager>();
        cam = FindAnyObjectByType<Camera>();
        staticPos = cam.transform.position;
        CurrentState = GameState.MainMenuMode;
        staticMainMenu = mainMenu;
        staticGameMenu = gameMenu;
    }

    private void StartGame(string gameAction)
    {
        IPM.ToggleState(GameState.ManageMode, GameState.Default);
        if (gameAction == "LoadGame")
        {
            DPM.LoadGame();
        }
        else
        {
            DPM.NewGame();
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
            if(CurrentState != GameState.TimeMode)
            {
                TM.RotateSky(1f);
                IPM.HandleGameStatesSwitchInput();
                IPM.HandleKeyboardInput(CurrentState);
                IPM.HandleMouseInput(CurrentState);
                QM.QuestCheck();
            }
            else
            {
                TM.RotateSky(30f);
            }
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
            case GameState.SettingsMode:
                break;
            case GameState.TimeMode:
                TM.timeWindow.SetActive(true);
                cam.transform.position = new Vector3(30f, 15f, -30f);
                cam.transform.rotation =  Quaternion.Euler(0f, -45f, 0f);
                UM.UIbutton.gameObject.SetActive(false);
                UM.questButton.gameObject.SetActive(false);
                UM.nextWeekButton.SetActive(false);
                UM.modeIndicator.transform.parent.gameObject.SetActive(false);
                UM.UIMenu.SetActive(false);
                UM.infoMenu.SetActive(false);
                QM.questMenu.SetActive(false);
                PM.Harvest();
                MM.UpdatePrices();
                TM.StartWeekCycle();
                UM.UpdateUI();
                break;
            case GameState.Default:
                UM.modeIndicator.sprite = UM.modeIcons[0];
                Cursor.visible = false;
                break;
            case GameState.ManageMode:
                UM.modeIndicator.sprite = UM.modeIcons[1];
                break;
            case GameState.SelectionMode:
                HM.ClearCardsInHand();
                SM.GeneratePickWindow();
                break;
            case GameState.InventoryMode:
                UM.openButton.ChangeMode(INM.inventoryWindow);
                UM.modeIndicator.sprite = UM.modeIcons[2];
                INM.UpdateInventoryItems();
                break;
            case GameState.CraftMode:
                UM.openButton.ChangeMode(CRM.craftWindow);
                UM.modeIndicator.sprite = UM.modeIcons[3];
                CRM.SetupCraftingMode();
                break;
            case GameState.MarketMode:
                UM.openButton.ChangeMode(MM.marketWindow);
                UM.modeIndicator.sprite = UM.modeIcons[4];
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
                IPM.rb.velocity = Vector3.zero;
                IPM.rb.angularVelocity = Vector3.zero;
                Cursor.visible = true;
                break;
            case GameState.SettingsMode:
                break;
            case GameState.TimeMode:
                cam.transform.position = staticPos;
                TM.timeWindow.SetActive(false);
                UM.UIbutton.gameObject.SetActive(true);
                UM.questButton.gameObject.SetActive(true);
                UM.nextWeekButton.SetActive(true);
                UM.modeIndicator.transform.parent.gameObject.SetActive(true);
                UM.UIMenu.SetActive(true);
                QM.questMenu.SetActive(true);
                break;
            case GameState.SelectionMode:
                break;
            case GameState.InventoryMode:
                UM.closeButton.ChangeModeToDefault(INM.inventoryWindow);
                break;
            case GameState.MarketMode:
                UM.closeButton.ChangeModeToDefault(MM.marketWindow);
                break;
            case GameState.CraftMode:
                UM.closeButton.ChangeModeToDefault(CRM.craftWindow);
                DM.CheckRefillHand();
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