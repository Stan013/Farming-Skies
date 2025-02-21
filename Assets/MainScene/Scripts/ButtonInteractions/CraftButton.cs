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
                nextSpawnTime -= spawnInterval;
                GameManager.CRM.craftUI.SpawnResources(GameManager.CRM.selectedCard.cardCraftResources);
                if (nextSpawnTime <= 0)
                {
                    GameManager.CRM.isCrafting = false;
                    GameManager.CRM.craftButton.GetComponent<Image>().color = Color.green;
                    GameManager.CRM.craftButtonText.SetText("Card crafted");
                    GameManager.CRM.craftSuccess = true;
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
            GameManager.CRM.AssignCardToSlot(centerSlot, craftCard, new Vector3(0.6f, 0.6f, 1f), Vector3.zero, false);
            GameManager.CRM.biggestResourceCost = Mathf.Max(craftCard.cardCraftResources[0], craftCard.cardCraftResources[1], craftCard.cardCraftResources[2]);
        }
        else
        {

        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(GameManager.CRM.CheckValidCraft())
        {
            if (!GameManager.CRM.craftSuccess)
            {
                foreach (GameObject resource in GameManager.CRM.craftUI.craftResources)
                {
                    Destroy(resource.gameObject);
                }
                GameManager.CRM.craftUI.craftResources.Clear();
                GameManager.CRM.craftButton.GetComponent<Image>().color = Color.red;
                GameManager.CRM.craftButtonText.SetText("Unsuccessful");
            }
            GameManager.UM.balance -= GameManager.CRM.craftUI.coinCost;
            GameManager.UM.water -= GameManager.CRM.craftUI.waterCost;
            GameManager.UM.fertilizer -= GameManager.CRM.craftUI.fertilizerCost;
            GameManager.DM.AddCardToDeck(GameManager.CRM.selectedCard.cardId);
            GameManager.CRM.ResetCraftCard();
            GameManager.CRM.SetCardCraftAmount(0);
        }
    }
}
