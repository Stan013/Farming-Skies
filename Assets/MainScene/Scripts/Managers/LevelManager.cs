using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour, IDataPersistence
{
    public TMP_Text farmLevelText;
    public Image farmLevelIcon;
    public int farmLevelMax;
    public int farmLevelMin;

    public Sprite farmLevel1;
    public Sprite farmLevel2;
    public Sprite farmLevel3;
    public Sprite farmLevel4;

    private int _farmLevel;
    public int FarmLevel
    {
        get => _farmLevel;
        set
        {
            _farmLevel = value;
            SetFarmLevel();
        }
    }

    public void SetFarmLevel()
    {
        if(_farmLevel == 0)
        {
            farmLevelIcon.sprite = farmLevel1;
            farmLevelMin = 0;
            farmLevelMax = 1;
        }
        else if(_farmLevel > 1)
        {
            farmLevelIcon.sprite = farmLevel1;
            farmLevelMin = 1;
            farmLevelMax = 5;
        }
        else if (_farmLevel > 5)
        { 
            farmLevelIcon.sprite = farmLevel2;
            farmLevelMin = 5;
            farmLevelMax = 25;
        }
        else if (_farmLevel > 25)
        { 
            farmLevelIcon.sprite = farmLevel3;
            farmLevelMin = 25;
            farmLevelMax = 50;
        }
        else if (_farmLevel > 50)
        { 
            farmLevelIcon.sprite = farmLevel4;
            farmLevelMin = 50;
            farmLevelMax = 100;
        }
        farmLevelText.text = "Level " + _farmLevel.ToString();

        if(_farmLevel == farmLevelMax)
        {
            GameManager.TAM.GenerateInflation();
        }
    }

    public void LoadData(GameData data)
    {
        FarmLevel = data.farmLevel;
    }

    public void SaveData(ref GameData data)
    {
        data.farmLevel = _farmLevel;
    }
}
