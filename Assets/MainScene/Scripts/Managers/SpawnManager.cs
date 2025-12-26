using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class SpawnManager : MonoBehaviour
{

   [Header("Start settings")]
   public float startBalance = 100f;
   public int startWater = 50;
   public int startFertiliser = 50;


    public void Spawn()
    {
        GameManager.WM.inMenu = false;
        GameManager.IPM.cam.transform.position = new Vector3(0f, 10f, 0f);
        GameManager.IPM.cam.transform.rotation = Quaternion.Euler(45f, 0f, 0f);
        GameManager.UM.resourceUI.SetActive(true);

        if (!GameManager.DBM.skipTutorial)
        {
            GameManager.WM.tutorialWindow.SetActive(true);
        }
        else
        {
            GameManager.QM.questActive = false;
            GameManager.UM.SetupUI();
            GameManager.IPM.nextWeekEnabled = true;
        }
    }
}
