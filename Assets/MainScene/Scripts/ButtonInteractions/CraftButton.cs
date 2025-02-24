using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CraftButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float holdTime;
    public float nextSpawnTime;

    private void Update()
    {
        if (GameManager.CRM.isCrafting)
        {
            holdTime += Time.deltaTime;
            float fillAmount = Mathf.Clamp01(1f - (holdTime * GameManager.CRM.craftSpeed));
            GameManager.CRM.craftUI.craftCardCover.GetComponent<Slider>().value = fillAmount;
            float spawnInterval = (5f / GameManager.CRM.biggestResourceCost);
            if (fillAmount <= nextSpawnTime)
            {
                GameManager.CRM.craftUI.SpawnResources(GameManager.CRM.selectedCard.cardCraftResources);
                nextSpawnTime -= spawnInterval;
            }
            if (fillAmount == 0)
            {
                GameManager.CRM.isCrafting = false;
                GameManager.CRM.craftButton.GetComponent<Image>().color = Color.green;
                GameManager.CRM.craftButtonText.SetText("Card crafted");
                GameManager.CRM.craftSuccess = true;
                if (GameManager.TTM.tutorial && GameManager.TTM.tutorialCount == 14)
                {
                    GameManager.CRM.selectedCard.GetComponent<Image>().color = new Color(0.74f, 0.74f, 0.74f);
                    GameManager.TTM.QuestCompleted = true;
                }
            }
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if(GameManager.CRM.CheckValidCraft())
        {
            GameManager.CRM.isCrafting = true;
            GameManager.CRM.craftSuccess = false;
            holdTime = 0f;
            nextSpawnTime = 1f;
            Card craftCard = GameManager.CRM.selectedCard;
            foreach (var slot in GameManager.CRM.selectionSlots)
            {
                slot.SetActive(false);
            }
            GameManager.CRM.craftButtonText.SetText("Crafting....");
            GameManager.CRM.craftButton.GetComponent<Image>().color = new Color(1f, 0.64f, 0f);
            GameObject centerSlot = GameManager.CRM.selectionSlots[2];
            centerSlot.SetActive(true);
            GameManager.CRM.craftUI.craftCardCover.SetActive(true);
            GameManager.CRM.UpdateCardSlot(craftCard, new Vector3(0.6f, 0.6f, 1f), Vector3.zero, false);
            GameManager.CRM.biggestResourceCost = Mathf.Max(craftCard.cardCraftResources[0], craftCard.cardCraftResources[1], craftCard.cardCraftResources[2]);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(GameManager.CRM.CheckValidCraft())
        {
            if (!GameManager.CRM.craftSuccess)
            {
                foreach (Resource resource in GameManager.CRM.craftUI.craftResources)
                {
                    Destroy(resource.gameObject);
                }
                GameManager.CRM.craftUI.craftResources.Clear();
                GameManager.CRM.craftButton.GetComponent<Image>().color = Color.red;
                GameManager.CRM.craftButtonText.SetText("Unsuccessful");
                if (GameManager.TTM.tutorialCount == 14 || GameManager.TTM.tutorialCount == 15)
                {
                    GameManager.CRM.SetCardCraftAmount(1);
                    GameManager.CRM.craftButton.GetComponent<Image>().color = Color.green;
                }
            }
            else
            {
                GameManager.UM.balance -= GameManager.CRM.selectedCard.cardCraftResources[0];
                GameManager.UM.water -= GameManager.CRM.selectedCard.cardCraftResources[1];
                GameManager.UM.fertilizer -= GameManager.CRM.selectedCard.cardCraftResources[2];
                GameManager.DM.AddCardToDeck(GameManager.CRM.selectedCard.cardId);
                GameManager.CRM.SetCardCraftAmount(0);
                GameManager.UM.UpdateUI();
                if(GameManager.TTM.tutorial)
                {
                    if(GameManager.CRM.selectedCard.cardName == "Phosphorus Fertilizer" || GameManager.CRM.selectedCard.cardName == "Potassium Fertilizer" || GameManager.CRM.selectedCard.cardName == "Nitrogen Fertilizer")
                    {
                        GameManager.CRM.selectedCard.GetComponent<Image>().color = new Color(0.74f, 0.74f, 0.74f);
                        GameManager.CRM.matchingCard = false;
                        GameManager.CRM.CheckSelectedCard();
                    }
                }
            }
            GameManager.CRM.ResetCraftCard();
        }
    }
}
