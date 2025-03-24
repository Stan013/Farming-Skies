using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IslandManager : MonoBehaviour
{
    [Header("GameManager objects")]
    public Transform availableIslandsParent;

    [Header("Island lists")]
    public List<Island> availableIsland;
    public List<Island> boughtIslands;
    public List<Island> unboughtIslands;

    [Header("General island variables")]
    public Island starterIsland;

    [Header("Island management variables")]
    public Island centerIsland;
    public GameObject islandInformation;
    public GameObject plotInformation;
    public TMP_Text islandName;
    public TMP_Text waterNeed;
    public TMP_Text nitrogenNeed;
    public TMP_Text phosphorusNeed;
    public TMP_Text potassiumNeed;

    [Header("Expense variables")]
    public ExpenseItem expenseItem;
    public GameObject islandExpenseContent;
    public GameObject waterBarrelExpenseContent;
    public GameObject compostExpenseContent;

    public void SetupIslands()
    {
        foreach(Transform islandRing in availableIslandsParent)
        {
            foreach (Island childIsland in islandRing.GetComponentsInChildren<Island>())
            {
                if (childIsland.islandBought)
                {
                    boughtIslands.Add(childIsland);
                }
                else
                {
                    unboughtIslands.Add(childIsland);
                }

                childIsland.SetIslandState(Island.IslandState.Transparent);
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
                return island;
            }
        }
        return null;
    }

    public void AddIslandToBought(Island reconstructedIsland)
    {
        reconstructedIsland.islandBought = true;
        reconstructedIsland.SetIslandState(Island.IslandState.Sowed);
        boughtIslands.Add(reconstructedIsland);
        unboughtIslands.Remove(reconstructedIsland);
        ExpenseItem islandExpense = Instantiate(expenseItem, Vector3.zero, Quaternion.identity, islandExpenseContent.transform);
        islandExpense.transform.localPosition = new Vector3(islandExpense.transform.localPosition.x, islandExpense.transform.localPosition.y, 0);
        islandExpense.transform.localRotation = Quaternion.identity;
        islandExpense.SetupIslandExpense(reconstructedIsland);
    }

    public Island FindIslandByID(string islandID)
    {
        return availableIsland.Find(island => island.islandID == islandID);
    }

    public void OpenIslandManagement(string tab)
    {
        switch (tab)
        {
            case "Available":
                islandInformation.SetActive(true);
                plotInformation.SetActive(false);
                islandName.text = "Island " + centerIsland.islandID;
                waterNeed.text = centerIsland.nutrientsAvailable[1].ToString();
                nitrogenNeed.text = centerIsland.nutrientsAvailable[2].ToString();
                phosphorusNeed.text = centerIsland.nutrientsAvailable[3].ToString();
                potassiumNeed.text = centerIsland.nutrientsAvailable[4].ToString();
                break;
            case "Required":
                islandInformation.SetActive(true);
                plotInformation.SetActive(false);
                islandName.text = "Island " + centerIsland.islandID;
                waterNeed.text = centerIsland.nutrientsRequired[1].ToString();
                nitrogenNeed.text = centerIsland.nutrientsRequired[2].ToString();
                phosphorusNeed.text = centerIsland.nutrientsRequired[3].ToString();
                potassiumNeed.text = centerIsland.nutrientsRequired[4].ToString();
                break;
            case "Plots":
                islandInformation.SetActive(false);
                plotInformation.SetActive(true);
                break;
        }
    }

/*    public void LoadData(GameData data)
    {
        boughtIslands.Clear();
        var islandDataCount = 0;
        foreach (string islandID in data.boughtIslands)
        {
            var island = FindIslandByID(islandID);
            unboughtIslands.Remove(island);
            boughtIslands.Add(island);
            island.islandBought = true;
            island.islandBottom.GetComponent<Renderer>().material = island.bottomDefaultMat;
            island.ToggleState(Island.IslandState.Sowed, Island.IslandState.Default);
            island.usedPlots.Clear();
            island.itemsOnIsland.Clear();
            if (data.islandDataMap[islandDataCount].islandId == island.islandID)
            {
                foreach (string plotName in data.islandDataMap[islandDataCount].usedPlotNames)
                {
                    var usedPlot = island.FindPlotByName(plotName);
                    var plantCard = island.FindItemOnIslandByCardId(data.islandDataMap[islandDataCount].itemsOnIsland[islandDataCount]);
                    GameObject plant = Instantiate(plantCard.GetComponent<CardDrag>().dragModel, Vector3.zero, Quaternion.identity, usedPlot.transform);
                    plant.transform.localPosition = new Vector3(0, -0.25f, 0);
                    plant.transform.localRotation = Quaternion.identity;
                    island.MakeUsedPlot(usedPlot, GameManager.HM.dragCard, plant);
                }
                islandDataCount++;
            }
        }

    }

    public void SaveData(ref GameData data)
    {
        data.boughtIslands.Clear();
        data.islandDataMap.Clear();
        foreach (Island island in boughtIslands)
        {
            List<string> usedPlotNamesList = new List<string>();
            foreach (GameObject plot in island.usedPlots)
            {
                usedPlotNamesList.Add(plot.name);
            }
            List<string> plantIdList = new List<string>();
            foreach (Plant plant in island.itemsOnIsland)
            {
                plantIdList.Add(plant.plantCardID);
            }
            IslandData islandData = new IslandData(usedPlotNamesList, plantIdList, island.islandID);
            data.boughtIslands.Add(island.islandID);
            data.islandDataMap.Add(islandData);
        }
    }*/
}
