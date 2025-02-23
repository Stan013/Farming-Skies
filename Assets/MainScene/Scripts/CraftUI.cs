using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CraftUI : MonoBehaviour
{
    public Button minButton, minusButton, plusButton, maxButton;
    public TMP_InputField craftAmountInput;
    public TMP_Text balanceCostText, waterCostText, fertilizerCostText;
    public GameObject craftCardCover;
    public Sprite waterCraftIcon, coinCraftIcon, fertilizerCraftIcon;
    public RectTransform spawnArea1, spawnArea2;
    public Resource resource;
    public List<Resource> craftResources;
    public int spawnAreaIndex;
    public int coinCost, waterCost, fertilizerCost;
    public Button leftButton, rightButton;

    public void SetupCraftingUI()
    {
        minButton.onClick.AddListener(() => GameManager.CRM.SetCardCraftAmount(0));
        minusButton.onClick.AddListener(() => GameManager.CRM.SetCardCraftAmount(GameManager.CRM.cardCraftAmount - 1));
        plusButton.onClick.AddListener(() => GameManager.CRM.SetCardCraftAmount(GameManager.CRM.cardCraftAmount + 1));
        maxButton.onClick.AddListener(() => GameManager.CRM.SetCardCraftAmount(GameManager.CRM.maxCraftableAmount));
        craftAmountInput.onValueChanged.AddListener(OnCraftAmountInputChanged);
    }

    private void OnCraftAmountInputChanged(string input)
    {
        GameManager.CRM.CheckValidCraft();
        if (int.TryParse(input, out int value))
        {
            GameManager.CRM.SetCardCraftAmount(value);
        }
        else
        {
            craftAmountInput.text = GameManager.CRM.cardCraftAmount.ToString();
        }
    }

    public void UpdateCostDisplay()
    {
        if (GameManager.CRM.selectedCard == null) return;

        coinCost = GameManager.CRM.selectedCard.cardCraftResources[0] * GameManager.CRM.cardCraftAmount;
        waterCost = GameManager.CRM.selectedCard.cardCraftResources[1] * GameManager.CRM.cardCraftAmount;
        fertilizerCost = GameManager.CRM.selectedCard.cardCraftResources[2] * GameManager.CRM.cardCraftAmount;
        balanceCostText.text = coinCost.ToString() + " ₴";
        waterCostText.text = waterCost.ToString() + " L";
        fertilizerCostText.text = fertilizerCost.ToString() + " L";
    }

    public void SpawnResources(int[] neededResources)
    {
        if (neededResources[0] > 0 || neededResources[1] > 0 || neededResources[2] > 0)
        {
            if (neededResources[0] > 0)
            {
                spawnAreaIndex++;
                SpawnSingleResource(spawnAreaIndex, "Coin");
                coinCost -= 5;
            }

            if (neededResources[1] > 0)
            {
                spawnAreaIndex++;
                SpawnSingleResource(spawnAreaIndex, "Water");
                waterCost -= 5;
            }

            if (neededResources[2] > 0)
            {
                spawnAreaIndex++;
                SpawnSingleResource(spawnAreaIndex, "Fertilizer");
                fertilizerCost -= 5;
            }
        }
    }

    private void SpawnSingleResource(int index, string type)
    {
        Resource newResource = Instantiate(resource, Vector3.zero, Quaternion.identity);
        craftResources.Add(newResource);
        if (index % 2 == 0)
        {
            newResource.transform.SetParent(spawnArea1);
            newResource.transform.localPosition = GetRandomPositionInSpawnArea(spawnArea1);

        }
        else
        {
            newResource.transform.SetParent(spawnArea2);
            newResource.transform.localPosition = GetRandomPositionInSpawnArea(spawnArea2);
        }
        newResource.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
        newResource.transform.localRotation = Quaternion.identity;
        newResource.resourceImage.sprite = GetResourceSprite(type);
        newResource.targetPosition = craftCardCover.transform.position;
        newResource.moving = true;
    }


    private Vector2 GetRandomPositionInSpawnArea(RectTransform spawnArea)
    {
        Vector2 minBounds = (Vector2)spawnArea.position - (spawnArea.rect.size * 0.5f);
        Vector2 maxBounds = (Vector2)spawnArea.position + (spawnArea.rect.size * 0.5f);

        float randomX = Random.Range(minBounds.x, maxBounds.x);
        float randomY = Random.Range(minBounds.y, maxBounds.y);

        return new Vector2(randomX, randomY);
    }

    private Sprite GetResourceSprite(string type)
    {
        if (type == "Coin") return coinCraftIcon;
        if (type == "Water") return waterCraftIcon;
        return fertilizerCraftIcon;
    }
}
