using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{
    public string islandId;
    public Material islandMat;
    public IslandState currentState = IslandState.Sowed;
    public IslandState potentialState = IslandState.Sowed;
    
    public bool islandBoughtStatus;
    public float islandBuildCost;
    public bool islandCanBought; 

    public List<GameObject> plotsSmallPlants;
    public List<GameObject> plotsMediumPlants;
    public List<GameObject> plotsBigPlants;
    public List<GameObject> usedPlots = new List<GameObject>();
    public List<Plant> itemsOnIsland = new List<Plant>();

    public GameObject islandTop;
    public Material topMat;
    public GameObject islandBottom;
    public Material bottomMat;

    private float nitrogen;
    private float phosphorus;
    private float potassium;

    public Material sowedMat;
    public Material cultivatedMat;
    public Material wateredMat;
    public Material bottomDefaultMat;

    public enum IslandState
    {
        Highlighted,
        Default,
        Watered,
        Sowed,
        Cultivated,
        NeedsNitrogen,
        NeedsPhosphor,
        NeedsPotassium,
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
                topMat.EnableKeyword("_EMISSION");
                topMat.SetColor("_EmissionColor", Color.blue * 2.0f);
                bottomMat.EnableKeyword("_EMISSION");
                bottomMat.SetColor("_EmissionColor", Color.blue * 2.0f);
                break;
            case IslandState.Default:
                topMat.DisableKeyword("_EMISSION");
                bottomMat.DisableKeyword("_EMISSION");
                Color topColor = topMat.color;
                topColor.a = 0f;
                topMat.color = topColor;
                Color bottomColor = bottomMat.color;
                bottomColor.a = 0f;
                bottomMat.color = bottomColor;
                break;
            case IslandState.Watered:
                islandMat = wateredMat;
                break;
            case IslandState.Sowed:
                islandMat = sowedMat;
                break;
            case IslandState.Cultivated:
                islandMat = cultivatedMat;
                break;
            case IslandState.NeedsNitrogen:
                break;
            case IslandState.NeedsPhosphor:
                break;
            case IslandState.NeedsPotassium:
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
                islandMat = wateredMat;
                break;
            case IslandState.Sowed:
                islandMat = sowedMat;
                break;
            case IslandState.Cultivated:
                islandMat = cultivatedMat;
                break;
            case IslandState.NeedsNitrogen:
                break;
            case IslandState.NeedsPhosphor:
                break;
            case IslandState.NeedsPotassium:
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
        if (topMat != null)
        {
            UpdateMaterialAlpha(topMat, alphaChangeSpeed);
        }
        if (bottomMat != null)
        {
            UpdateMaterialAlpha(bottomMat, alphaChangeSpeed);
        }
    }

    private void UpdateMaterialAlpha(Material material, float alphaChangeSpeed)
    {
        if (material == null)
        {
            return;
        }
        Color color = material.color;
        color.a += alphaChangeSpeed * Time.deltaTime;
        color.a = Mathf.Clamp01(color.a);
        material.color = color;

        if (Mathf.Approximately(color.a, 1.0f))
        {
            ToOpaqueMode(material);
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
}
