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
    public GameObject previousHoverPlot;
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
            if (!collisionOn)
            {
                GameManager.ISM.SetCollisions(GameManager.HM.dragCard.cardType);
                collisionOn = true;
            }
            else
            {
                collisionOn = false;
            }
            switch (GameManager.HM.dragCard.cardType)
            {
                case "Utility":
                    if (GameManager.ISM.CheckPotentialIsland() != null)
                    {
                        if(hoverIsland != null)
                        {
                            previousHoverIsland = hoverIsland;
                        }
                        hoverIsland = GameManager.ISM.CheckPotentialIsland();
                        if (GameManager.HM.dragCard.cardName == "Cultivator" && GameManager.ISM.CheckPotentialIsland().currentState == Island.IslandState.Sowed)
                        {
                            hoverIsland.previousState = hoverIsland.currentState;
                            hoverIsland.ToggleState(Island.IslandState.Cultivated, hoverIsland.currentState);
                        }
                        if (GameManager.HM.dragCard.cardName == "Watering Can" && GameManager.ISM.CheckPotentialIsland().currentState == Island.IslandState.Cultivated)
                        {
                            hoverIsland.previousState = hoverIsland.currentState;
                            hoverIsland.ToggleState(Island.IslandState.Watered, hoverIsland.currentState);
                        }
                        if (GameManager.HM.dragCard.cardName == "Grass Seed" && GameManager.ISM.CheckPotentialIsland().currentState != Island.IslandState.Sowed)
                        {

                            hoverIsland.previousState = hoverIsland.currentState;
                            hoverIsland.ToggleState(Island.IslandState.Sowed, hoverIsland.currentState);
                        }
                    }
                    else
                    {
                        if(hoverIsland != null)
                        {
                            hoverIsland.ToggleState(hoverIsland.previousState, hoverIsland.currentState);
                            hoverIsland = null;
                        }
                    }
                    break;
                default:
                    if (CheckPotentialPlot() != null)
                    {
                        if (hoverPlot != previousHoverPlot && previousHoverPlot != null)
                        {
                            previousHoverPlot.transform.GetChild(0).gameObject.SetActive(false);
                        }
                        hoverPlot.transform.GetChild(0).gameObject.SetActive(true);
                        BoxCollider plotCollider = hoverPlot.GetComponent<BoxCollider>();
                        BoxCollider dragInstanceCollider = dragInstance.GetComponent<BoxCollider>();
                        Vector3 plotCenter = plotCollider.transform.position + plotCollider.center;
                        Vector3 dragInstanceCenterOffset = dragInstanceCollider.center;
                        dragInstance.transform.position = new Vector3(plotCenter.x - dragInstanceCenterOffset.x, plotCollider.bounds.max.y, plotCenter.z - dragInstanceCenterOffset.z);
                        previousHoverPlot = hoverPlot;
                    }
                    else
                    {
                        if(hoverPlot != null)
                        {
                            hoverPlot.transform.GetChild(0).gameObject.SetActive(false);
                        }
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
        switch (GameManager.HM.dragCard.cardType)
        {
            case "Utility":
                if (hoverIsland != null)
                {
                    if(GameManager.HM.dragCard.nutrientIndex != 0)
                    {
                        hoverIsland.nutrientsAvailable[GameManager.HM.dragCard.nutrientIndex-1] += 50;
                    }
                    GameManager.ISM.UpdateIslandMaterial(hoverIsland);
                    hoverIsland.UpdateNutrientsRequired();
                    GameManager.HM.dragCard.dragSucces = true;
                    GameManager.HM.dragCard.ToggleState(Card.CardState.Hidden, Card.CardState.Hidden);
                }
                else
                {
                    hoverIsland = null;
                    GameManager.HM.dragCard.dragSucces = false;
                    GameManager.HM.dragCard.ToggleState(Card.CardState.InHand, Card.CardState.InDrag);
                }
                break;
            case "Buildable":
                if (CheckPotentialPlot() != null)
                {
                    CheckPotentialPlot().transform.GetChild(0).gameObject.SetActive(false);
                    GameObject plant = Instantiate(dragInstance, Vector3.zero, Quaternion.identity, hoverPlot.transform);
                    plant.transform.localPosition = new Vector3(0, -0.25f, 0);
                    plant.transform.localRotation = Quaternion.identity;
                    plant.GetComponent<Plant>().attachedIsland = hoverIsland;
                    hoverIsland.MakeUsedPlot(hoverPlot, GameManager.HM.dragCard, plant);
                    ExpenseItem islandExpense = Instantiate(GameManager.ISM.expenseItem, Vector3.zero, Quaternion.identity, GameManager.ISM.buildableExpenseContent.transform);
                    islandExpense.transform.localPosition = new Vector3(islandExpense.transform.localPosition.x, islandExpense.transform.localPosition.y, 0);
                    islandExpense.transform.localRotation = Quaternion.identity;
                    islandExpense.SetupBuildableExpense(plant.GetComponent<Plant>());
                    GameManager.HM.dragCard.dragSucces = true;
                    GameManager.HM.dragCard.ToggleState(Card.CardState.Hidden, Card.CardState.Hidden);
                }
                else
                {
                    GameManager.HM.dragCard.dragSucces = false;
                    GameManager.HM.dragCard.ToggleState(Card.CardState.InHand, Card.CardState.InDrag);
                }
                break;
            default:
                if (CheckPotentialPlot() != null)
                {
                    CheckPotentialPlot().transform.GetChild(0).gameObject.SetActive(false);
                    GameObject plant = Instantiate(dragInstance, Vector3.zero, Quaternion.identity, hoverPlot.transform);
                    plant.transform.localPosition = new Vector3(0, -0.25f, 0);
                    plant.transform.localRotation = Quaternion.identity;
                    hoverIsland.MakeUsedPlot(hoverPlot, GameManager.HM.dragCard, plant);
                    hoverIsland.UpdateNutrientsRequired();
                    GameManager.HM.dragCard.dragSucces = true;
                    GameManager.HM.dragCard.ToggleState(Card.CardState.Hidden, Card.CardState.Hidden);
                }
                else
                {
                    GameManager.HM.dragCard.dragSucces = false;
                    GameManager.HM.dragCard.ToggleState(Card.CardState.InHand, Card.CardState.InDrag);
                }
                break;
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
