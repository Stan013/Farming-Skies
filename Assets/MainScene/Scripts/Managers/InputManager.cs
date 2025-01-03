using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputManager : MonoBehaviour
{
    private float holdDuration = 2f;
    private float inventoryHeight = 200f;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float moveSpeed;
    private float zoomSpeed = 50;
    private float minFOV = 50f;
    private float maxFOV = 100f;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Rigidbody rb;

    private float verticalRotation = 0f;
    private Vector2 smoothMouseInput;
    private Vector3 smoothMoveDirection;
    private Island m_ClickedIsland;
    public Island clickedIsland { get { return m_ClickedIsland; } set { m_ClickedIsland = value; } }
    private bool buildIslandPress = false;
    private float holdTimer = 0.0f;

    public void HandleGameStatesSwitchInput()
    {
        if (Input.GetKeyDown(KeyCode.B)) // No movement only mouse clicking
        {
            ToggleState(GameManager.GameState.BuildMode, GameManager.GameState.Default);
        }
        if (Input.GetKeyDown(KeyCode.M)) // Limited movement and mouse clicking
        {
            ToggleState(GameManager.GameState.ManageMode, GameManager.GameState.Default);
        }
        if (Input.GetKeyDown(KeyCode.E)) // Payment and card picking (no building or dragging)
        {
            ToggleState(GameManager.GameState.EndRoundMode, GameManager.GameState.Default);
        }
        if (Input.GetKeyDown(KeyCode.H)) // Open Shop (no building or dragging)
        {
            ToggleState(GameManager.GameState.ShopMode, GameManager.GameState.Default);
        }
        if (Input.GetKeyDown(KeyCode.I)) // Open Inventory (no building, dragging or cards)
        {
            if(GameManager.CurrentState == GameManager.GameState.InventoryMode)
            {
                ToggleState(GameManager.GameState.Default, GameManager.GameState.InventoryMode);
            }
            else
            {
                ToggleState(GameManager.GameState.InventoryMode, GameManager.GameState.Default);
            }
        }
        if (Input.GetKeyDown(KeyCode.M)) // Open Market (no building, dragging or cards)
        {
            if (GameManager.CurrentState == GameManager.GameState.MarketMode)
            {
                ToggleState(GameManager.GameState.Default, GameManager.GameState.MarketMode);
            }
            else
            {
                ToggleState(GameManager.GameState.MarketMode, GameManager.GameState.Default);
            }
        }
    }

    public void ToggleState(GameManager.GameState targetState, GameManager.GameState fallbackState)
    {
        GameManager.SetState(GameManager.CurrentState == targetState ? fallbackState : targetState);
    }

    public void HandleKeyboard(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.Default:
                DefaultKeyboard();
                break;
            case GameManager.GameState.ManageMode:
                break;
            case GameManager.GameState.BuildMode:
                mainCamera.transform.position = GameManager.staticPos;
                break;
            case GameManager.GameState.EndRoundMode:
                mainCamera.transform.position = GameManager.staticPos;
                break;
            case GameManager.GameState.InventoryMode:
                mainCamera.transform.position = GameManager.staticPos;
                break;
            case GameManager.GameState.MarketMode:
                mainCamera.transform.position = GameManager.staticPos;
                break;
        }
    }

    private void DefaultKeyboard()
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

    public void HandleMouse(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.Default:
                DefaultMouse();
                MouseZoom();
                break;
            case GameManager.GameState.ManageMode:
                break;
            case GameManager.GameState.BuildMode:
                BuildMouse();
                break;
            case GameManager.GameState.EndRoundMode:
                break;
            case GameManager.GameState.InventoryMode:
                break;
            case GameManager.GameState.MarketMode:
                break;
        }
    }

    private void DefaultMouse()
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

    private void BuildMouse()
    {
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit)
            && hit.transform.GetComponent<Island>() != null && Input.mousePosition.y >= inventoryHeight)
        {
            clickedIsland = GetClickedIsland();
            if (GameManager.UM.balance >= clickedIsland.islandBuildCost && clickedIsland.islandBoughtStatus == false && clickedIsland.islandCanBought)
            {
                Cursor.lockState = CursorLockMode.Locked;
                GameManager.UM.constructionLabel.gameObject.SetActive(true);
                buildIslandPress = true;
                GameManager.UM.SetBuildIslandSlider();
            }
        }

        if (!Input.GetMouseButton(0))
        {
            Cursor.lockState = CursorLockMode.None;
            buildIslandPress = false;
            holdTimer = 0f;
            if (GameManager.UM.constructionLabel.gameObject.activeSelf)
            {
                GameManager.UM.constructionLabel.gameObject.SetActive(false);
                clickedIsland.ToggleState(Island.IslandState.Default, Island.IslandState.Default);
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

    private bool IsFacingWall(Vector3 direction)
    {
        return Physics.Raycast(transform.position, direction, 1f);
    }

    private Island GetClickedIsland()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out var hit) ? hit.collider.gameObject.GetComponent<Island>() : null;
    }
}
