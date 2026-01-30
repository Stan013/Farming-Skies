using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillManager : MonoBehaviour
{
    [Header("Refill variables")]
    public int refillAmount;

    [Header("Camera variables")]
    public Camera islandCamera;

    public IEnumerator RefillEvent()
    {
        IslandCameraMovement cameraMovement = islandCamera.GetComponent<IslandCameraMovement>();
        foreach (Island island in GameManager.ISM.boughtIslands)
        {
            while (!cameraMovement.islandID.Equals(island.islandID))
            {
                string direction = GetNextStepDirection(cameraMovement.islandID, island.islandID);
                if (direction != null)
                {
                    cameraMovement.MoveCamera(direction);
                }
                yield return null;
            }

            yield return StartCoroutine(island.RefillNutrients(refillAmount));

            yield return new WaitForSeconds(1.5f);
        }
    }

    private string GetNextStepDirection(string currentID, string targetID)
    {
        Vector2 currentPos = ParseIslandID(currentID);
        Vector2 targetPos = ParseIslandID(targetID);

        float dx = targetPos.x - currentPos.x;
        float dy = targetPos.y - currentPos.y;

        if (dx > 0) return "East";
        if (dx < 0) return "West";
        if (dy > 0) return "North";
        if (dy < 0) return "South";
        
        return null;
    }

    private Vector2 ParseIslandID(string id)
    {
        string cleaned = id.Trim('(', ')');
        string[] parts = cleaned.Split(',');
        int x = int.Parse(parts[0]);
        int y = int.Parse(parts[1]);
        return new Vector2(x, y);
    }
}
