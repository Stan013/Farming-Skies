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
        SettingsMode,
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
            IPM.HandleGameStatesSwitchInput();
            IPM.HandleKeyboardInput(CurrentState);
            IPM.HandleMouseInput(CurrentState);
            if(TTM.tutorialCount == 11)
            {
                if(cam.transform.position.y > 7)
                {
                    TTM.QuestCompleted = true;
                }
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
            case GameState.Default:
                Cursor.visible = false;
                break;
            case GameState.ManageMode:
                break;
            case GameState.SelectionMode:
                HM.ClearCardsInHand();
                SM.GeneratePickWindow();
                break;
            case GameState.InventoryMode:
                UM.openInventoryButton.GetComponent<OpenButton>().OnKeyboardButtonClick("Inventory");
                INM.UpdateInventoryItems();
                break;
            case GameState.MarketMode:
                UM.openMarketButton.GetComponent<OpenButton>().OnKeyboardButtonClick("Market");
                MM.UpdateMarketItems();
                break;
            case GameState.CraftMode:
                UM.openCraftButton.GetComponent<OpenButton>().OnKeyboardButtonClick("Craft");
                CRM.SetupCraftingMode();
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
            case GameState.ManageMode:
                break;
            case GameState.SettingsMode:
                break;
            case GameState.SelectionMode:
                break;
            case GameState.InventoryMode:
                UM.closeButton.GetComponent<CloseButton>().closeWindow = INM.inventoryWindow;
                UM.closeButton.GetComponent<CloseButton>().OnButtonClick();
                break;
            case GameState.MarketMode:
                UM.closeButton.GetComponent<CloseButton>().closeWindow = MM.marketWindow;
                UM.closeButton.GetComponent<CloseButton>().OnButtonClick();
                break;
            case GameState.CraftMode:
                UM.closeButton.GetComponent<CloseButton>().closeWindow = CRM.craftWindow;
                UM.closeButton.GetComponent<CloseButton>().OnButtonClick();
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