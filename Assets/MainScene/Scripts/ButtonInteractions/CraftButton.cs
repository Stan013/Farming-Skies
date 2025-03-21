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
                GameManager.CRM.craftButton.GetComponent<Image>().sprite = GameManager.CRM.successCraft;
                GameManager.CRM.craftButtonText.SetText("Craft success");
                GameManager.CRM.craftSuccess = true;
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
            GameManager.CRM.craftButton.GetComponent<Image>().sprite = GameManager.CRM.validCraft;
            GameManager.CRM.craftButtonText.SetText("Crafting...");
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
                GameManager.CRM.craftButton.GetComponent<Image>().sprite = GameManager.CRM.validCraft;
                GameManager.CRM.craftButtonText.SetText("Craft card");
            }
            else
            {
                GameManager.UM.balance -= GameManager.CRM.selectedCard.cardCraftResources[0];
                GameManager.UM.water -= GameManager.CRM.selectedCard.cardCraftResources[1];
                GameManager.UM.fertiliser -= GameManager.CRM.selectedCard.cardCraftResources[2];
                GameManager.DM.AddCardToDeck(GameManager.CRM.selectedCard.cardId);
                GameManager.CRM.SetCardCraftAmount(0);
                GameManager.UM.UpdateUI();
                GameManager.CRM.CalculateMaxCraftableAmount();
            }
            GameManager.CRM.ResetCraftCard();
        }
    }
}
