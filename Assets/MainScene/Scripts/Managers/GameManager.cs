using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    public static DataPersistenceManager DPM { get; private set; }
    public static SpawnManager SPM { get; private set; }
    public static WindowManager WM { get; private set; }
    public static QuestManager QM { get; private set; }
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
    public static CraftManager CRM { get; private set; }

    [Header("Debug")]
    public static DebugManager DBM { get; private set; }

    [Header("Game modes")]
    public static GameMode CurrentMode;
    public enum GameMode { Career, Scenario, Creative }

    private void Awake()
    {
        DBM = GetComponent<DebugManager>();
        SPM = GetComponent<SpawnManager>();
        WM = GetComponent<WindowManager>();
        UM = GetComponent<UIManager>();
        QM = GetComponent<QuestManager>();
        IPM = GetComponent<InputManager>();
        ISM = GetComponent<IslandManager>();
        DM = GetComponent<DeckManager>();
        HM = GetComponent<HandManager>();
        CM = GetComponent<CardManager>();
        TM = GetComponent<TimeManager>();
        INM = GetComponent<InventoryManager>();
        PM = GetComponent<PlantManager>();
        CRM = GetComponent<CraftManager>();
        MM = GetComponent<MarketManager>();
        SM = GetComponent<SelectionManager>();
        DPM = GetComponent<DataPersistenceManager>();
    }

    private void StartGame(string gameAction)
    {
        if (gameAction == "LoadGame")
        {
            DPM.LoadGame();
        }
        else
        {
            DPM.NewGame();
        }
    }

    private void Update()
    {
        if (!WM.mainWindow.activeSelf) 
        {
            if(!WM.advanceWindow.activeSelf)
            {
                TM.RotateSky(3f);
                IPM.KeyboardInput();
                IPM.MouseInput();
                QM.QuestCheck();

                DBM.DebugInput(); // Debug mode
            }
            else
            {
                TM.RotateSky(30f);
            }
        }
    }

/*    public static void SetState(GameState newState)
    {
        ExitState(CurrentState);
        CurrentState = newState;
        EnterState(CurrentState);
    }*/

/*    private static void EnterState(GameState state)
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
                if(UM.UIMenu.activeSelf)
                {
                    UM.UIbutton.OpenUIMenu();
                }
                if(QM.questMenu.activeSelf)
                {
                    UM.questButton.OpenQuestMenu();
                }
                UM.infoMenu.SetActive(false);
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
                cam.transform.position = new Vector3(0f, 10f, 0f);
                break;
            case GameState.Default:
                IPM.rb.velocity = Vector3.zero;
                IPM.rb.angularVelocity = Vector3.zero;
                Cursor.visible = true;
                break;
            case GameState.SettingsMode:
                break;
            case GameState.TimeMode:
                if(TTM.tutorial)
                {
                    UM.questButton.OpenQuestMenu();
                }
                cam.transform.position = staticPos;
                TM.timeWindow.SetActive(false);
                UM.UIbutton.gameObject.SetActive(true);
                UM.questButton.gameObject.SetActive(true);
                UM.nextWeekButton.SetActive(true);
                UM.modeIndicator.transform.parent.gameObject.SetActive(true);
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
    }*/

/*    public void LoadData(GameData data)
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
    }*/
}