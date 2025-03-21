using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    private float centerDuration = 8f;
    private float lookAroundDuration = 8f;
    private float dropDuration = 2f;

    public void Spawn()
    {
        GameManager.WM.mainWindow.SetActive(false);
        GameManager.WM.gameWindow.SetActive(true);
        GameManager.UM.levelUI.SetActive(true);
        GameManager.UM.timeUI.SetActive(true);
        if(GameManager.DBM.skipIntro)
        {
            GameManager.IPM.cam.transform.position = new Vector3(0f,10f,0f);
            GameManager.IPM.cam.transform.rotation = Quaternion.Euler(45f, 0f, 0f);
            GameManager.UM.resourceUI.SetActive(true);
            GameManager.WM.tutorialWindow.SetActive(true);
            return;
        }
        StartCoroutine(SpawnSequence());
    }

    public IEnumerator SpawnSequence()
    {
        yield return StartCoroutine(CenterTransition()); // Move camera to center

        yield return StartCoroutine(LookAroundAnimation()); // Make camera look around

        yield return StartCoroutine(DropAnimation()); //Drop camera

        yield return new WaitForSeconds(1f);

        GameManager.IPM.startingPos = GameManager.IPM.cam.transform.position;
        GameManager.IPM.startingRot = GameManager.IPM.cam.transform.rotation;
        GameManager.UM.resourceUI.SetActive(true);
        GameManager.WM.tutorialWindow.SetActive(true);
    }

    private IEnumerator CenterTransition()
    {
        Vector3 startPos = GameManager.IPM.cam.transform.position;
        Vector3 targetPos = new Vector3(0f, 25f, 0f);

        Quaternion startRot = Quaternion.Euler(32f, 45f, 0f); // Look straight
        Quaternion leftRot = Quaternion.Euler(32f, -45f, 0f); // Look left
        Quaternion rightRot = Quaternion.Euler(32f, 135f, 0f); // Look right

        float elapsedTime = 0f;

        while (elapsedTime < centerDuration)
        {
            float t = elapsedTime / centerDuration;
            GameManager.IPM.cam.transform.position = Vector3.Lerp(startPos, targetPos, t);
            GameManager.IPM.cam.transform.rotation = startRot;

            if (t < 0.25f)
            {
                GameManager.IPM.cam.transform.rotation = Quaternion.Lerp(startRot, leftRot, t / 0.25f);
            }
            else if (t < 0.5f)
            {
                GameManager.IPM.cam.transform.rotation = Quaternion.Lerp(leftRot, startRot, (t - 0.25f) / 0.25f);
            }
            else if (t < 0.75f)
            {
                GameManager.IPM.cam.transform.rotation = Quaternion.Lerp(startRot, rightRot, (t - 0.5f) / 0.25f);
            }
            else
            {
                GameManager.IPM.cam.transform.rotation = Quaternion.Lerp(rightRot, startRot, (t - 0.75f) / 0.25f);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        GameManager.IPM.cam.transform.position = targetPos; // Ensure it reaches exactly
    }

    private IEnumerator LookAroundAnimation()
    {
        Quaternion startRot = GameManager.IPM.cam.transform.rotation;
        Quaternion upRot = Quaternion.Euler(-45f, 45f, 0f); // Look up
        Quaternion upLeftRot = Quaternion.Euler(-45f, -45f, 0f); // Look up left
        Quaternion upRightRot = Quaternion.Euler(-45f, 135f, 0f); // Look up right

        float elapsedTime = 0f;

        while (elapsedTime < lookAroundDuration)
        {
            float t = elapsedTime / lookAroundDuration;

            if (t < 0.1666f)
            {
                GameManager.IPM.cam.transform.rotation = Quaternion.Lerp(startRot, upRot, t / 0.1666f);
            }
            else if (t < 0.3332f)
            {
                GameManager.IPM.cam.transform.rotation = Quaternion.Lerp(upRot, upLeftRot, (t - 0.1666f) / 0.1666f);
            }
            else if (t < 0.4998f)
            {
                GameManager.IPM.cam.transform.rotation = Quaternion.Lerp(upLeftRot, upRot, (t - 0.3332f) / 0.1666f);
            }
            else if (t < 0.6664f)
            {
                GameManager.IPM.cam.transform.rotation = Quaternion.Lerp(upRot, upRightRot, (t - 0.4998f) / 0.1666f);
            }
            else if (t < 0.8330f)
            {
                GameManager.IPM.cam.transform.rotation = Quaternion.Lerp(upRightRot, upRot, (t - 0.6664f) / 0.1666f);
            }
            else
            {
                GameManager.IPM.cam.transform.rotation = Quaternion.Lerp(upRot, startRot, (t - 0.8330f) / 0.1666f);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        GameManager.IPM.cam.transform.rotation = startRot; // Ensure exact rotation
    }

    private IEnumerator DropAnimation()
    {
        Vector3 startPos = GameManager.IPM.cam.transform.position;
        Vector3 targetPos = new Vector3(0f, 10f, 0f);
        Quaternion startRot = GameManager.IPM.cam.transform.rotation;

        float elapsedTime = 0f;

        while (elapsedTime < dropDuration)
        {
            float t = elapsedTime / dropDuration;
            GameManager.IPM.cam.transform.position = Vector3.Lerp(startPos, targetPos, t);
            float spinAngle = Mathf.Lerp(0f, 675f, t); // Spin 315 degrees
            GameManager.IPM.cam.transform.rotation = Quaternion.Euler(startRot.eulerAngles.x, startRot.eulerAngles.y + spinAngle, startRot.eulerAngles.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position and rotation are exact
        GameManager.IPM.cam.transform.position = targetPos;
        GameManager.IPM.cam.transform.rotation = Quaternion.Euler(45f, startRot.eulerAngles.y + 675f, startRot.eulerAngles.z);
    }

}
