using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void OnStartNewGameButtonPressed()
    {
        DataPersistenceManager.dataManager.NewGame();
    }

    public void OnLoadGameButtonPressed()
    {
        DataPersistenceManager.dataManager.LoadGame();
    }

    public void OnSaveGameButtonPressed()
    {
        DataPersistenceManager.dataManager.SaveGame();
    }
}
