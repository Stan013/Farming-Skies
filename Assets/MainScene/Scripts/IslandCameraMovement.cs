using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandCameraMovement : MonoBehaviour
{
    public Camera islandCam;
    public Vector2 islandPosition = Vector2.zero;
    public string islandID;

    private Dictionary<string, Vector2> directionMap = new Dictionary<string, Vector2>
    {
        { "North", new Vector2(0, 1) },
        { "East", new Vector2(1, 0) },
        { "South", new Vector2(0, -1) },
        { "West", new Vector2(-1, 0) }
    };

    private Dictionary<string, Vector3> cameraOffset = new Dictionary<string, Vector3>
    {
        { "North", new Vector3(0, 0, 8) },
        { "East", new Vector3(8, 0, 0) },
        { "South", new Vector3(0, 0, -8) },
        { "West", new Vector3(-8, 0, 0) }
    };

    public void MoveCamera(string direction)
    {
        if (!directionMap.ContainsKey(direction)) return;

        islandPosition += directionMap[direction];
        UpdateIslandID();

        if (GameManager.ISM.centerIsland.islandBought)
        {
            MoveCameraPosition(direction);
        }
        else
        {
            islandPosition -= directionMap[direction];
            UpdateIslandID();
        }
    }

    private void UpdateIslandID()
    {
        islandID = $"({(int)islandPosition.x},{(int)islandPosition.y})";
        GameManager.ISM.centerIsland = GameManager.ISM.FindIslandByID(islandID);
    }

    private void MoveCameraPosition(string direction)
    {
        islandCam.transform.position += cameraOffset[direction];
    }
}
