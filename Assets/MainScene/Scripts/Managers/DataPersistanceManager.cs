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
    public GameManager gm;

    public void Awake()
    {
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
    }

    public void NewGame()
    {
        gameData = new GameData();
        InitializeGame();
    }

    public void InitializeGame()
    {
        GameManager.SPM.Spawn();
        GameManager.LM.FarmLevel = gameData.farmLevel;
        GameManager.UM.Balance = gameData.balance;
        GameManager.UM.Water = gameData.water;
        GameManager.UM.Fertiliser = gameData.fertiliser;
        GameManager.TM.Weeks = gameData.weeks;
        GameManager.HM.SetHandSlots();
        GameManager.ISM.SetupIslands();
        GameManager.CM.SetupCards();
        GameManager.DM.SetStartingDeck();
        GameManager.MM.SetupMarket();
    }
    public void ClearChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load(selectedProfileId);
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
        return gm.GetComponents<MonoBehaviour>().OfType<IDataPersistence>().ToList();
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }
}
