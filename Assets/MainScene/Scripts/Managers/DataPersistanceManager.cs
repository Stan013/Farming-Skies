using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.IO;

public class DataPersistenceManager : MonoBehaviour
{
    private string currentSaveSlot;
    private FileDataHandler dataHandler;
    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    public GameManager gm;

    public void Awake()
    {
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        string savesPath = Path.Combine(Application.persistentDataPath, "Saves");
        dataHandler = new FileDataHandler(savesPath, "Save.game");

        if (string.IsNullOrEmpty(currentSaveSlot))
        {
            currentSaveSlot = "Save1";
        }
    }


    public void NewGame()
    {
        gameData = new GameData(GameManager.SPM.startBalance, GameManager.SPM.startWater, GameManager.SPM.startFertiliser, GameManager.SPM.startExpense);
        InitializeGame();
    }

    public void InitializeGame()
    {
        GameManager.LM.FarmLevel = gameData.farmLevel;
        GameManager.UM.Balance = gameData.balance;
        GameManager.UM.Water = gameData.water;
        GameManager.UM.Fertiliser = gameData.fertiliser;
        GameManager.EM.Expense = gameData.expense;
        GameManager.TM.Weeks = gameData.weeks;
        GameManager.HM.SetHandSlots();
        GameManager.ISM.SetupIslands();
        GameManager.CM.SetupCards();
        GameManager.DM.SetStartingDeck();
        GameManager.MM.SetupMarket();
        GameManager.EVM.SetupEvents(0);
        GameManager.SPM.Spawn();
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
        if (string.IsNullOrEmpty(currentSaveSlot))
        {
            Debug.LogError("No save slot selected!");
            return;
        }

        gameData = dataHandler.Load(currentSaveSlot);

        if (gameData == null)
        {
            Debug.LogWarning("Save not found, creating new data.");
            gameData = new GameData(GameManager.SPM.startBalance, GameManager.SPM.startWater, GameManager.SPM.startFertiliser, GameManager.SPM.startExpense);
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }

        InitializeGame();
    }

    public void SaveGame() 
    {
        if (string.IsNullOrEmpty(currentSaveSlot))
        {
            Debug.LogError("No save slot selected!");
            return;
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        dataHandler.Save(gameData, currentSaveSlot);
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
