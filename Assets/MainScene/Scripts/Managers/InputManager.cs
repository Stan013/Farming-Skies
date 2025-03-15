using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class InputManager : MonoBehaviour
{
    private int holdDuration = 2;
    private int inventoryHeight = 200;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float moveSpeed;
    private int zoomSpeed = 50;
    private int minFOV = 25;
    private int maxFOV = 125;
    public Camera mainCamera;
    public Rigidbody rb;

    private float verticalRotation = 0.0f;
    private Vector2 smoothMouseInput;
    private Vector3 smoothMoveDirection;
    public Island clickedIsland;
    private bool buildIslandPress = false;
    private float holdTimer = 0.0f;

    private bool isHoldingSpace = false;
    private float spaceHoldTimer = 0.0f;

    public bool defaultModeEnabled;
    public bool manageModeEnabled;
    public bool inventoryModeEnabled;
    public bool craftModeEnabled;
    public bool marketModeEnabled;
    public bool timeModeEnabled;
    public bool islandInspectEnabled;

    public void HandleGameStatesSwitchInput()
    {
        if(!GameManager.HM.dragging)
        {
            if (Input.GetKeyDown(KeyCode.Q) && manageModeEnabled) //No WASD movement and visible mouse
            {
                if (GameManager.CurrentState == GameManager.GameState.ManageMode && defaultModeEnabled)
                {
                    ToggleState(GameManager.GameState.Default, GameManager.GameState.ManageMode); //Switch back to default
                }
                else
                {
                    ToggleState(GameManager.GameState.ManageMode, GameManager.GameState.Default); //Switch to manage mode
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape)) //No WASD movement and only settings window
            {
                if (GameManager.CurrentState == GameManager.GameState.SettingsMode)
                {
                    ToggleState(GameManager.GameState.Default, GameManager.GameState.SettingsMode); //Switch back to default
                }
                else
                {
                    ToggleState(GameManager.GameState.SettingsMode, GameManager.GameState.Default); //Switch to settings mode
                }
            }
            if (Input.GetKeyDown(KeyCode.E) && inventoryModeEnabled) //No WASD movement and only inventory window
            {
                if (GameManager.CurrentState == GameManager.GameState.InventoryMode && defaultModeEnabled)
                {
                    ToggleState(GameManager.GameState.Default, GameManager.GameState.InventoryMode); //Switch back to default
                }
                else
                {
                    ToggleState(GameManager.GameState.InventoryMode, GameManager.GameState.Default); //Switch to inventory mode
                }
            }
            if (Input.GetKeyDown(KeyCode.R) && marketModeEnabled) //No WASD movement and only market window
            {
                if (GameManager.CurrentState == GameManager.GameState.MarketMode && defaultModeEnabled)
                {
                    ToggleState(GameManager.GameState.Default, GameManager.GameState.MarketMode); //Switch back to default
                }
                else
                {
                    ToggleState(GameManager.GameState.MarketMode, GameManager.GameState.Default); //Switch to market mode
                }
            }
            if (Input.GetKeyDown(KeyCode.C) && craftModeEnabled) //No WASD movement and only market window
            {
                if (GameManager.CurrentState == GameManager.GameState.CraftMode && defaultModeEnabled)
                {
                    ToggleState(GameManager.GameState.Default, GameManager.GameState.CraftMode); //Switch back to default
                }
                else
                {
                    ToggleState(GameManager.GameState.CraftMode, GameManager.GameState.Default); //Switch to craft mode
                }
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                GameManager.cheats.AddMoney();
            }
        }
        else
        {
            return; //Can't switch GameState when dragging a card
        }
    }

    public void ToggleState(GameManager.GameState targetState, GameManager.GameState fallbackState)
    {
        GameManager.SetState(GameManager.CurrentState == targetState ? fallbackState : targetState);
    }

    public void HandleKeyboardInput(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.Default:
                MovemementKeyboard();
                break;
            default:
                break;
        }
    }

    private void MovemementKeyboard()
    {
        Vector3 moveDirection = Vector3.zero;
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;
        Vector3 up = mainCamera.transform.up;

        moveDirection += (Input.GetKey(KeyCode.W) && !IsFacingWall(forward)) ? forward : Vector3.zero;
        moveDirection -= (Input.GetKey(KeyCode.S) && !IsFacingWall(-forward)) ? forward : Vector3.zero;
        moveDirection += (Input.GetKey(KeyCode.D) && !IsFacingWall(right)) ? right : Vector3.zero;
        moveDirection -= (Input.GetKey(KeyCode.A) && !IsFacingWall(-right)) ? right : Vector3.zero;
        moveDirection += (Input.GetKey(KeyCode.LeftShift) && !IsFacingWall(-up)) ? up : Vector3.zero;
        moveDirection -= (Input.GetKey(KeyCode.LeftControl) && !IsFacingWall(up)) ? up : Vector3.zero;

        smoothMoveDirection = Vector3.Lerp(smoothMoveDirection, moveDirection.normalized, Time.deltaTime * moveSpeed);
        rb.velocity = smoothMoveDirection * moveSpeed;
    }

    private void NextWeekInput()
    {
        if (Input.GetKey(KeyCode.Space) && timeModeEnabled)
        {
            spaceHoldTimer += Time.deltaTime;
            float fillValue = Mathf.Clamp01(spaceHoldTimer / 1f);
            GameManager.UM.nextWeekSlider.value = fillValue;
            RectTransform handleRect = GameManager.UM.nextWeekSlider.handleRect;
            float newX = Mathf.Lerp(25f, -35f, fillValue);
            handleRect.anchoredPosition = new Vector2(newX, handleRect.anchoredPosition.y);
            if (spaceHoldTimer >= 1f && !isHoldingSpace)
            {
                isHoldingSpace = true;
                ToggleState(GameManager.GameState.TimeMode, GameManager.GameState.Default);
            }
        }
        else
        {
            if(spaceHoldTimer != 0f)
            {
                spaceHoldTimer = 0f;
                isHoldingSpace = false;
                GameManager.UM.nextWeekSlider.value = 0f;
                RectTransform handleRect = GameManager.UM.nextWeekSlider.handleRect;
                handleRect.anchoredPosition = new Vector2(25f, handleRect.anchoredPosition.y);
            }
        }
    }



    public void HandleMouseInput(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.SettingsMode:
                break;
            case GameManager.GameState.Default:
                MovementMouse();
                MouseZoom();
                NextWeekInput();
                break;
            case GameManager.GameState.ManageMode:
                DefaultMouse();
                MouseZoom();
                NextWeekInput();
                break;
            case GameManager.GameState.InventoryMode:
                break;
            case GameManager.GameState.MarketMode:
                break;
            case GameManager.GameState.CraftMode:
                break;
            case GameManager.GameState.SelectionMode:
                break;
        }
    }

    private void DefaultMouse()
    {
        if (Input.GetMouseButtonDown(1) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit) //Hold to build island
            && hit.transform.GetComponent<Island>() != null)
        {
            if(GameManager.HM.cardsInHand.Count != 0 && Input.mousePosition.y < inventoryHeight)
            {
                return;
            }
            clickedIsland = GameManager.ISM.GetClickedIsland();
            if (GameManager.UM.money >= clickedIsland.islandBuildCost && clickedIsland.islandBought == false && clickedIsland.islandAvailable)
            {
                if (!GameManager.TTM.tutorial || clickedIsland.currentState == Island.IslandState.Highlighted)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    GameManager.UM.constructionLabel.gameObject.SetActive(true);
                    buildIslandPress = true;
                    GameManager.UM.SetBuildIslandSlider();
                }
            }
        }

        if (!Input.GetMouseButton(1)) //Unsuccesfull island build
        {
            Cursor.lockState = CursorLockMode.None;
            buildIslandPress = false;
            holdTimer = 0f;
            if (GameManager.UM.constructionLabel.gameObject.activeSelf)
            {
                GameManager.UM.constructionLabel.gameObject.SetActive(false);
                clickedIsland.ResetMaterials();
            }
        }

        if (buildIslandPress)
        {
            holdTimer = Mathf.Min(holdTimer + Time.deltaTime, holdDuration);
            float alphaChangeSpeed = 1.0f / holdDuration;
            GameManager.UM.UpdateBuildIslandSlider(clickedIsland);
            clickedIsland.ChangeMaterial(alphaChangeSpeed);
        }

        if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit2) //Click to inspect island
            && hit2.transform.GetComponent<Island>() != null && islandInspectEnabled)
        {
            Island hitIsland = hit2.transform.GetComponent<Island>();
            if(hitIsland.islandBought)
            {
                GameManager.ISM.islandMenu.SetActive(true);
                GameManager.ISM.islandMenu.GetComponent<IslandInfoUI>().SetupIslandInfo(hitIsland);
            }
        }
    }

    private void MovementMouse()
    {
        Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        smoothMouseInput = Vector2.Lerp(smoothMouseInput, mouseInput, Time.deltaTime * mouseSensitivity);

        float horizontalRotation = smoothMouseInput.x * mouseSensitivity;
        mainCamera.transform.Rotate(Vector3.up * horizontalRotation, Space.World);

        verticalRotation -= smoothMouseInput.y * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        mainCamera.transform.localEulerAngles = new Vector3(verticalRotation, mainCamera.transform.localEulerAngles.y, 0f);
    }

    private void MouseZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        mainCamera.fieldOfView -= scroll * zoomSpeed;
        mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, minFOV, maxFOV);
    }

    private bool IsFacingWall(Vector3 direction)
    {
        return Physics.Raycast(transform.position, direction, 1f);
    }
}