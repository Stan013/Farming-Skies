using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public GameObject dragModel;
    private float adjustZ = 5f;
    private GameObject dragInstance;
    public Island hoverIsland;
    private Island previousHoverIsland;
    public GameObject hoverPlot;
    private bool collisionOn = false;
    private Quaternion dragInstanceRotation;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GameManager.HM.dragging == false && GameManager.CM.inspectCard == null)
        {
            GameManager.HM.dragging = true;
            GameManager.HM.dragCard = gameObject.GetComponent<Card>();
            GameManager.HM.dragCard.ToggleState(Card.CardState.InDrag, Card.CardState.InHand);
            dragInstance = Instantiate(dragModel, SetMousePosition(), Quaternion.identity);
        }
        else
        {
            eventData.pointerDrag = null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragInstance != null)
        {
            dragInstance.transform.position = SetMousePosition();
            dragInstance.transform.LookAt(GameManager.cam.transform);
            dragInstanceRotation = Quaternion.Euler(0, dragInstance.transform.rotation.eulerAngles.y - 90, dragInstance.transform.rotation.eulerAngles.z);
            dragInstance.transform.rotation = dragInstanceRotation;
            switch (GameManager.HM.dragCard.cardType)
            {
                case "Utility":
                    if (!collisionOn)
                    {
                        GameManager.ISM.SetCollisions("Utility");
                        collisionOn = true;
                    }
                    else
                    {
                        collisionOn = false;
                    }
                    if (GameManager.ISM.CheckPotentialIsland() != null)
                    {
                        hoverIsland = GameManager.ISM.CheckPotentialIsland();
                        if (GameManager.HM.dragCard.name.Contains("CultivatorCard") && hoverIsland.currentState == Island.IslandState.Sowed && hoverIsland.potentialState != Island.IslandState.Cultivated)
                        {
                            hoverIsland.TogglePotentialState(Island.IslandState.Cultivated, Island.IslandState.Sowed);
                            previousHoverIsland = GameManager.ISM.CheckPotentialIsland();
                        }
                        if (GameManager.HM.dragCard.name.Contains("WateringCanCard") && hoverIsland.currentState == Island.IslandState.Cultivated && hoverIsland.potentialState != Island.IslandState.Watered)
                        {
                            hoverIsland.TogglePotentialState(Island.IslandState.Watered, Island.IslandState.Cultivated);
                            previousHoverIsland = hoverIsland;
                        }
                        if (GameManager.HM.dragCard.name.Contains("GrassSeedCard") && hoverIsland.currentState != Island.IslandState.Sowed && hoverIsland.potentialState != Island.IslandState.Sowed)
                        {
                            hoverIsland.TogglePotentialState(Island.IslandState.Sowed, Island.IslandState.Watered);
                            previousHoverIsland = hoverIsland;
                        }
                    }
                    else
                    {
                        if(previousHoverIsland != null)
                        {
                            previousHoverIsland.ToggleState(previousHoverIsland.currentState, previousHoverIsland.currentState);
                            previousHoverIsland.potentialState = previousHoverIsland.currentState;
                            previousHoverIsland = null;
                        }
                    }
                    break;
                default:
                    if (!collisionOn)
                    {
                        switch (GameManager.HM.dragCard.cardType)
                        {
                            case "PlantSmall":
                                GameManager.ISM.SetCollisions("PlantSmall");
                                break;
                            case "PlantMedium":
                                GameManager.ISM.SetCollisions("PlantMedium");
                                break;
                            case "PlantBig":
                                GameManager.ISM.SetCollisions("PlantBig");
                                break;
                            case "Machine":
                                GameManager.ISM.SetCollisions("Machine");
                                break;

                        }
                        collisionOn = true;
                    }
                    else
                    {
                        collisionOn = false;
                    }
                    if (CheckPotentialPlot() != null)
                    {
                        BoxCollider plotCollider = CheckPotentialPlot().GetComponent<BoxCollider>();
                        BoxCollider dragInstanceCollider = dragInstance.GetComponent<BoxCollider>();
                        Vector3 plotCenter = plotCollider.transform.position + plotCollider.center;
                        Vector3 dragInstanceCenterOffset = dragInstanceCollider.center;
                        dragInstance.transform.position = new Vector3(plotCenter.x - dragInstanceCenterOffset.x, plotCollider.bounds.max.y, plotCenter.z - dragInstanceCenterOffset.z);
                    }
                    else
                    {
                        return;
                    }
                    break;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameManager.HM.dragging = false;
        collisionOn = false;
        if (hoverIsland != null && hoverIsland.currentState != hoverIsland.potentialState && GameManager.ISM.CheckPotentialIsland() != null)
        {
            GameManager.ISM.CheckPotentialIsland().ToggleState(hoverIsland.potentialState, hoverIsland.currentState);
            if (GameManager.HM.dragCard.cardName == "Watering Can")
            {
                GameManager.ISM.CheckPotentialIsland().water += 50;
            }
            if (GameManager.TTM.tutorialCount == 3 || GameManager.TTM.tutorialCount == 4)
            {
                GameManager.TTM.QuestCompleted = true;
            }
            GameManager.HM.dragCard.dragSucces = true;
            GameManager.HM.dragCard.ToggleState(Card.CardState.Hidden, Card.CardState.Hidden);
        }
        else
        {
            if (GameManager.ISM.CheckPotentialIsland() != null)
            {
                switch (GameManager.HM.dragCard.cardName)
                {
                    case "Nitrogen Fertilizer":
                        GameManager.ISM.CheckPotentialIsland().Nitrogen += 50;
                        GameManager.HM.dragCard.dragSucces = true;
                        GameManager.HM.dragCard.ToggleState(Card.CardState.Hidden, Card.CardState.Hidden);
                        break;
                    case "Phosphorus Fertilizer":
                        GameManager.ISM.CheckPotentialIsland().Phosphorus += 50;
                        GameManager.HM.dragCard.dragSucces = true;
                        GameManager.HM.dragCard.ToggleState(Card.CardState.Hidden, Card.CardState.Hidden);
                        break;
                    case "Potassium Fertilizer":
                        GameManager.ISM.CheckPotentialIsland().Potassium += 50;
                        GameManager.HM.dragCard.dragSucces = true;
                        GameManager.HM.dragCard.ToggleState(Card.CardState.Hidden, Card.CardState.Hidden);
                        break;
                    default:
                        break;
                }
            }
            if (CheckPotentialPlot() != null)
            {
                GameObject plant = Instantiate(dragInstance, Vector3.zero, Quaternion.identity);
                plant.transform.SetParent(hoverPlot.transform);
                plant.transform.localPosition = new Vector3(0, -0.25f, 0);
                hoverIsland.MakeUsedPlot(hoverPlot, GameManager.HM.dragCard, plant);
                GameManager.HM.dragCard.dragSucces = true;
                GameManager.HM.dragCard.ToggleState(Card.CardState.Hidden, Card.CardState.Hidden);

                if (GameManager.TTM.tutorialCount == 6)
                {
                    GameManager.TTM.QuestCompleted = true;
                }
                if (GameManager.TTM.tutorialCount == 7 && GameManager.HM.cardsInHand.Count == 0)
                {
                    GameManager.TTM.QuestCompleted = true;
                }
            }
            else
            {
                GameManager.HM.dragCard.dragSucces = false;
                GameManager.HM.dragCard.ToggleState(Card.CardState.InHand, Card.CardState.InDrag);
            }
        }
        Destroy(dragInstance);
        GameManager.ISM.SetCollisions("Reset");
    }

    public GameObject CheckPotentialPlot()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, ~0, QueryTriggerInteraction.Collide);
        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Plot"))
            {
                var plot = hit.collider.gameObject;
                var plotParent = plot.transform.parent;
                hoverIsland = plotParent.transform.parent.GetComponent<Island>();
                if (hoverIsland.usedPlots.Contains(plot))
                {
                    hoverPlot = null;
                    return null;
                }
                else
                {
                    hoverPlot = plot;
                    return plot;
                }
            }
        }
        return null;
    }

    private Vector3 SetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + adjustZ));
    }
}
