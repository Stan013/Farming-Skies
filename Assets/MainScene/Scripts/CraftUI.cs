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
    public Button craftCardButton;
    public GameObject craftCardCover;
    public Sprite waterCraftIcon, coinCraftIcon, fertilizerCraftIcon;
    public Transform craftEffectParent;
    public Transform cardTargetPosition;
    public RectTransform spawnArea1, spawnArea2; 

    private bool isCrafting = false;
    private float craftDuration = 2f;
    private float craftProgress = 0f;
    private Coroutine craftingCoroutine;
    private bool useFirstSpawnArea = true;

    void Start()
    {
        AddEventTrigger(craftCardButton.gameObject, EventTriggerType.PointerDown, () => StartCrafting());
        AddEventTrigger(craftCardButton.gameObject, EventTriggerType.PointerUp, () => StopCrafting());

        minButton.onClick.AddListener(() => SetCraftAmount(0));
        minusButton.onClick.AddListener(() => SetCraftAmount(GameManager.CRM.cardCraftAmount - 1));
        plusButton.onClick.AddListener(() => SetCraftAmount(GameManager.CRM.cardCraftAmount + 1));
        maxButton.onClick.AddListener(() => SetCraftAmount(GameManager.CRM.maxCraftableAmount));

        craftAmountInput.onValueChanged.AddListener(OnCraftAmountInputChanged);
        GameManager.CRM.CalculateMaxCraftableAmount();
        UpdateCostDisplay();
    }

    void AddEventTrigger(GameObject obj, EventTriggerType eventType, System.Action action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>() ?? obj.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener((eventData) => action());
        trigger.triggers.Add(entry);
    }


    private void SetCraftAmount(int amount)
    {
        GameManager.CRM.cardCraftAmount = Mathf.Clamp(amount, 0, GameManager.CRM.maxCraftableAmount);
        craftAmountInput.text = GameManager.CRM.cardCraftAmount.ToString();
        UpdateCostDisplay();
    }

    private void OnCraftAmountInputChanged(string input)
    {
        if (int.TryParse(input, out int value))
        {
            SetCraftAmount(value);
        }
        else
        {
            craftAmountInput.text = GameManager.CRM.cardCraftAmount.ToString();
        }
    }

    private void UpdateCostDisplay()
    {
        if (GameManager.CRM.selectedCard == null) return;

        float coinCost = GameManager.CRM.selectedCard.cardCraftResources[0] * GameManager.CRM.cardCraftAmount;
        int waterCost = GameManager.CRM.selectedCard.cardCraftResources[1] * GameManager.CRM.cardCraftAmount;
        int fertilizerCost = GameManager.CRM.selectedCard.cardCraftResources[2] * GameManager.CRM.cardCraftAmount;

        balanceCostText.text = coinCost.ToString() + " ₴";
        waterCostText.text = waterCost.ToString() + " L";
        fertilizerCostText.text = fertilizerCost.ToString() + " L";
    }

    private void StartCrafting()
    {
        if (craftingCoroutine != null) StopCoroutine(craftingCoroutine);
        craftingCoroutine = StartCoroutine(CraftProcess());
    }

    private void StopCrafting()
    {
        if (craftingCoroutine != null)
        {
            StopCoroutine(craftingCoroutine);
            craftingCoroutine = null;
        }
        ResetCrafting();
    }

    private IEnumerator CraftProcess()
    {
        isCrafting = true;
        craftProgress = 0f;
        float spawnInterval = craftDuration / (GameManager.CRM.cardCraftAmount * 3);

        while (craftProgress < craftDuration)
        {
            craftProgress += Time.deltaTime;
            float alpha = 1f - (craftProgress / craftDuration);
            SetCraftCoverAlpha(alpha);

            if (craftProgress % spawnInterval < Time.deltaTime)
            {
                SpawnCraftingResources();
            }

            yield return null;
        }

        CompleteCrafting();
    }

    private void ResetCrafting()
    {
        isCrafting = false;
        SetCraftCoverAlpha(1f);
    }

    private void CompleteCrafting()
    {
        isCrafting = false;
        SetCraftCoverAlpha(0f);

        float totalCoinCost = GameManager.CRM.selectedCard.cardCraftResources[0] * GameManager.CRM.cardCraftAmount;
        int totalWaterCost = GameManager.CRM.selectedCard.cardCraftResources[1] * GameManager.CRM.cardCraftAmount;
        int totalFertilizerCost = GameManager.CRM.selectedCard.cardCraftResources[2] * GameManager.CRM.cardCraftAmount;

        if (GameManager.UM.balance >= totalCoinCost &&
            GameManager.UM.water >= totalWaterCost &&
            GameManager.UM.fertilizer >= totalFertilizerCost)
        {
            GameManager.UM.balance -= totalCoinCost;
            GameManager.UM.water -= totalWaterCost;
            GameManager.UM.fertilizer -= totalFertilizerCost;

            for (int i = 0; i < GameManager.CRM.cardCraftAmount; i++)
            {
                GameManager.DM.AddCardToDeck(GameManager.CRM.selectedCard.cardId);
            }
        }
        else
        {
            Debug.Log("Not enough resources to craft the card.");
        }

        GameManager.CRM.CalculateMaxCraftableAmount();
        SetCraftAmount(0);
    }

    private void SetCraftCoverAlpha(float alpha)
    {
        Color color = craftCardCover.GetComponent<Image>().color;
        color.a = Mathf.Clamp01(alpha);
        craftCardCover.GetComponent<Image>().color = color;
    }

    private void SpawnCraftingResources()
    {
        if (GameManager.CRM.selectedCard == null) return;

        int totalResources = GameManager.CRM.cardCraftAmount * 3;

        for (int i = 0; i < totalResources; i++)
        {
            Sprite resourceSprite = GetResourceSprite(i);
            GameObject resourceIcon = new GameObject("ResourceIcon");
            Image iconImage = resourceIcon.AddComponent<Image>();
            iconImage.sprite = resourceSprite;
            resourceIcon.transform.SetParent(craftEffectParent, false);
            RectTransform chosenSpawnArea = useFirstSpawnArea ? spawnArea1 : spawnArea2;
            useFirstSpawnArea = !useFirstSpawnArea;

            Vector2 randomSpawnPos = GetRandomPositionInSpawnArea(chosenSpawnArea);
            resourceIcon.transform.localPosition = randomSpawnPos;

            StartCoroutine(MoveResourceToCard(resourceIcon.transform, cardTargetPosition.position, 0.8f));
        }
    }

    private Vector2 GetRandomPositionInSpawnArea(RectTransform spawnArea)
    {
        Vector2 minBounds = spawnArea.rect.min;
        Vector2 maxBounds = spawnArea.rect.max;
        return new Vector2(Random.Range(minBounds.x, maxBounds.x), Random.Range(minBounds.y, maxBounds.y));
    }

    private IEnumerator MoveResourceToCard(Transform resourceTransform, Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0;
        Vector3 startPosition = resourceTransform.localPosition;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            resourceTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        Destroy(resourceTransform.gameObject);
    }

    private Sprite GetResourceSprite(int index)
    {
        int type = index % 3;
        if (type == 0) return coinCraftIcon;
        if (type == 1) return waterCraftIcon;
        return fertilizerCraftIcon;
    }
}
