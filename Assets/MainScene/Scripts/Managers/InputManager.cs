using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour
{
    [Header("Player variables")]
    public Vector3 startingPos;
    public Quaternion startingRot;
    public Rigidbody rb;
    public Camera cam;

    [Header("Mouse control variables")]
    private static int mouseSensitivity = 2;
    private static int moveSpeed = 10;
    private static int zoomSpeed = 50;
    private static int minFOV = 25;
    private static int maxFOV = 125;

    [Header("Keyboard control variables")]
    private Vector3 smoothMoveDirection;

    [Header("Island build variables")]
    private static int holdDuration = 2;
    private static int inventoryHeight = 200;
    private bool buildIslandPress = false;
    private float holdTimer = 0.0f;
    public Island potentialIsland;

    [Header("Next week variables")]
    private bool isHoldingSpace = false;
    private float spaceHoldTimer = 0.0f;

    public void KeyboardInput()
    {
        NextWeekInput();
        Vector3 moveDirection = Vector3.zero;

        moveDirection += (Input.GetKey(KeyCode.W) && !IsFacingWall(Vector3.forward)) ? Vector3.forward : Vector3.zero;
        moveDirection -= (Input.GetKey(KeyCode.S) && !IsFacingWall(Vector3.back)) ? Vector3.forward : Vector3.zero;
        moveDirection += (Input.GetKey(KeyCode.D) && !IsFacingWall(Vector3.right)) ? Vector3.right : Vector3.zero;
        moveDirection -= (Input.GetKey(KeyCode.A) && !IsFacingWall(Vector3.left)) ? Vector3.right : Vector3.zero;
        moveDirection += (Input.GetKey(KeyCode.LeftShift) && !IsFacingWall(Vector3.up)) ? Vector3.up : Vector3.zero;
        moveDirection -= (Input.GetKey(KeyCode.LeftControl) && !IsFacingWall(Vector3.down)) ? Vector3.up : Vector3.zero;

        smoothMoveDirection = Vector3.Lerp(smoothMoveDirection, moveDirection.normalized, Time.deltaTime * moveSpeed);
        rb.velocity = smoothMoveDirection * moveSpeed;
    }


    private void NextWeekInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            spaceHoldTimer += Time.deltaTime;
            float fillValue = Mathf.Clamp01(spaceHoldTimer / 1f);
            GameManager.TM.advanceWeekImage.fillAmount = fillValue;
            if (spaceHoldTimer >= 1f && !isHoldingSpace)
            {
                GameManager.WM.advanceWindow.SetActive(true);
                GameManager.TM.AdvanceNextWeek();
            }
        }
        else
        {
            if (spaceHoldTimer != 0f)
            {
                spaceHoldTimer = 0f;
                GameManager.TM.advanceWeekImage.fillAmount = 0f;
            }
        }
    }

    public void MouseInput()
    {
        if (Input.GetMouseButtonDown(1) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit) //Hold to build island
            && hit.transform.GetComponent<Island>() != null)
        {
            if(GameManager.HM.cardsInHand.Count != 0 && Input.mousePosition.y < inventoryHeight)
            {
                return;
            }
            potentialIsland = GameManager.ISM.GetPotentialIsland();
            if (GameManager.UM.balance >= potentialIsland.islandBuildCost)
            {
                if (!GameManager.QM.questActive || potentialIsland.currentState == Island.IslandState.Highlighted)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    GameManager.UM.constructionLabel.gameObject.SetActive(true);
                    buildIslandPress = true;
                    GameManager.UM.SetBuildIslandSlider();
                }
            }
        }

        if (!Input.GetMouseButton(1)) //Unsuccesful island build
        {
            Cursor.lockState = CursorLockMode.None;
            buildIslandPress = false;
            holdTimer = 0f;
            if (GameManager.UM.constructionLabel.gameObject.activeSelf)
            {
                GameManager.UM.constructionLabel.gameObject.SetActive(false);
                potentialIsland.SetIslandState(Island.IslandState.Transparent);
            }
        }

        if (buildIslandPress) //Successful island build
        {
            holdTimer = Mathf.Min(holdTimer + Time.deltaTime, holdDuration);
            float alphaChangeSpeed = 1.0f / holdDuration;
            GameManager.UM.UpdateBuildIslandSlider(potentialIsland);
            potentialIsland.UpdateMaterialAlpha(alphaChangeSpeed);
        }

        if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit2) // Click to inspect island
            && hit2.transform.GetComponent<Island>() != null)
        {
            Island hitIsland = hit2.transform.GetComponent<Island>();
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel"); // Zooming
        cam.fieldOfView -= scroll * zoomSpeed;
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minFOV, maxFOV);
    }

    private bool IsFacingWall(Vector3 direction)
    {
        return Physics.Raycast(transform.position, direction, 1f);
    }
}