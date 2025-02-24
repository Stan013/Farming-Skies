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

    public float doubleClickTime = 0.5f;
    private float lastClickTime;
    public UnityEvent onDoubleClick;

    public void HandleGameStatesSwitchInput()
    {
        if(!GameManager.HM.dragging)
        {
            if (Input.GetKeyDown(KeyCode.Tab)) //No WASD movement and visible mouse
            {
                if (GameManager.CurrentState == GameManager.GameState.ManageMode)
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
            if (Input.GetKeyDown(KeyCode.E) && GameManager.UM.openInventoryButton.IsInteractable()) //No WASD movement and only inventory window
            {
                if (GameManager.CurrentState == GameManager.GameState.InventoryMode)
                {
                    ToggleState(GameManager.GameState.Default, GameManager.GameState.InventoryMode); //Switch back to default
                }
                else
                {
                    ToggleState(GameManager.GameState.InventoryMode, GameManager.GameState.Default); //Switch to inventory mode
                }
            }
            if (Input.GetKeyDown(KeyCode.Q) && GameManager.UM.openMarketButton.IsInteractable()) //No WASD movement and only market window
            {
                if (GameManager.CurrentState == GameManager.GameState.MarketMode)
                {
                    ToggleState(GameManager.GameState.Default, GameManager.GameState.MarketMode); //Switch back to default
                }
                else
                {
                    ToggleState(GameManager.GameState.MarketMode, GameManager.GameState.Default); //Switch to market mode
                }
            }
            if (Input.GetKeyDown(KeyCode.C) && GameManager.UM.openCraftButton.IsInteractable()) //No WASD movement and only market window
            {
                if (GameManager.CurrentState == GameManager.GameState.CraftMode)
                {
                    ToggleState(GameManager.GameState.Default, GameManager.GameState.CraftMode); //Switch back to default
                }
                else
                {
                    ToggleState(GameManager.GameState.CraftMode, GameManager.GameState.Default); //Switch to craft mode
                }
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

    public void HandleMouseInput(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.SettingsMode:
                break;
            case GameManager.GameState.Default:
                MovementMouse();
                MouseZoom();
                break;
            case GameManager.GameState.ManageMode:
                DefaultMouse();
                MouseZoom();
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
            && hit.transform.GetComponent<Island>() != null && Input.mousePosition.y >= inventoryHeight)
        {
            clickedIsland = GameManager.ISM.GetClickedIsland();
            if (GameManager.UM.balance >= clickedIsland.islandBuildCost && clickedIsland.islandBought == false && clickedIsland.islandAvailable)
            {
                if (!GameManager.TTM.tutorial || clickedIsland.name == "0,0")
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    GameManager.UM.constructionLabel.gameObject.SetActive(true);
                    buildIslandPress = true;
                    GameManager.UM.SetBuildIslandSlider();
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hitIsland) //Island nutrients inspection
           && hitIsland.transform.GetComponent<Island>() != null && Input.mousePosition.y >= inventoryHeight)
        {
            if (hitIsland.transform.GetComponent<Island>().islandBought)
            {
                if (Time.time - lastClickTime < doubleClickTime)
                {
                    if(GameManager.TTM.tutorial)
                    {
                        if(GameManager.TTM.tutorialCount > 9)
                        {
                            MoveCameraToIsland(hitIsland.transform.GetComponent<Island>());
                        }
                    }
                    else
                    {
                        MoveCameraToIsland(hitIsland.transform.GetComponent<Island>());
                    }
                }
                lastClickTime = Time.time;
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
                if (!GameManager.TTM.tutorial)
                {
                    clickedIsland.ToggleState(Island.IslandState.Default, Island.IslandState.Default);
                }
                else
                {
                    clickedIsland.topMat.SetColor("_EmissionColor", Color.blue * 2.0f);
                    clickedIsland.bottomMat.SetColor("_EmissionColor", Color.blue * 2.0f);
                }
            }
        }

        if (buildIslandPress)
        {
            holdTimer = Mathf.Min(holdTimer + Time.deltaTime, holdDuration);
            float alphaChangeSpeed = 1.0f / holdDuration;
            GameManager.UM.UpdateBuildIslandSlider(clickedIsland);
            clickedIsland.ChangeMaterial(alphaChangeSpeed);
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

    private void MoveCameraToIsland(Island island)
    {
        if (GameManager.TTM.tutorialCount == 10 || GameManager.TTM.tutorialCount == 16)
        {
            GameManager.TTM.QuestCompleted = true;
        }

        Vector3 targetPosition = new Vector3(island.transform.position.x + 4, 5, island.transform.position.z + 4);
        Quaternion targetRotation = Quaternion.Euler(5, -135, 0);

        StartCoroutine(SmoothMoveAndRotate(mainCamera.transform, targetPosition, targetRotation, 5f));
    }

    private IEnumerator SmoothMoveAndRotate(Transform cameraTransform, Vector3 targetPos, Quaternion targetRot, float speed)
    {
        Vector3 startPos = cameraTransform.position;
        Quaternion startRot = cameraTransform.rotation;

        float journey = 0f;
        float duration = Vector3.Distance(startPos, targetPos) / speed;

        while (journey < duration)
        {
            journey += Time.deltaTime;
            float t = journey / duration;

            cameraTransform.position = Vector3.Lerp(startPos, targetPos, t);
            cameraTransform.rotation = Quaternion.Lerp(startRot, targetRot, t);

            yield return null;
        }

        cameraTransform.position = targetPos;
        cameraTransform.rotation = targetRot;
    }

    private bool IsFacingWall(Vector3 direction)
    {
        return Physics.Raycast(transform.position, direction, 1f);
    }
}