using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Island : MonoBehaviour
{
    public string islandID;
    public Material islandMat;
    public IslandState currentState = IslandState.Sowed;
    public IslandState potentialState = IslandState.Sowed;
    public GameObject glow;

    public bool islandAvailable;
    public int islandBuildCost;
    public int islandTaxCost;
    public bool islandBought;

    public GameObject sign;
    public GameObject signWarningIcon;
    public List<GameObject> plotsSmallPlants;
    public List<GameObject> plotsMediumPlants;
    public List<GameObject> plotsBigPlants;
    public List<GameObject> usedPlots = new List<GameObject>();
    public List<Plant> itemsOnIsland = new List<Plant>();
    public GameObject islandTop;
    public Material topMat;
    public GameObject islandBottom;
    public Material bottomMat;

    public TMP_Text waterAvailableText;
    public TMP_Text nitrogenAvailableText;
    public TMP_Text phosphorusAvailableText;
    public TMP_Text potassiumAvailableText;
    public int water;
    public int _nitrogen;
    public int _phosphorus;
    public int _potassium;
    public int magnesium;
    public int sulfur;
    public int calcium;
    public int Nitrogen
    {
        get => _nitrogen;
        set { _nitrogen = value; OnNutrientChanged(); }
    }
    public int Phosphorus
    {
        get => _phosphorus;
        set { _phosphorus = value; OnNutrientChanged(); }
    }
    public int Potassium
    {
        get => _potassium;
        set { _potassium = value; OnNutrientChanged(); }
    }

    public TMP_Text waterUsageText;
    public TMP_Text nitrogenUsageText;
    public TMP_Text phosphorusUsageText;
    public TMP_Text potassiumUsageText;
    public int waterUsage;
    public int nitrogenUsage;
    public int phosphorusUsage;
    public int potassiumUsage;

    public bool needsNPK;
    public Material bottomDefaultMat;

    public enum IslandState
    {
        Highlighted,
        Default,
        Watered,
        Sowed,
        Cultivated,
    }

    public void OnNutrientChanged()
    {
        GameManager.ISM.UpdateIslandMaterial(this);
        UpdateIslandStats();
    }

    public void ToggleState(IslandState targetState, IslandState fallbackState)
    {
        SetState(currentState == targetState ? fallbackState : targetState);
    }

    public void SetState(IslandState newState)
    {
        currentState = newState;
        EnterState(currentState);
    }

    private void EnterState(IslandState state)
    {
        GetIslandComponents();
        switch (state)
        {
            case IslandState.Highlighted:
                glow.SetActive(true);
                break;
            case IslandState.Default:
                glow.SetActive(false);
                Color topColor = topMat.color;
                topColor.a = 0f;
                topMat.color = topColor;
                Color bottomColor = bottomMat.color;
                bottomColor.a = 0f;
                bottomMat.color = bottomColor;
                break;
            case IslandState.Watered:
                if (islandID == "Island(0,0)Ring1" && GameManager.TTM.tutorialCount == 4)
                {
                    GameManager.TTM.QuestCompleted = true;
                }
                if (needsNPK)
                {
                    islandMat = GameManager.ISM.wateredNeedsNPKMat;
                }
                else
                {
                    islandMat = GameManager.ISM.wateredMat;
                }
                break;
            case IslandState.Sowed:
                if (needsNPK)
                {
                    islandMat = GameManager.ISM.sowedNeedsNPKMat;
                }
                else
                {
                    islandMat = GameManager.ISM.sowedMat;
                }
                break;
            case IslandState.Cultivated:
                if(islandID == "Island(0,0)Ring1" && GameManager.TTM.tutorialCount == 3)
                {
                    GameManager.TTM.QuestCompleted = true;
                }
                if (needsNPK)
                {
                    islandMat = GameManager.ISM.cultivatedNeedsNPKMat;
                }
                else
                {
                    islandMat = GameManager.ISM.cultivatedMat;
                }
                break;
        }
        if (islandMat != null)
        {
            SetMaterial(islandMat);
        }
    }

    public void TogglePotentialState(IslandState targetState, IslandState fallbackState)
    {
        SetPotentialState(potentialState == targetState ? fallbackState : targetState);
    }

    public void SetPotentialState(IslandState newState)
    {
        potentialState = newState;
        EnterPotentialState(potentialState);
    }

    private void EnterPotentialState(IslandState state)
    {
        switch (state)
        {
            case IslandState.Watered:
                if (needsNPK)
                {
                    islandMat = GameManager.ISM.wateredNeedsNPKMat;
                }
                else
                {
                    islandMat = GameManager.ISM.wateredMat;
                }
                break;
            case IslandState.Sowed:
                if (needsNPK)
                {
                    islandMat = GameManager.ISM.sowedMat;
                }
                else
                {
                    islandMat = GameManager.ISM.sowedNeedsNPKMat;
                }
                break;
            case IslandState.Cultivated:
                if(needsNPK)
                {
                    islandMat = GameManager.ISM.cultivatedNeedsNPKMat;
                }
                else
                {
                    islandMat = GameManager.ISM.cultivatedMat;
                }
                break;
        }
        SetMaterial(islandMat);
    }

    public void MakeUsedPlot(GameObject usedPlot, Card usedCard, GameObject usedPlant)
    {
        if (usedPlot == null || usedCard == null || usedPlant == null)
        {
            Debug.LogWarning("Invalid plot, card or plant provided.");
            return;
        }
        itemsOnIsland.Add(usedPlant.GetComponent<Plant>());
        usedPlots.Add(usedPlot);
        switch (usedCard.cardType)
        {
            case "PlantSmall":
                GameManager.ISM.SetCollisions("PlantMedium");
                GameManager.ISM.SetCollisions("PlantBig");
                break;
            case "PlantMedium":
                GameManager.ISM.SetCollisions("PlantSmall");
                GameManager.ISM.SetCollisions("PlantBig");
                break;
            case "PlantBig":
                GameManager.ISM.SetCollisions("PlantSmall");
                GameManager.ISM.SetCollisions("PlantMedium");
                break;
            case "Machine":
                GameManager.ISM.SetCollisions("PlantSmall");
                GameManager.ISM.SetCollisions("PlantBig");
                break;
        }
        CheckOverlappingPlots(usedPlot.GetComponent<BoxCollider>());
    }

    public void CheckOverlappingPlots(BoxCollider boxCollider)
    {
        if (boxCollider == null)
        {
            return;
        }
        Vector3 center = boxCollider.transform.TransformPoint(boxCollider.center);
        Vector3 halfExtents = boxCollider.size / 2;
        Collider[] overlappingColliders = Physics.OverlapBox(center, halfExtents, boxCollider.transform.rotation);
        foreach (Collider collider in overlappingColliders)
        {
            if (collider != boxCollider && collider.GetComponent<Plant>() == null)
            {
                usedPlots.Add(collider.gameObject);
            }
        }
        GameManager.ISM.SetCollisions("Reset");
    }

    public void GetIslandComponents()
    {
        if (islandTop != null && islandBottom != null)
        {
            topMat = islandTop.GetComponent<Renderer>()?.material;
            bottomMat = islandBottom.GetComponent<Renderer>()?.material;
        }
    }

    public void ChangeMaterial(float alphaChangeSpeed)
    {
        GetIslandComponents();
        UpdateMaterialAlpha(topMat, alphaChangeSpeed);
        UpdateMaterialAlpha(bottomMat, alphaChangeSpeed);
    }

    private void UpdateMaterialAlpha(Material material, float alphaChangeSpeed)
    {
        if (material == null) return;
        Color color = material.color;
        color.a += alphaChangeSpeed * Time.deltaTime;
        color.a = Mathf.Clamp01(color.a);
        material.color = color;
        if (Mathf.Approximately(color.a, 1.0f))
        {
            ToOpaqueMode(material, false);
        }
    }

    public void ToOpaqueMode(Material material, bool transparent)
    {
        if(transparent)
        {
            Color color = material.color;
            color.a = 0f;
            material.color = color;
        }
        else
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
    }

    public void ResetMaterials()
    {
        GetIslandComponents();
        ToOpaqueMode(topMat, true);
        ToOpaqueMode(bottomMat, true);
    }

    public void SetMaterial(Material changeMaterial)
    {
        if (islandTop != null)
        {
            var renderer = islandTop.GetComponent<Renderer>();
            if (renderer != null)
            {
                var materials = renderer.materials;
                materials[0] = changeMaterial;
                renderer.materials = materials;
            }
        }
        topMat = changeMaterial;
    }

    public GameObject FindPlotByName(string name)
    {
        return usedPlots.Find(plot => plot.name == name);
    }

    public Card FindItemOnIslandByCardId(string plantCardId)
    {
        return GameManager.CM.FindCardById("Card" + plantCardId);
    }

    public void UpdateIslandStats()
    {
        waterUsage = 0;
        nitrogenUsage = 0;
        phosphorusUsage = 0;
        potassiumUsage = 0;

        foreach (Plant plant in itemsOnIsland)
        {
            waterUsage += plant.water;
            nitrogenUsage += plant.nitrogen;
            phosphorusUsage += plant.phosphorus;
            potassiumUsage += plant.potassium;
        }
        CheckWarningIcon();
        waterUsageText.SetText(waterUsage.ToString() + " L");
        nitrogenUsageText.SetText(nitrogenUsage.ToString() + " L");
        phosphorusUsageText.SetText(phosphorusUsage.ToString() + " L");
        potassiumUsageText.SetText(potassiumUsage.ToString() + " L");
        waterAvailableText.SetText(water.ToString() + " L");
        nitrogenAvailableText.SetText(_nitrogen.ToString() + " L");
        phosphorusAvailableText.SetText(_phosphorus.ToString() + " L");
        potassiumAvailableText.SetText(_potassium.ToString() + " L");
    }

    public void CheckWarningIcon()
    {
        if (waterUsage > water || nitrogenUsage > _nitrogen || phosphorusUsage > _phosphorus || potassiumUsage > _potassium)
        {
            signWarningIcon.SetActive(true);
        }
        else
        {
            signWarningIcon.SetActive(false);
        }
    }
}
