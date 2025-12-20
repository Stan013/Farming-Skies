using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IslandManager : MonoBehaviour, IDataPersistence
{
    [Header("GameManager objects")]
    public Transform availableIslandsParent;

    [Header("Island lists")]
    public List<Island> allIslands;
    public List<Island> availableIslands;
    public List<Island> boughtIslands;

    [Header("General island variables")]
    public Island starterIsland;
    public string islandManageTab;
    public Button closeButton;
    private int _islandValue;

    public int IslandValue
    {
        get => _islandValue;
        set
        {
            _islandValue = value;
            GameManager.EM.UpdateFarmValue();
        }
    }
    public int oldIslandValue;
    public int islandValueChange;

    [Header("Island management variables")]
    public GameObject islandCamera;
    public Island centerIsland;
    public GameObject islandInformation;
    public TMP_Text islandName;
    public TMP_Text nutrientText;
    public TMP_Text waterNeed;
    public TMP_Text nitrogenNeed;
    public TMP_Text phosphorusNeed;
    public TMP_Text potassiumNeed;

    [Header("Plot management variables")]
    public GameObject plotInformation;
    public TMP_Text smallPlotsAvailable;
    public TMP_Text mediumPlotsAvailable;
    public TMP_Text largePlotsAvailable;
    public TMP_Text smallPlants;
    public TMP_Text mediumPlants;
    public TMP_Text largePlants;

    [Header("Expense variables")]
    public ExpenseItem expenseItem;
    public GameObject islandExpenseContent;
    public GameObject waterBarrelExpenseContent;
    public GameObject compostBinExpenseContent;

    public void SetupIslands()
    {
        foreach(Transform islandRing in availableIslandsParent)
        {
            foreach (Island childIsland in islandRing.GetComponentsInChildren<Island>())
            {
                if (childIsland.islandAvailable)
                {
                    availableIslands.Add(childIsland);
                }

                childIsland.currentState = Island.IslandState.Transparent;
                childIsland.previousState = Island.IslandState.Transparent;
                childIsland.topMat = childIsland.islandTop.GetComponent<Renderer>().material;
                childIsland.bottomMat = childIsland.islandBottom.GetComponent<Renderer>().material;
            }
        }
        SetupIslandCollisions(false);
    }

    public void SetupIslandCollisions(bool active)
    {
        foreach (Island island in GameManager.ISM.boughtIslands)
        {
            island.GetComponent<BoxCollider>().enabled = active;
        }
    }

    public Island GetPotentialBoughtIsland()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, ~0, QueryTriggerInteraction.Collide);
        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent(out Island island))
            {
                if (island.islandBought)
                {
                    return island;
                }
            }
        }
        return null;
    }

    public Island GetPotentialIsland()

    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, ~0, QueryTriggerInteraction.Collide);
        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent(out Island island))
            {
                if(island.islandBought)
                {
                    return null;
                }
                else
                {
                    return island;
                }
            }
        }
        return null;
    }

    public void AddIslandToBought(Island reconstructedIsland)
    {
        reconstructedIsland.islandBought = true;
        reconstructedIsland.currentState = Island.IslandState.Sowed;
        boughtIslands.Add(reconstructedIsland);
        GameManager.LM.FarmLevel += 1;
        IslandValue += reconstructedIsland.islandExpenseCost;
        GameManager.EM.AddExpenseIsland(reconstructedIsland);
        SetAvailableIslands(reconstructedIsland.islandID);
    }

    public void SetAvailableIslands(string id)
    {
        string cleanInput = id.Trim('(', ')');
        string[] coordinates = cleanInput.Split(',');
        int x = int.Parse(coordinates[0]);
        int y = int.Parse(coordinates[1]);

        List<Vector2Int> neighborCoordinates = new List<Vector2Int>
        {
            new Vector2Int(x + 1, y), // Right
            new Vector2Int(x - 1, y), // Left
            new Vector2Int(x, y + 1), // Up
            new Vector2Int(x, y - 1), // Down
        };

        foreach (var coord in neighborCoordinates)
        {
            string islandID = $"({coord.x},{coord.y})";
            Island island = FindIslandByID(islandID);
            island.islandAvailable = true;
            if (!availableIslands.Contains(island))
            {
                availableIslands.Add(island);
            }
        }
    }

    public void OpenIslandManagement(string tab)
    {
        if(centerIsland == null)
        {
            centerIsland = starterIsland;
        }

        if(boughtIslands.Count != 0)
        {
            islandCamera.SetActive(true);
            switch (tab)
            {
                case "Available":
                    nutrientText.text = "Available Nutrients: ";
                    islandInformation.SetActive(true);
                    plotInformation.SetActive(false);
                    islandName.text = "Island " + centerIsland.islandID + ":";
                    waterNeed.text = centerIsland.nutrientsAvailable[0].ToString();
                    nitrogenNeed.text = centerIsland.nutrientsAvailable[1].ToString();
                    phosphorusNeed.text = centerIsland.nutrientsAvailable[2].ToString();
                    potassiumNeed.text = centerIsland.nutrientsAvailable[3].ToString();
                    break;
                case "Required":
                    nutrientText.text = "Required Nutrients: ";
                    islandInformation.SetActive(true);
                    plotInformation.SetActive(false);
                    islandName.text = "Island " + centerIsland.islandID + ":";
                    waterNeed.text = centerIsland.nutrientsRequired[0].ToString();
                    nitrogenNeed.text = centerIsland.nutrientsRequired[1].ToString();
                    phosphorusNeed.text = centerIsland.nutrientsRequired[2].ToString();
                    potassiumNeed.text = centerIsland.nutrientsRequired[3].ToString();
                    break;
                case "Plot":
                    islandInformation.SetActive(false);
                    plotInformation.SetActive(true);
                    islandName.text = "Island " + centerIsland.islandID + ":";
                    smallPlotsAvailable.text = centerIsland.availableSmallPlots.Count.ToString();
                    mediumPlotsAvailable.text = centerIsland.availableMediumPlots.Count.ToString();
                    largePlotsAvailable.text = centerIsland.availableLargePlots.Count.ToString();
                    smallPlants.text = centerIsland.smallPlantsOnIsland.Count.ToString();
                    mediumPlants.text = centerIsland.mediumPlantsOnIsland.Count.ToString();
                    largePlants.text = centerIsland.largePlantsOnIsland.Count.ToString();
                    break;
            }
        }
        else
        {
            islandCamera.SetActive(false);
            islandInformation.SetActive(false);
            plotInformation.SetActive(false);
        }
    }

    public Island FindIslandByID(string islandID)
    {
        return allIslands.Find(island => island.islandID == islandID);
    }

    public void LoadData(GameData data)
    {
        for (int i = 0; i < data.islandsMap.Count; i++)
        {
            Island island = FindIslandByID(data.islandsMap[i].islandID);
            island.LoadIslandData(data.islandsMap[i]);
            availableIslands.Add(island);
            if (island.islandBought)
            {
                boughtIslands.Add(island);
            }
        }
        GameManager.ISM.SetupIslandCollisions(true);
        IslandValue = data.islandValue;
    }

    public void SaveData(ref GameData data)
    {
        data.islandsMap.Clear();
        foreach (Island island in availableIslands)
        {
            data.islandsMap.Add(island.SaveIslandData());
        }
        data.islandValue = IslandValue;
    }
}
