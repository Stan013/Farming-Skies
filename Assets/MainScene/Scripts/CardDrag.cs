using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [Header("Drag variables")]
    public GameObject dragModel;
    private float adjustZ = 5f;
    private GameObject dragInstance;
    private Quaternion dragInstanceRotation;

    [Header("Hover variables")]
    public Island hoverIsland;
    private Island previousHoverIsland;
    public GameObject hoverPlot;
    public GameObject previousHoverPlot;
    public Sprite plotIndicatorGreen;
    public Sprite plotIndicatorOrange;

    [Header("Collision variables")]
    private bool collisionOn = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!GameManager.HM.dragging && GameManager.CM.inspectCard == null)
        {
            GameManager.HM.dragging = true;
            GameManager.HM.dragCard = GetComponent<Card>();
            GameManager.HM.dragCard.SetCardState(Card.CardState.InDrag);
            dragInstance = Instantiate(dragModel, SetMousePosition(), Quaternion.identity);
        }
        else
        {
            eventData.pointerDrag = null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragInstance == null) return;

        UpdateDragInstanceTransform();
        HandleIslandCollisions();

        switch (GameManager.HM.dragCard.cardType)
        {
            case "Utilities":
                HandleUtilityHover();
                break;

            default:
                HandleBuildableHover();
                break;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameManager.HM.dragging = false;
        collisionOn = false;

        switch (GameManager.HM.dragCard.cardType)
        {
            case "Utilities":
                HandleUtilityDrop();
                break;
            default:
                HandleBuildableDrop();
                break;
        }

        Destroy(dragInstance);
        hoverIsland?.SetCollisions("Reset");
    }

    private void UpdateDragInstanceTransform()
    {
        dragInstance.transform.position = SetMousePosition();
        dragInstance.transform.LookAt(GameManager.IPM.cam.transform);
        dragInstanceRotation = Quaternion.Euler(0, dragInstance.transform.rotation.eulerAngles.y - 90, dragInstance.transform.rotation.eulerAngles.z);
        dragInstance.transform.rotation = dragInstanceRotation;
    }

    private void HandleIslandCollisions()
    {
        if (!collisionOn && GameManager.ISM.GetPotentialBoughtIsland() != null)
        {
            GameManager.ISM.SetupIslandCollisions(true);
            GameManager.ISM.GetPotentialBoughtIsland().SetCollisions(GameManager.HM.dragCard.cardType);
            collisionOn = true;
        }
    }

    private void HandleUtilityHover()
    {
        var potentialIsland = GameManager.ISM.GetPotentialBoughtIsland();

        if (potentialIsland == null)
        {
            previousHoverIsland = hoverIsland;
            hoverIsland = null;

            if (previousHoverIsland != null && previousHoverIsland.previousState != Island.IslandState.Transparent && !GameManager.HM.dragCard.name.Contains("Fertiliser"))
            {
                previousHoverIsland.islandMatPotential = false;
                previousHoverIsland.currentState = previousHoverIsland.previousState;
                previousHoverIsland = null;
            }
            return;
        }
        else
        {
            hoverIsland = potentialIsland;
        }

        if (GameManager.HM.dragCard.cardName == "Cultivator" && hoverIsland.currentState == Island.IslandState.Sowed)
        {
            UpdateIslandState(Island.IslandState.Cultivated);
        }
        else if (GameManager.HM.dragCard.cardName == "Watering Can" && hoverIsland.currentState == Island.IslandState.Cultivated)
        {
            UpdateIslandState(Island.IslandState.Watered);
        }
        else if (GameManager.HM.dragCard.cardName == "Grass Seed" && hoverIsland.currentState != Island.IslandState.Sowed)
        {
            UpdateIslandState(Island.IslandState.Sowed);
        }
        else if (GameManager.HM.dragCard.cardName == "Concrete Bag" && hoverIsland.currentState == Island.IslandState.Sowed)
        {
            hoverIsland.potentialMatTop = hoverIsland.pavedMatTop;
            hoverIsland.islandMatPotential = true;
            UpdateIslandState(Island.IslandState.Paved);
        }
    }

    private void HandleUtilityDrop()
    {
        if (hoverIsland == null)
        {
            CancelDrag();
            return;
        }

        if (GameManager.HM.dragCard.nutrientIndex != 0)
        {
            if (GameManager.HM.dragCard.nutrientIndex == 1 && hoverIsland.previousState != Island.IslandState.Cultivated)
            {
                CancelDrag();
                return;
            }

            hoverIsland.nutrientsAvailable[GameManager.HM.dragCard.nutrientIndex - 1] += GameManager.HM.dragCard.nutrientAddition;
        }

        switch (GameManager.HM.dragCard.cardName)
        {
            case "Cultivator":
                hoverIsland.topMat = hoverIsland.potentialMatTop;
                hoverIsland.bottomMat = hoverIsland.potentialMatBottom;
                hoverIsland.CreateIslandMaterial(Island.IslandState.Watered);
                hoverIsland.currentState = Island.IslandState.Cultivated;
                break;
            case "Watering Can":
                hoverIsland.topMat = hoverIsland.potentialMatTop;
                hoverIsland.bottomMat = hoverIsland.potentialMatBottom;
                hoverIsland.CreateIslandMaterial(Island.IslandState.Sowed);
                hoverIsland.currentState = Island.IslandState.Watered;
                break;
            case "Grass Seed":
                hoverIsland.topMat = hoverIsland.potentialMatTop;
                hoverIsland.bottomMat = hoverIsland.potentialMatBottom;
                hoverIsland.CreateIslandMaterial(Island.IslandState.Cultivated);
                hoverIsland.currentState = Island.IslandState.Sowed;
                break;
            case "Concrete Bag":
                hoverIsland.topMat = hoverIsland.pavedMatTop;
                hoverIsland.bottomMat = hoverIsland.sowedMatBot;
                hoverIsland.CreateIslandMaterial(Island.IslandState.Paved);
                hoverIsland.currentState = Island.IslandState.Paved;
                break;
            default:
                hoverIsland.CreateIslandMaterial(hoverIsland.currentState);
                hoverIsland.islandMatPotential = true;
                hoverIsland.SetIslandMaterial();
                hoverIsland.CheckWarningIcon();
                break;
        }

        GameManager.HM.dragCard.dragSucces = true;
        GameManager.HM.dragCard.SetCardState(Card.CardState.Hidden);
        hoverIsland.UpdateNutrients();
    }

    private void HandleBuildableHover()
    {
        if (CheckPotentialPlot() != null)
        {
            if (hoverPlot != previousHoverPlot && previousHoverPlot != null)
            {
                previousHoverPlot.transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                hoverPlot.transform.GetChild(0).gameObject.SetActive(true);
            }

            if (GameManager.HM.dragCard.cardType == "Buildables")
            {
                hoverPlot.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = plotIndicatorOrange;
            }
            else
            {
                hoverPlot.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = plotIndicatorGreen;
            }

            AlignDragInstanceToPlot();
            previousHoverPlot = hoverPlot;
        }
        else if (hoverPlot != null)
        {
            hoverPlot.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void HandleBuildableDrop()
    {
        if (CheckPotentialPlot() != null)
        {
            hoverPlot.transform.GetChild(0).gameObject.SetActive(false);
            GameObject plant = Instantiate(dragInstance, Vector3.zero, Quaternion.identity, hoverPlot.transform);
            plant.transform.localPosition = new Vector3(0, -0.25f, 0);
            plant.transform.localRotation = Quaternion.identity;
            hoverIsland.MakeUsedPlot(hoverPlot, GameManager.HM.dragCard, plant.GetComponent<Plant>());
            GameManager.HM.dragCard.dragSucces = true;
            GameManager.HM.dragCard.SetCardState(Card.CardState.Hidden);
        }
        else
        {
            CancelDrag();
        }
    }

    private void UpdateIslandState(Island.IslandState newState)
    {
        hoverIsland.islandMatPotential = true;
        hoverIsland.previousState = hoverIsland.currentState;
        hoverIsland.currentState = newState;
    }

    private void AlignDragInstanceToPlot()
    {
        BoxCollider plotCollider = hoverPlot.GetComponent<BoxCollider>();
        BoxCollider dragCollider = dragInstance.GetComponent<BoxCollider>();
        Vector3 plotCenter = plotCollider.transform.position + plotCollider.center;
        Vector3 dragOffset = dragCollider.center;
        dragInstance.transform.position = new Vector3(
            plotCenter.x - dragOffset.x,
            plotCollider.bounds.max.y,
            plotCenter.z - dragOffset.z
        );
    }

    private void CancelDrag()
    {
        GameManager.HM.dragCard.dragSucces = false;
        GameManager.HM.dragCard.SetCardState(Card.CardState.InHand);
    }

    private GameObject CheckPotentialPlot()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, ~0, QueryTriggerInteraction.Collide);

        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Plot"))
            {
                GameObject plot = hit.collider.gameObject;
                hoverIsland = plot.transform.parent.parent.GetComponent<Island>();

                if (hoverIsland.usedSmallPlots.Contains(plot) ||
                    hoverIsland.usedMediumPlots.Contains(plot) ||
                    hoverIsland.usedLargePlots.Contains(plot))
                {
                    hoverPlot = null;
                    return null;
                }

                hoverPlot = plot;
                return plot;
            }
        }

        return null;
    }

    private Vector3 SetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane + adjustZ));
    }

    /*            
case "Buildable":
    if (CheckPotentialPlot() != null)
    {
        CheckPotentialPlot().transform.GetChild(0).gameObject.SetActive(false);

        GameObject plant = Instantiate(dragInstance, Vector3.zero, Quaternion.identity, hoverPlot.transform);
        plant.transform.localPosition = new Vector3(0, -0.25f, 0);
        plant.transform.localRotation = Quaternion.identity;

        plant.GetComponent<Plant>().attachedIsland = hoverIsland;
        hoverIsland.MakeUsedPlot(hoverPlot, GameManager.HM.dragCard, plant);

        expenseItem islandexpense = Instantiate(
            GameManager.ISM.expenseItem,
            Vector3.zero,
            Quaternion.identity,
            GameManager.ISM.buildableexpenseContent.transform
        );

        islandexpense.transform.localPosition = new Vector3(
            islandexpense.transform.localPosition.x,
            islandexpense.transform.localPosition.y,
            0
        );
        islandexpense.transform.localRotation = Quaternion.identity;
        islandexpense.SetupBuildableexpense(plant.GetComponent<Plant>());

        GameManager.HM.dragCard.dragSucces = true;
        GameManager.HM.dragCard.ToggleState(Card.CardState.Hidden, Card.CardState.Hidden);
    }
    else
    {
        GameManager.HM.dragCard.dragSucces = false;
        GameManager.HM.dragCard.ToggleState(Card.CardState.InHand, Card.CardState.InDrag);
    }
    break;
*/
}