using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IDataPersistence
{
    [Header("Managers")]
    public static SpawnManager SPM { get; private set; }
    public static WindowManager WM { get; private set; }
    public static UIManager UM { get; private set; }
    public static InputManager IPM { get; private set; }
    public static IslandManager ISM { get; private set; }
    public static DeckManager DM { get; private set; }
    public static HandManager HM { get; private set; }
    public static CardManager CM { get; private set; }
    public static TimeManager TM { get; private set; }
    public static InventoryManager INM { get; private set; }
    public static PlantManager PM { get; private set; }
    public static CraftManager CRM { get; private set; }
    public static MarketManager MM { get; private set; }
    public static ExpenseManager EM { get; private set; }
    public static LevelManager LM { get; private set; }
    public static QuestManager QM { get; private set; }
    public static SelectionManager SM { get; private set; }

    [Header("Save/Load")]
    public static DataPersistenceManager DPM { get; private set; }

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
        EM = GetComponent<ExpenseManager>();
        LM = GetComponent<LevelManager>();
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

    public void LoadData(GameData data)
    {
        CurrentMode = data.gameMode;
    }

    public void SaveData(ref GameData data)
    {
        data.gameMode = CurrentMode;
    }
}