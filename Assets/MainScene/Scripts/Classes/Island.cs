using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Island : MonoBehaviour
{
    public string islandID;
    public Material islandMat;
    public IslandState currentState;
    public IslandState previousState;
    public GameObject glow;

    public bool islandAvailable;
    public int islandBuildCost;
    public int islandTaxCost;
    public bool islandBought;

    public List<GameObject> plotsSmallPlants;
    public List<GameObject> plotsMediumPlants;
    public List<GameObject> plotsBigPlants;
    public List<GameObject> usedPlots = new List<GameObject>();
    public List<Plant> itemsOnIsland = new List<Plant>();
    public GameObject islandTop;
    public Material topMat;
    public GameObject islandBottom;
    public Material bottomMat;
    public Material bottomDefaultMat;

    public List<int> nutrientsAvailable;
    public List<int> nutrientsRequired;

    public GameObject warningIcon;
    public bool needsNPK;

    public enum IslandState
    {
        Highlighted,
        Default,
        Watered,
        Sowed,
        Cultivated,
    }

    public void ToggleState(IslandState targetState, IslandState fallbackState)
    {
        SetState(currentState == targetState ? fallbackState : targetState);
    }

    private void SetState(IslandState state)
    {
        currentState = state;
        GetIslandComponents();
        switch (state)
        {
            case IslandState.Highlighted:
                glow.SetActive(true);
                break;
            case IslandState.Default:
                Color topColor = topMat.color;
                topColor.a = 0f;
                topMat.color = topColor;
                Color bottomColor = bottomMat.color;
                bottomColor.a = 0f;
                bottomMat.color = bottomColor;
                break;
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
                glow.SetActive(false);
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
            case "Buildable":
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

    public void UpdateNutrientsRequired()
    {
        for(int i = 0; i < nutrientsRequired.Count; i++)
        {
            nutrientsRequired[i] = 0;
        }
        foreach (Plant plant in itemsOnIsland)
        {
            for (int i = 0; i < nutrientsRequired.Count; i++)
            {
                nutrientsRequired[i] += plant.nutrientsUsages[i];
            }
        }
        CheckWarningIcon();
    }

    public void CheckWarningIcon()
    {
        if (nutrientsRequired[1] > nutrientsAvailable[1] || nutrientsRequired[2] > nutrientsAvailable[2] || nutrientsRequired[3] > nutrientsAvailable[3] && warningIcon.activeSelf != true)
        {
            warningIcon.SetActive(true);
        }
        else
        {
            warningIcon.SetActive(false);
        }
    }
}
