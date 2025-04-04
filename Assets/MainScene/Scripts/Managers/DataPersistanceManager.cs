using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class DataPersistenceManager : MonoBehaviour
{
    public string fileName;
    private string selectedProfileId = "test";
    private FileDataHandler dataHandler;
    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    public static DataPersistenceManager dataManager { get; private set; }

    private void Awake()
    {
        if (dataManager != null && dataManager != this)
        {
            Destroy(gameObject);
            return;
        }

        dataManager = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
        InitializeGame();
    }

    public void InitializeGame()
    {
        GameManager.SPM.Spawn();
        GameManager.UM.FarmLevel = this.gameData.farmLevel;
        GameManager.UM.Expense = this.gameData.expense;
        GameManager.UM.Balance = this.gameData.balance;
        GameManager.UM.Water = this.gameData.water;
        GameManager.UM.Fertiliser = this.gameData.fertiliser;
        GameManager.UM.Weeks = this.gameData.weeks;
        GameManager.ISM.SetupIslands();
        GameManager.CM.SetupCards();
        GameManager.DM.SetStartingDeck();
        GameManager.MM.SetupMarket();
        ;
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load(selectedProfileId);
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void SaveGame() 
    {
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }
        dataHandler.Save(gameData, selectedProfileId);
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }
}
