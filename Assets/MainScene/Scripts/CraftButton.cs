using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CraftButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Slider craftProgressBar;
    public Image progressBarFill;
    public Color startColor;
    public Color endColor;
    public TMP_Text craftCardText;

    private bool isHoldingButton = false;
    private float craftProgress = 0f;
    private readonly float craftTime = 2f;

    private Coroutine resetCoroutine;

    void Update()
    {
        // If the player is not holding the button, no progress happens.
        if (isHoldingButton)
        {
            craftProgress += Time.deltaTime / craftTime;
            craftProgressBar.value = craftProgress;
            progressBarFill.color = Color.Lerp(startColor, endColor, craftProgress);

            // If progress reaches 100%, complete the crafting
            if (craftProgress >= 1f)
            {
                CompleteCrafting();
            }
        }
    }

    // Triggered when the player presses the button
    public void OnPointerDown(PointerEventData eventData)
    {
        // Only start crafting if the pointer is within the slider bounds
        if (craftProgressBar.interactable)
        {
            // Check if resources are enough only when pressing the slider
            if (GameManager.UM.water < GameManager.CRM.selectedCard.cardCraftResources[0] ||
                GameManager.UM.fertilizer < GameManager.CRM.selectedCard.cardCraftResources[1])
            {
                // If not enough resources, update the UI and prevent crafting
                if (GameManager.CRM.selectedCard.cardCraftResources[1] == 0)
                {
                    craftCardText.text = GameManager.CRM.selectedCard.cardCraftRequirementsText +
                                         GameManager.CRM.selectedCard.cardCraftResources[0] + " L";
                }
                else
                {
                    craftCardText.text = GameManager.CRM.selectedCard.cardCraftRequirementsText +
                                         GameManager.CRM.selectedCard.cardCraftResources[0] + " L and " +
                                         GameManager.CRM.selectedCard.cardCraftResources[1] + " L";
                }

                craftProgressBar.interactable = false;  // Disable slider interaction
            }
            else
            {
                craftCardText.text = "Hold to Craft";  // Update text for crafting
                isHoldingButton = true;
                craftProgressBar.gameObject.SetActive(true);

                // Stop the reset coroutine if it's running
                if (resetCoroutine != null)
                {
                    StopCoroutine(resetCoroutine);
                    resetCoroutine = null;
                }
            }
        }
    }

    // Triggered when the player releases the button
    public void OnPointerUp(PointerEventData eventData)
    {
        isHoldingButton = false;

        // Start resetting the progress smoothly if released early
        if (craftProgress < 1f)
        {
            resetCoroutine = StartCoroutine(ResetCraftingSmoothly());
        }
    }

    // Complete the crafting process
    private void CompleteCrafting()
    {
        isHoldingButton = false;
        craftProgress = 0f;
        craftProgressBar.value = 1f;
        progressBarFill.color = endColor;
        GameManager.DM.AddCardToDeck(GameManager.CRM.selectedCard.cardId);
        GameManager.UM.water -= GameManager.CRM.selectedCard.cardCraftResources[0];
        GameManager.UM.fertilizer -= GameManager.CRM.selectedCard.cardCraftResources[1];
    }

    // Smoothly reset the progress bar if released early
    private IEnumerator ResetCraftingSmoothly()
    {
        float resetDuration = 0.5f;
        float elapsedTime = 0f;
        Color initialColor = progressBarFill.color;

        while (elapsedTime < resetDuration)
        {
            elapsedTime += Time.deltaTime;
            craftProgressBar.value = Mathf.Lerp(craftProgress, 0f, elapsedTime / resetDuration);
            progressBarFill.color = Color.Lerp(initialColor, startColor, elapsedTime / resetDuration);
            yield return null;
        }
        craftProgress = 0f;
        craftProgressBar.value = 0f;
        progressBarFill.color = startColor;
    }
}
