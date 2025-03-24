using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    [Header("Time variables")]
    public Image advanceWeekImage;
    public TMP_Text weekDayText;

    [Header("Week cycle variables")]
    public List<string> weekDays = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
    public int currentDayOfWeek = 0;

    public void AdvanceNextWeek()
    {
        GameManager.IPM.startingPos = GameManager.IPM.cam.transform.position;
        GameManager.IPM.cam.transform.position = new Vector3(-25f, 25f, -25f);
        GameManager.IPM.cam.transform.rotation = Quaternion.Euler(32f,45f,0f);
        GameManager.PM.Harvest();
        StartCoroutine(CycleWeekDays());
    }

    private IEnumerator CycleWeekDays()
    {
        SlideTextTransition();
        for (int i = 0; i < weekDays.Count; i++)
        {
            currentDayOfWeek++;

            if (currentDayOfWeek >= weekDays.Count)
            {
                currentDayOfWeek = 0;
            }

            weekDayText.text = weekDays[currentDayOfWeek];
            yield return new WaitForSeconds(0.75f);
        }
        GameManager.UM.weeks++;
        GameManager.WM.advanceWindow.SetActive(false);
        GameManager.UM.UpdateUI();
        GameManager.IPM.cam.transform.position = GameManager.IPM.startingPos;
        GameManager.IPM.cam.transform.rotation = Quaternion.Euler(45f, 0f, 0f);
    }

    private IEnumerator SlideTextTransition()
    {
        RectTransform rectTransform = weekDayText.GetComponent<RectTransform>();
        Vector3 initialPosition = rectTransform.localPosition;
        Vector3 offScreenLeft = new Vector3(-Screen.width, initialPosition.y, initialPosition.z);
        Vector3 offScreenRight = new Vector3(Screen.width, initialPosition.y, initialPosition.z);

        float slideDuration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < slideDuration)
        {
            rectTransform.localPosition = Vector3.Lerp(initialPosition, offScreenLeft, elapsedTime / slideDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.localPosition = offScreenLeft;
        weekDayText.text = weekDays[currentDayOfWeek];
        elapsedTime = 0f;
        rectTransform.localPosition = offScreenRight;

        while (elapsedTime < slideDuration)
        {
            rectTransform.localPosition = Vector3.Lerp(offScreenRight, initialPosition, elapsedTime / slideDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        rectTransform.localPosition = initialPosition;
    }

    public void RotateSky(float skyRotationSpeed)
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * skyRotationSpeed);
    }
}
