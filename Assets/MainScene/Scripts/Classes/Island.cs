using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Island : MonoBehaviour
{
    [Header("Island stat variables")]
    public bool islandAvailable;
    public bool islandBought;
    public bool needsNPK;
    public string islandID;

    [Header("Island variables")]
    public GameObject islandTop;
    public GameObject islandBottom;
    public Material topMat;
    public Material bottomMat;

    [Header("Island build variables")]
    public int islandBuildCost;
    public int islandExpenseCost;

    [Header("Island states")]
    public IslandState currentState;
    public IslandState previousState;
    public GameObject glow;

    [Header("Island top materials")]
    public Material transparentMatTop;
    public Material sowedMatTop;
    public Material sowedNeedsNPKMatTop;
    public Material cultivatedMatTop;
    public Material cultivatedNeedsNPKMatTop;
    public Material wateredMatTop;
    public Material wateredNeedsNPKMatTop;

    [Header("Island bottom materials")]
    public Material transparentMatBot;
    public Material sowedMatBot;
    public Material sowedNeedsNPKMatBot;
    public Material wateredMatBot;
    public Material wateredNeedsNPKMatBot;

    [Header("Plots lists")]
    public List<GameObject> availableSmallPlots;
    public List<GameObject> availableMediumPlots;
    public List<GameObject> availableLargePlots;
    public List<GameObject> usedSmallPlots;
    public List<GameObject> usedMediumPlots;
    public List<GameObject> usedLargePlots;

    [Header("Objects on island lists")]
    public List<Plant> itemsOnIsland = new List<Plant>();
    public List<Plant> smallPlantsOnIsland = new List<Plant>();
    public List<Plant> mediumPlantsOnIsland = new List<Plant>();
    public List<Plant> largePlantsOnIsland = new List<Plant>();
    public List<Plant> buildablesOnIsland = new List<Plant>();

    [Header("Nutrient variables")]
    public GameObject warningIcon;
    public List<int> nutrientsAvailable = new List<int>();
    public List<int> nutrientsRequired = new List<int>();

    public enum IslandState
    {
        Transparent,
        Highlighted,
        Sowed,
        Cultivated,
        Watered
    }

    public void SetIslandState(IslandState state)
    {
        currentState = state;
        switch (state)
        {
            case IslandState.Transparent:
                islandTop.GetComponent<MeshRenderer>().material = transparentMatTop;
                islandBottom.GetComponent<MeshRenderer>().material = transparentMatBot;
                break;
            case IslandState.Highlighted:
                glow.SetActive(true);
                break;
            case IslandState.Watered:
                islandTop.GetComponent<MeshRenderer>().material = wateredMatTop;
                islandBottom.GetComponent<MeshRenderer>().material = wateredMatBot;
                break;
            case IslandState.Sowed:
                glow.SetActive(false);
                islandTop.GetComponent<MeshRenderer>().material = sowedMatTop;
                islandBottom.GetComponent<MeshRenderer>().material = sowedMatBot;
                break;
            case IslandState.Cultivated:
                islandTop.GetComponent<MeshRenderer>().material = cultivatedMatTop;
                islandBottom.GetComponent<MeshRenderer>().material = sowedMatBot;
                break;
        }
    }

    public void SetIslandMaterial(Material islandMat, Material islandMatNeedsNPK, GameObject islandPart)
    {
        MeshRenderer meshRenderer = islandPart.GetComponent<MeshRenderer>();
        if (islandMat == transparentMatTop || islandMat == transparentMatBot)
        {
            Color color = meshRenderer.materials[0].color;
            color.a = 0f;
            meshRenderer.materials[0].color = color;
        }
        else
        {
            float totalNPK = nutrientsAvailable.Sum() - nutrientsAvailable[0];
            float blendFactor = 1f - Mathf.Clamp01(totalNPK / 900);
            Material blendedMaterial = new Material(islandMat);
            Color blendedColor = Color.Lerp(islandMat.color, islandMatNeedsNPK.color, blendFactor);
            blendedMaterial.color = blendedColor;

            if (islandMat.mainTexture is Texture2D tex1 && islandMatNeedsNPK.mainTexture is Texture2D tex2)
            {
                Texture2D blendedTexture = BlendTextures(tex1, tex2, blendFactor);
                blendedMaterial.SetTexture("_MainTex", blendedTexture);
            }
            
            meshRenderer.material = blendedMaterial;
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

    public void SetCollisions(string cardType)
    {
        switch (cardType)
        {
            case "Small crops":
                foreach (Island island in GameManager.ISM.boughtIslands)
                {
                    if (island.currentState == Island.IslandState.Watered)
                    {
                        foreach (GameObject plot in island.availableSmallPlots)
                        {
                            plot.GetComponent<BoxCollider>().enabled = true;
                        }
                    }
                }
                break;
            case "Medium crops":
                foreach (Island island in GameManager.ISM.boughtIslands)
                {
                    if (island.currentState == Island.IslandState.Watered)
                    {
                        foreach (GameObject plot in island.availableMediumPlots)
                        {
                            plot.GetComponent<BoxCollider>().enabled = true;
                        }
                    }
                }
                break;
            case "Large crops":
                foreach (Island island in GameManager.ISM.boughtIslands)
                {
                    if (island.currentState == Island.IslandState.Watered)
                    {
                        foreach (GameObject plot in island.availableLargePlots)
                        {
                            plot.GetComponent<BoxCollider>().enabled = true;
                        }
                    }
                }
                break;
            case "Buildable":
                foreach (Island island in GameManager.ISM.boughtIslands)
                {
                    island.GetComponent<BoxCollider>().enabled = false;
                    foreach (GameObject plot in island.availableMediumPlots)
                    {
                        plot.GetComponent<BoxCollider>().enabled = true;
                    }
                }
                break;
            default:
                foreach (Island island in GameManager.ISM.boughtIslands)
                {
                    island.GetComponent<BoxCollider>().enabled = true;
                    foreach (GameObject plot in island.availableSmallPlots)
                    {
                        plot.GetComponent<BoxCollider>().enabled = false;
                    }
                    foreach (GameObject plot in island.availableMediumPlots)
                    {
                        plot.GetComponent<BoxCollider>().enabled = false;
                    }
                    foreach (GameObject plot in island.availableLargePlots)
                    {
                        plot.GetComponent<BoxCollider>().enabled = false;
                    }
                }
                break;
        }
    }

    public void UpdateMaterialAlpha(float alphaChangeSpeed)
    {
        Color colorTop = topMat.color;
        Color colorBot = bottomMat.color;
        colorTop.a += alphaChangeSpeed * Time.deltaTime;
        colorBot.a += alphaChangeSpeed * Time.deltaTime;
        colorTop.a = Mathf.Clamp01(colorTop.a);
        colorBot.a = Mathf.Clamp01(colorBot.a);
        topMat.color = colorTop;
        bottomMat.color = colorBot;
        if (Mathf.Approximately(colorTop.a, 1.0f) || Mathf.Approximately(colorBot.a, 1.0f))
        {
            ToOpaqueMode(topMat);
            ToOpaqueMode(bottomMat);
        }
    }

    public void ToOpaqueMode(Material material)
    {
        material.SetOverrideTag("RenderType", "");
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        material.SetInt("_ZWrite", 1);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = -1;
    }

    public void MakeUsedPlot(GameObject usedPlot, Card usedCard, Plant usedPlant)
    {
        itemsOnIsland.Add(usedPlant);
        switch (usedCard.cardType)
        {
            case "Small crops":
                smallPlantsOnIsland.Add(usedPlant);
                usedSmallPlots.Add(usedPlot);
                SetCollisions("Medium crops");
                SetCollisions("Large crops");
                GameManager.INM.UnlockInventoryItem(usedCard, usedPlant);
                UpdateNutrientsRequired(usedPlant);
                usedPlant.attachedIsland = this;
                usedPlant.UpdatePredictedYield();
                break;
            case "Medium crops":
                usedMediumPlots.Add(usedPlot);
                mediumPlantsOnIsland.Add(usedPlant);
                SetCollisions("Small crops");
                SetCollisions("Large crops");
                GameManager.INM.UnlockInventoryItem(usedCard, usedPlant);
                UpdateNutrientsRequired(usedPlant);
                usedPlant.attachedIsland = this;
                usedPlant.UpdatePredictedYield();
                break;
            case "Large crops":
                usedLargePlots.Add(usedPlot);
                largePlantsOnIsland.Add(usedPlant);
                SetCollisions("Small crops");
                SetCollisions("Medium crops");
                GameManager.INM.UnlockInventoryItem(usedCard, usedPlant);
                UpdateNutrientsRequired(usedPlant);
                usedPlant.attachedIsland = this;
                usedPlant.UpdatePredictedYield();
                break;
            case "Buildable":
                buildablesOnIsland.Add(usedPlant);
                SetCollisions("Small crops");
                SetCollisions("Large crops");
                break;
        }
        CheckOverlappingPlots(usedPlot.GetComponent<BoxCollider>());
        usedPlot.GetComponent<BoxCollider>().enabled = false;
    }

    public void CheckOverlappingPlots(BoxCollider boxCollider)
    {
        Vector3 center = boxCollider.transform.TransformPoint(boxCollider.center);
        Vector3 halfExtents = boxCollider.size / 2;
        Collider[] overlappingColliders = Physics.OverlapBox(center, halfExtents, boxCollider.transform.rotation);
        foreach (Collider collider in overlappingColliders)
        {
            if (collider != boxCollider && collider.GetComponent<Plant>() == null)
            {
                if(collider.gameObject.name.Contains("PlotSmall"))
                {
                    usedSmallPlots.Add(collider.gameObject);
                    availableSmallPlots.Remove(collider.gameObject);
                }
                if (collider.gameObject.name.Contains("PlotMedium"))
                {
                    usedMediumPlots.Add(collider.gameObject);
                    availableMediumPlots.Remove(collider.gameObject);
                }
                if (collider.gameObject.name.Contains("PlotLarge"))
                {
                    usedLargePlots.Add(collider.gameObject);
                    availableLargePlots.Remove(collider.gameObject);
                }
                collider.enabled = false;
            }
        }
        SetCollisions("Reset");
    }

    public void UpdateNutrientsRequired(Plant plant)
    {
        for (int i = 0; i < nutrientsRequired.Count; i++)
        {
            nutrientsRequired[i] += plant.nutrientsUsages[i];
        }
    }

    public void UpdateNutrients()
    {
        foreach (Plant plant in smallPlantsOnIsland.Concat(mediumPlantsOnIsland).Concat(largePlantsOnIsland))
        {
            plant.attachedInventoryItem.totalBaseYield = 0;
            plant.attachedInventoryItem.totalPredictedYield = 0;
            plant.UpdatePredictedYield();
        }
        CheckIslandMaterial();
    }

    public Card FindItemOnIslandByCardId(string plantCardId)
    {
        return GameManager.CM.FindCardByID("Card" + plantCardId);
    }

    public void CheckIslandMaterial()
    {
        switch (currentState)
        {
            case IslandState.Sowed:
                {
                    SetIslandMaterial(sowedMatTop, sowedNeedsNPKMatTop, islandTop);
                    SetIslandMaterial(sowedMatBot, sowedNeedsNPKMatBot, islandBottom);
                    break;
                }
            case IslandState.Cultivated:
                {
                    SetIslandMaterial(cultivatedMatTop, cultivatedNeedsNPKMatTop, islandTop);
                    SetIslandMaterial(sowedMatBot, sowedNeedsNPKMatBot, islandBottom);
                    break;
                }
            case IslandState.Watered:
                {
                    SetIslandMaterial(wateredMatTop, wateredNeedsNPKMatTop, islandTop);
                    SetIslandMaterial(wateredMatBot, wateredNeedsNPKMatBot, islandBottom);
                    break;
                }
        }
        CheckWarningIcon();
    }

    public void CheckWarningIcon()
    {
        if (nutrientsRequired[1] > nutrientsAvailable[1] || nutrientsRequired[2] > nutrientsAvailable[2] || nutrientsRequired[3] > nutrientsAvailable[3])
        {
            warningIcon.SetActive(true);
        }
        else
        {
            warningIcon.SetActive(false);
        }
    }
}
