using System;
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
    private Island previousIsland;
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

        hoverIsland = GameManager.ISM.GetPotentialBoughtIsland();
        UpdateDragInstanceTransform();
        HandleIslandCollisions(hoverIsland);

        switch (GameManager.HM.dragCard.cardType)
        {
            case "Utilities":
                HandleUtilityHover(hoverIsland);
                break;
            case "Structure":
                HandleStructureHover(hoverIsland);
                break;
            default:
                HandleCropHover(hoverIsland);
                break;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        hoverIsland = GameManager.ISM.GetPotentialBoughtIsland();
        GameManager.HM.dragging = false;
        collisionOn = false;

        switch (GameManager.HM.dragCard.cardType)
        {
            case "Utilities":
                HandleUtilityDrop(hoverIsland);
                break;
            case "Structure":
                HandleStructureDrop(hoverIsland);
                break;
            default:
                HandleCropDrop(hoverIsland);
                break;
        }

        previousIsland = null;
        hoverIsland = null;
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

    private void HandleIslandCollisions(Island hoverIsland)
    {
        if (!collisionOn && hoverIsland != null)
        {
            GameManager.ISM.SetupIslandCollisions(true);
            hoverIsland.SetCollisions(GameManager.HM.dragCard.cardType);
            collisionOn = true;
        }
    }

    private void HandleUtilityHover(Island hoverIsland)
    {
        if (hoverIsland == null)
        {
            if(previousIsland != null)
            {
                previousIsland.SetIslandMaterial(false);
            }
            return;
        }

        hoverIsland.MaterialDragValidation(GameManager.HM.dragCard.cardName);
        hoverIsland.SetIslandMaterial(true);
        previousIsland = hoverIsland;
    }

    private void HandleStructureHover(Island hoverIsland)
    {
        if (hoverIsland == null || CheckPotentialPlot(hoverIsland) == null)
        {
            if (previousHoverPlot != null)
            {
                previousHoverPlot.transform.GetChild(0).gameObject.SetActive(false);
                previousHoverPlot = null;
            }
            return;
        }

        if (hoverPlot != previousHoverPlot && previousHoverPlot != null)
        {
            previousHoverPlot.transform.GetChild(0).gameObject.SetActive(false);
        }

        hoverPlot.transform.GetChild(0).gameObject.SetActive(true);
        hoverPlot.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = plotIndicatorOrange;
        AlignDragInstanceToPlot(hoverPlot);
        previousHoverPlot = hoverPlot;
    }

    private void HandleCropHover(Island hoverIsland)
    {
        if (hoverIsland == null || CheckPotentialPlot(hoverIsland) == null)
        {
            if (previousHoverPlot != null)
            {
                previousHoverPlot.transform.GetChild(0).gameObject.SetActive(false);
                previousHoverPlot = null;
            }
            return;
        }

        if (hoverPlot != previousHoverPlot && previousHoverPlot != null)
        {
            previousHoverPlot.transform.GetChild(0).gameObject.SetActive(false);
        }

        hoverPlot.transform.GetChild(0).gameObject.SetActive(true);
        hoverPlot.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = plotIndicatorGreen;
        AlignDragInstanceToPlot(hoverPlot);
        previousHoverPlot = hoverPlot;
    }

    private void HandleUtilityDrop(Island hoverIsland)
    {
        if (hoverIsland == null)
        {
            if (previousIsland != null)
                previousIsland.hoverMatSetup = false;

            CancelDrag();
            return;
        }

        hoverIsland.hoverMatSetup = false;

        var dragCard = GameManager.HM.dragCard;
        int nutrientIndex = dragCard.nutrientIndex;

        if (nutrientIndex != 0)
            hoverIsland.nutrientsAvailable[nutrientIndex - 1] += dragCard.nutrientAddition;

        if (!hoverIsland.validPotentialMat)
        {
            CancelDrag();
            return;
        }

        dragCard.dragSucces = true;
        dragCard.SetCardState(Card.CardState.Hidden);

        hoverIsland.UpdateNutrients();
        hoverIsland.previousState = hoverIsland.currentState;
        hoverIsland.currentState = hoverIsland.potentialState;
    }

    private void HandleStructureDrop(Island hoverIsland)
    {
        if (hoverIsland == null || hoverPlot == null)
        {
            CancelDrag();
            return;
        }

        hoverPlot.transform.GetChild(0).gameObject.SetActive(false);
        GameObject plant = Instantiate(dragInstance, Vector3.zero, Quaternion.identity, hoverPlot.transform);
        plant.transform.localPosition = new Vector3(0, -0.25f, 0);
        plant.transform.localRotation = Quaternion.identity;
        hoverIsland.MakeUsedPlot(hoverPlot, GameManager.HM.dragCard, plant.GetComponent<Plant>());
        GameManager.HM.dragCard.dragSucces = true;
        GameManager.HM.dragCard.SetCardState(Card.CardState.Hidden);
    }

    private void HandleCropDrop(Island hoverIsland)
    {
        if (hoverIsland == null || hoverPlot == null)
        {
            CancelDrag();
            return;
        }

        hoverPlot.transform.GetChild(0).gameObject.SetActive(false);
        GameObject plant = Instantiate(dragInstance, Vector3.zero, Quaternion.identity, hoverPlot.transform);
        plant.transform.localPosition = new Vector3(0, -0.25f, 0);
        plant.transform.localRotation = Quaternion.identity;
        hoverIsland.MakeUsedPlot(hoverPlot, GameManager.HM.dragCard, plant.GetComponent<Plant>());
        GameManager.HM.dragCard.dragSucces = true;
        GameManager.HM.dragCard.SetCardState(Card.CardState.Hidden);
        hoverIsland.UpdateNutrients();

    }

    private void AlignDragInstanceToPlot(GameObject hoverPlot)
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
        previousIsland = null;
        hoverIsland = null;
        GameManager.HM.dragCard.dragSucces = false;
        GameManager.HM.dragCard.SetCardState(Card.CardState.InHand);
    }

    private GameObject CheckPotentialPlot(Island hoverIsland)
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
}