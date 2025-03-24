using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandCameraMovement : MonoBehaviour
{
    public Camera islandCam;
    public Vector2 islandPosition = new Vector2(0,0);
    public string islandID;

    public void MoveCamera(string direction)
    {
        switch (direction)
        {
            case "North":
                islandPosition.y++;
                islandID = $"({(int)islandPosition.x},{(int)islandPosition.y})";
                GameManager.ISM.centerIsland = GameManager.ISM.FindIslandByID(islandID);
                if (GameManager.ISM.centerIsland.islandBought)
                {
                    islandCam.transform.position = new Vector3(islandCam.transform.position.x, islandCam.transform.position.y, islandCam.transform.position.z + 8);
                }
                else
                {
                    islandPosition.y--;
                }
                break;
            case "East":
                islandPosition.x++;
                islandID = $"({(int)islandPosition.x},{(int)islandPosition.y})";
                GameManager.ISM.centerIsland = GameManager.ISM.FindIslandByID(islandID);
                if (GameManager.ISM.centerIsland.islandBought)
                {
                    islandCam.transform.position = new Vector3(islandCam.transform.position.x + 8, islandCam.transform.position.y, islandCam.transform.position.z);
                }
                else
                {
                    islandPosition.x--;
                }
                break;
            case "South":
                islandPosition.y--;
                islandID = $"({(int)islandPosition.x},{(int)islandPosition.y})";
                GameManager.ISM.centerIsland = GameManager.ISM.FindIslandByID(islandID);
                if (GameManager.ISM.centerIsland.islandBought)
                {
                    islandCam.transform.position = new Vector3(islandCam.transform.position.x, islandCam.transform.position.y, islandCam.transform.position.z - 8);
                }
                else
                {
                    islandPosition.y++;
                }
                break;
            case "West":
                islandPosition.x--;
                islandID = $"({(int)islandPosition.x},{(int)islandPosition.y})";
                GameManager.ISM.centerIsland = GameManager.ISM.FindIslandByID(islandID);
                if (GameManager.ISM.centerIsland.islandBought)
                {
                    islandCam.transform.position = new Vector3(islandCam.transform.position.x - 8, islandCam.transform.position.y, islandCam.transform.position.z);
                }
                else
                {
                    islandPosition.x++;
                }
                break;
        }
    }
}
