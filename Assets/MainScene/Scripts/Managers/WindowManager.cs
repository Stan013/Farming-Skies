using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [Header("Game windows")]
    public GameObject mainWindow;
    public GameObject gameWindow;
    public GameObject tutorialWindow;
    public GameObject questWindow;
    public GameObject advanceWindow;
    public GameObject inventoryWindow;
    public GameObject manageWindow;
    public GameObject craftWindow;
    public GameObject expenseWindow;

    [Header("Quest window variables")]
    public float windowTransitionDuration = 0.5f;

    public void OpenQuestWindow()
    {
        if (questWindow == null) return;

        RectTransform rect = questWindow.GetComponent<RectTransform>();
        questWindow.SetActive(true);
        StartCoroutine(MoveDown(rect, 415f));
    }

    private IEnumerator MoveDown(RectTransform window, float targetY)
    {
        float startY = window.anchoredPosition.y;
        float elapsedTime = 0f;

        while (elapsedTime < windowTransitionDuration)
        {
            float newY = Mathf.Lerp(startY, targetY, elapsedTime / windowTransitionDuration);
            window.anchoredPosition = new Vector2(window.anchoredPosition.x, newY);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        window.anchoredPosition = new Vector2(window.anchoredPosition.x, targetY);
    }
}
