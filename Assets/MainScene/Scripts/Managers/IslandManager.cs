using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandManager : MonoBehaviour, IDataPersistence
{
    //Manager
    public Transform availableIslandsParent;
    public List<Island> availableIsland;
    public List<Island> boughtIslands;
    public List<Island> unboughtIslands;
    public Island starterIsland;

    public Material sowedMat;
    public Material sowedNeedsNPKMat;
    public Material cultivatedMat;
    public Material cultivatedNeedsNPKMat;
    public Material wateredMat;
    public Material wateredNeedsNPKMat;

    public void SetIslands()
    {
        foreach(Transform islandRing in availableIslandsParent)
        {
            foreach (Island childIsland in islandRing.GetComponentsInChildren<Island>())
            {
                if(childIsland.islandAvailable)
                {
                    availableIsland.Add(childIsland);
                    if (childIsland.islandBought)
                    {
                        boughtIslands.Add(childIsland);
                    }
                    else
                    {
                        unboughtIslands.Add(childIsland);
                    }
                }
            }
        }
        if (GameManager.TTM.tutorial)
        {
            starterIsland.ToggleState(Island.IslandState.Highlighted, Island.IslandState.Default);
        }
    }

    public Island GetClickedIsland()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out var hit) ? hit.collider.gameObject.GetComponent<Island>() : null;
    }

    public void AddIslandToBought(Island reconstructedIsland)
    {
        if(GameManager.TTM.tutorial)
        {
            if(reconstructedIsland.name == "0,0" || reconstructedIsland.name == "-1,0")
            {
                //GameManager.TTM.QuestCompleted = true;
            }
        }
        reconstructedIsland.ToggleState(Island.IslandState.Sowed, Island.IslandState.Default);
        boughtIslands.Add(reconstructedIsland);
        unboughtIslands.Remove(reconstructedIsland);
    }

    public void SetCollisions(string cardType)
    {
        switch (cardType)
        {
            case "Utility":
                foreach (Island island in boughtIslands)
                {
                    island.GetComponent<BoxCollider>().enabled = true;
                }
                break;
            case "PlantSmall":
                foreach (Island island in boughtIslands)
                {
                    if (island.currentState == Island.IslandState.Watered)
                    {
                        foreach (GameObject plot in island.plotsSmallPlants)
                        {
                            plot.GetComponent<BoxCollider>().enabled = true;
                        }
                    }
                }
                break;
            case "PlantMedium":
                foreach (Island island in boughtIslands)
                {
                    if (island.currentState == Island.IslandState.Watered)
                    {
                        foreach (GameObject plot in island.plotsMediumPlants)
                        {
                            plot.GetComponent<BoxCollider>().enabled = true;
                        }
                    }
                }
                break;
            case "PlantBig":
                foreach (Island island in boughtIslands)
                {
                    if (island.currentState == Island.IslandState.Watered)
                    {
                        foreach (GameObject plot in island.plotsBigPlants)
                        {
                            plot.GetComponent<BoxCollider>().enabled = true;
                        }
                    }
                }
                break;
            case "Machine":
                foreach (Island island in boughtIslands)
                {
                    island.GetComponent<BoxCollider>().enabled = false;
                    foreach (GameObject plot in island.plotsMediumPlants)
                    {
                        plot.GetComponent<BoxCollider>().enabled = true;
                    }
                }
                break;
            default:
                foreach (Island island in boughtIslands)
                {
                    island.GetComponent<BoxCollider>().enabled = true;
                    foreach (GameObject plot in island.plotsSmallPlants)
                    {
                        plot.GetComponent<BoxCollider>().enabled = false;
                    }
                    foreach (GameObject plot in island.plotsMediumPlants)
                    {
                        plot.GetComponent<BoxCollider>().enabled = false;
                    }
                    foreach (GameObject plot in island.plotsBigPlants)
                    {
                        plot.GetComponent<BoxCollider>().enabled = false;
                    }
                }
                break;
        }
    }

    public Island CheckPotentialIsland()

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

    public void UpdateIslandMaterial(Island island)
    {
        float totalNPK = island.Nitrogen + island.Phosphorus + island.Potassium;
        float blendFactor = Mathf.Clamp01(totalNPK / 50f);
        Material defaultMat = null;
        Material needsNPKMat = null;

        switch (island.currentState)
        {
            case Island.IslandState.Sowed:
                defaultMat = sowedMat;
                needsNPKMat = sowedNeedsNPKMat;
                break;
            case Island.IslandState.Watered:
                defaultMat = wateredMat;
                needsNPKMat = wateredNeedsNPKMat;
                break;
            case Island.IslandState.Cultivated:
                defaultMat = cultivatedMat;
                needsNPKMat = cultivatedNeedsNPKMat;
                break;
        }

        if (defaultMat != null && needsNPKMat != null)
        {
            Material blendedMaterial = new Material(defaultMat);
            Color blendedColor = Color.Lerp(needsNPKMat.color, defaultMat.color, blendFactor);
            blendedMaterial.color = blendedColor;
            if (defaultMat.mainTexture != null && needsNPKMat.mainTexture != null)
            {
                blendedMaterial.SetTexture("_MainTex", BlendTextures(
                    (Texture2D)needsNPKMat.mainTexture,
                    (Texture2D)defaultMat.mainTexture,
                    blendFactor
                ));
            }
            island.SetMaterial(blendedMaterial);
        }
    }

    private Texture2D BlendTextures(Texture2D tex1, Texture2D tex2, float blendFactor)
    {
        int width = tex1.width;
        int height = tex1.height;
        Texture2D blendedTex = new Texture2D(width, height);

        Color[] colors1 = tex1.GetPixels();
        Color[] colors2 = tex2.GetPixels();
        Color[] blendedColors = new Color[colors1.Length];

        for (int i = 0; i < blendedColors.Length; i++)
        {
            blendedColors[i] = Color.Lerp(colors1[i], colors2[i], blendFactor);
        }

        blendedTex.SetPixels(blendedColors);
        blendedTex.Apply();

        return blendedTex;
    }

    public bool CheckIslandWater(Plant plant)
    {
        if(GameManager.UM.water >= plant.water)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Island FindIslandByID(string islandID)
    {
        foreach(Island island in availableIsland)
        {
            if(island.islandID == islandID)
            {
                return island;
            }
        }
        return null;
    }

    public void LoadData(GameData data)
    {
        boughtIslands.Clear();
        var islandDataCount = 0;
        foreach (string islandID in data.boughtIslands)
        {
            var island = FindIslandByID(islandID);
            unboughtIslands.Remove(island);
            boughtIslands.Add(island);
            island.islandBought= true;
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
                    GameObject plant = Instantiate(plantCard.GetComponent<CardDrag>().dragModel, Vector3.zero, Quaternion.identity);
                    plant.transform.SetParent(usedPlot.transform);
                    plant.transform.localPosition = new Vector3(0, -0.25f, 0);
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
            foreach(GameObject plot in island.usedPlots)
            {
                usedPlotNamesList.Add(plot.name);
            }
            List<string> plantIdList = new List<string>();
            foreach(Plant plant in island.itemsOnIsland)
            {
                plantIdList.Add(plant.plantCardID);
            }
            IslandData islandData = new IslandData(usedPlotNamesList, plantIdList, island.islandID);
            data.boughtIslands.Add(island.islandID);
            data.islandDataMap.Add(islandData);
        }
    }
}
