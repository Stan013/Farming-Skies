using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Island : MonoBehaviour
{
    [Header("Island stat variables")]
    public bool islandAvailable;
    public bool islandBought;
    public string islandID;

    [Header("Island variables")]
    public MeshRenderer islandTop;
    public MeshRenderer islandBottom;
    public Material topMat;
    public Material bottomMat;
    public IslandData islandData;

    [Header("Island build variables")]
    public int islandBuildCost;
    public int islandExpenseCost;

    [Header("Island states")]
    public int islandState;
    public IslandState previousState;
    public IslandState potentialState;
    public GameObject glow;
    private IslandState _currentState;
    public IslandState currentState
    {
        get => _currentState;
        set
        {
            if (_currentState != value)
            {
                _currentState = value;
            }
        }
    }

    [Header("Island top materials")]
    public Material transparentMatTop;
    public Material pavedMatTop;
    public Material sowedMatTop;
    public Material cultivatedMatTop;
    public Material wateredMatTop;

    [Header("Island bottom materials")]
    public Material transparentMatBot;
    public Material sowedMatBot;
    public Material wateredMatBot;

    [Header("Island hover materials")]
    public bool hoverMatSetup = false;
    public bool fertiliserHoverSetup = false;
    public bool validPotentialMat = false;
    public Material previousMatTop;
    public Material previousMatBot;
    public Material potentialMatTop;
    public Material potentialMatBot;
    public Color previousTopColor;
    public Color previousBotColor;
    public Color potentialTopColor;
    public Color potentialBotColor;

    [Header("Plots lists")]
    public List<GameObject> availablePlots;
    public List<GameObject> usedPlots;

    [Header("Objects on island lists")]
    public List<MonoBehaviour> itemsOnIsland = new List<MonoBehaviour>();
    public List<Plant> plantsOnIsland = new List<Plant>();
    public List<Structure> structuresOnIsland = new List<Structure>();

    [Header("Nutrient variables")]
    public GameObject warningIcon;
    public List<int> nutrientsAvailable = new List<int>();
    public List<int> nutrientsRequired = new List<int>();

    public enum IslandState
    {
        Transparent,
        Highlighted,
        Denied,
        Sowed,
        Cultivated,
        Watered,
        Paved
    }

    private void Awake()
    {
        pavedMatTop.color = Color.white;
        sowedMatTop.color = Color.white;
        cultivatedMatTop.color = Color.white;
        wateredMatTop.color = Color.white;
    }

    public void SetCollisions(string cardType, string placeableSize)
    {
        switch(cardType)
        {
            case "Utilities":
                foreach (GameObject plot in availablePlots)
                {
                    plot.GetComponent<BoxCollider>().enabled = false;
                }
                GetComponent<BoxCollider>().enabled = true;
            break;
            default:
                if (cardType == "Structure" && currentState == IslandState.Paved || cardType == "Crop" && currentState == IslandState.Watered)
                {
                    foreach (GameObject plot in availablePlots)
                    {
                        string name = plot.name;
                        bool match = name.Contains(placeableSize) && 
                                    (placeableSize != "S" || !name.Contains("XS")) && 
                                    (placeableSize != "L" || !name.Contains("XL"));
                        plot.GetComponent<BoxCollider>().enabled = match;
                    }
                }
            break;
        }
    }

    public void UpdateMaterialAlpha(float alphaChangeSpeed)
    {
        topMat = islandTop.materials[0];
        bottomMat = islandBottom.materials[0];
        Color colorTop = topMat.color;
        Color colorBot = bottomMat.color;
        colorTop.a += alphaChangeSpeed * Time.deltaTime;
        colorBot.a += alphaChangeSpeed * Time.deltaTime;
        colorTop.a = Mathf.Clamp01(colorTop.a);
        colorBot.a = Mathf.Clamp01(colorBot.a);
        topMat.color = colorTop;
        bottomMat.color = colorBot;

        if (colorTop.a >= 0.999f && colorBot.a >= 0.999f)
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

    public void MakeUsedPlot(GameObject usedPlot, Card usedCard, Plant usedPlant, Structure usedStructure)
    {
        switch (usedCard.cardType)
        {
            case "Structure":
                itemsOnIsland.Add(usedStructure);
                structuresOnIsland.Add(usedStructure);
                GameManager.EM.AddExpenseStructure(usedStructure);
                break;
            case "Crops":
                usedPlant.attachedCard = usedCard;
                usedPlant.attachedIsland = this;
                itemsOnIsland.Add(usedPlant);
                plantsOnIsland.Add(usedPlant);
                GameManager.INM.UnlockInventoryItem(usedCard, usedPlant);
                UpdateNutrientsRequired(usedPlant);
                usedPlant.UpdatePredictedYield();
                GameManager.PM.plantValueChange += usedPlant.predictedYield * usedPlant.attachedInventoryItem.attachedItemCard.itemPrice;
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
                usedPlots.Add(collider.gameObject);
                availablePlots.Remove(collider.gameObject);
                collider.enabled = false;
            }
        }
        SetCollisions("Utilities", "Reset");
    }
    
    public void UpdateNutrientsRequired(Plant plant)
    {
        for (int i = 0; i < nutrientsRequired.Count; i++)
        {
            nutrientsRequired[i] += plant.nutrientsUsages[i];
        }
        CheckWarningIcon();
    }

    public void UpdateNutrients()
    {
        foreach (Plant plant in plantsOnIsland)
        {
            plant.attachedInventoryItem.totalBaseYield = 0;
            plant.attachedInventoryItem.totalPredictedYield = 0;
            plant.UpdatePredictedYield();
        }
    }

    public Card FindItemOnIslandByCardId(string plantCardId)
    {
        return GameManager.CM.FindCardByID("Card" + plantCardId);
    }

    public void MaterialDragValidation(string cardName)
    {
        if (!hoverMatSetup)
        {
            previousMatTop = islandTop.material;
            previousMatBot = islandBottom.material;
            potentialMatTop = new Material(previousMatTop);
            potentialMatBot = new Material(previousMatBot);
            hoverMatSetup = true;
        }

        validPotentialMat = false;

        switch (cardName)
        {
            case "Cultivator":
                if (currentState == IslandState.Sowed)
                {
                    potentialMatTop = cultivatedMatTop;
                    potentialMatBot = sowedMatBot;
                    validPotentialMat = true;
                    potentialState = IslandState.Cultivated;
                }
                break;

            case "Watering Can":
                if (currentState == IslandState.Cultivated)
                {
                    potentialMatTop = wateredMatTop;
                    potentialMatBot = wateredMatBot;
                    validPotentialMat = true;
                    potentialState = IslandState.Watered;
                }
                break;

            case "Grass Seeds":
                if (currentState != IslandState.Sowed)
                {
                    potentialMatTop = sowedMatTop;
                    potentialMatBot = sowedMatBot;
                    validPotentialMat = true;
                    potentialState = IslandState.Sowed;
                }
                break;

            case "Concrete Bag":
                if (currentState != IslandState.Paved)
                {
                    potentialMatTop = pavedMatTop;
                    potentialMatBot = sowedMatBot;
                    validPotentialMat = true;
                    potentialState = IslandState.Paved;
                }
                break;
        }

        potentialMatTop.color = islandTop.material.color;
        potentialMatBot.color = islandBottom.material.color;
    }

    public void HoverFertiliser(int nutrientIndex, int nutrientAddition)
    {
        int addedNutrient = Mathf.Clamp(nutrientsAvailable[nutrientIndex - 1] + nutrientAddition, 0, 100);
        int totalNutrients = 0;

        for (int i = 1; i < nutrientsAvailable.Count; i++)
        {
            if (i == nutrientIndex - 1)
                totalNutrients += addedNutrient;
            else
                totalNutrients += nutrientsAvailable[i];
        }

        float factor = totalNutrients / (100f * (nutrientsAvailable.Count - 1));

        Color targetTop = new Color(165f / 255f, 1f, 165f / 255f);
        Color targetBot = new Color(1f, 1f, 0f);

        potentialMatTop.color = Color.Lerp(previousMatTop.color, targetTop, factor);
        potentialMatBot.color = Color.Lerp(previousMatBot.color, targetBot, factor);
    }

    public void SetIslandColor(int nutrientIndex, int nutrientAddition, bool validDrag)
    {
        if (!fertiliserHoverSetup)
        {
            previousTopColor = islandTop.material.color;
            previousBotColor = islandBottom.material.color;
            potentialTopColor = previousTopColor;
            potentialBotColor = previousBotColor;
            fertiliserHoverSetup = true;
        }

        if (validDrag)
        {
            int addedNutrient = Mathf.Clamp(nutrientsAvailable[nutrientIndex] + nutrientAddition, 0, 100);
            int totalNutrients = 0;
            for (int i = 1; i < nutrientsAvailable.Count; i++)
            {
                totalNutrients += (i == nutrientIndex ? addedNutrient : nutrientsAvailable[i]);
            }

            Color topMin = Color.white;
            Color topMax = new Color(175f/255f, 1f, 175f/255f);

            Color botMin = Color.white;
            Color botMax = new Color(175f/255f, 175f/255f, 1f);

            float factor = totalNutrients / (100f * (nutrientsAvailable.Count - 1));

            potentialTopColor = Color.Lerp(topMin, topMax, factor);
            potentialBotColor = Color.Lerp(botMin, botMax, factor);

            islandTop.material.color = potentialTopColor;
            islandBottom.material.color = potentialBotColor;
        }
        else
        {
            islandTop.material.color = previousTopColor;
            islandBottom.material.color = previousBotColor;
        }
    }

    public void UpdateIslandMaterial(bool validDrag)
    {
        if(currentState == IslandState.Highlighted || currentState == IslandState.Transparent || currentState == IslandState.Denied)
        {
            Material newTop = new Material(topMat);
            Material newBottom = new Material(bottomMat);

            Color topColor = newTop.color;
            topColor.a = 0f;
            newTop.color = topColor;

            Color bottomColor = newBottom.color;
            bottomColor.a = 0f;
            newBottom.color = bottomColor;

            topMat = newTop;
            bottomMat = newBottom;
            islandTop.material = topMat;
            islandBottom.material = bottomMat;

            glow.SetActive(false);
            
            if(currentState == IslandState.Highlighted)
            {
                glow.SetActive(true);
            }

        }
        else
        {
            if (validDrag)
            {
                islandTop.material = potentialMatTop;
                islandBottom.material = potentialMatBot;
            }
            else
            {
                islandTop.material = previousMatTop;
                islandBottom.material = previousMatBot;
            }
            glow.SetActive(false);
        }
    }

    public void SetIslandMaterial(IslandState islandState, Material matTop, Material matBottom)
    {
        currentState = islandState;
        SetIslandColor(1, 0, true);
        potentialMatTop = matTop;
        potentialMatBot = matBottom;
        UpdateIslandMaterial(true);
        CheckWarningIcon();
    }

    public IEnumerator RefillNutrients(int amount, float duration = 1.5f)
    {
        int startNitrogen = nutrientsAvailable[1];
        int startPhosphorus = nutrientsAvailable[2];
        int startPotassium = nutrientsAvailable[3];

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            nutrientsAvailable[1] = Mathf.RoundToInt(Mathf.Lerp(startNitrogen, startNitrogen + amount, t));
            nutrientsAvailable[2] = Mathf.RoundToInt(Mathf.Lerp(startPhosphorus, startPhosphorus + amount, t));
            nutrientsAvailable[3] = Mathf.RoundToInt(Mathf.Lerp(startPotassium, startPotassium + amount, t));
            
            SetIslandColor(1, 0, true);
            GameManager.ISM.OpenIslandManagement("Available");
            
            yield return null;
        }

        nutrientsAvailable[1] = startNitrogen + amount;
        nutrientsAvailable[2] = startPhosphorus + amount;
        nutrientsAvailable[3] = startPotassium + amount;
        SetIslandColor(1, 0, true);
        GameManager.ISM.OpenIslandManagement("Available");
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

    public IEnumerator ShowNotAllowedRoutine(GameObject notAllowedIndicator)
    {
        notAllowedIndicator.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        notAllowedIndicator.SetActive(false);
    }

    public void LoadIslandData(IslandData data)
    {
        islandBought = data.islandBought;
        islandAvailable = data.islandAvailable;
        islandID = data.islandID;
        islandState = data.islandState;
        nutrientsAvailable.Clear();
        nutrientsAvailable = data.nutrientsAvailable;

        switch (data.islandState)
        {
            case 0:
                currentState = IslandState.Transparent;
                break;
            case 1:
                currentState = IslandState.Highlighted;
                break;
            case 2:
                currentState = IslandState.Sowed;
                previousState = IslandState.Watered;
                topMat = potentialMatTop;
                bottomMat = potentialMatBot;
                break;
            case 3:
                currentState = IslandState.Cultivated;
                previousState = IslandState.Sowed;
                topMat = potentialMatTop;
                bottomMat = potentialMatBot;
                break;
            case 4:
                currentState = IslandState.Watered;
                previousState = IslandState.Cultivated;
                topMat = potentialMatTop;
                bottomMat = potentialMatBot;
                break;
            case 5:
                currentState = IslandState.Paved;
                previousState = IslandState.Sowed;
                topMat = pavedMatTop;
                bottomMat = sowedMatBot;
                break;
        }
        GameManager.PM.SetPlantData(this, data.plantsMap);
    }

    public IslandData SaveIslandData()
    {
        switch (currentState)
        {
            case IslandState.Transparent:
                islandState = 0;
                break;
            case IslandState.Highlighted:
                islandState = 1;
                break;
            case IslandState.Sowed:
                islandState = 2;
                break;
            case IslandState.Cultivated:
                islandState = 3;
                break;
            case IslandState.Watered:
                islandState = 4;
                break;
            case IslandState.Paved:
                islandState = 5;
                break;
        }

        islandData = new IslandData(islandAvailable, islandBought, islandID, islandState, nutrientsAvailable, GameManager.PM.GetPlantData(this));
        return islandData;
    }
}
