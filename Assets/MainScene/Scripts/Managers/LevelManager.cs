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
        if(_farmLevel >= 1)
        {
            farmLevelIcon.sprite = farmLevel1;
            farmLevelMin = 0;
            farmLevelMax = 9;
        }
        else if (_farmLevel >= 9)
        { 
            farmLevelIcon.sprite = farmLevel2;
            farmLevelMin = 9;
            farmLevelMax = 25;
        }
        farmLevelText.text = "Level " + _farmLevel.ToString();
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
