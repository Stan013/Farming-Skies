using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    void Start()
    {
        closeButton.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        GameManager.IPM.ToggleState(GameManager.GameState.Default, GameManager.GameState.Default);
    }

    public void CloseWindow(GameObject window)
    {
        window.SetActive(false);
    }
}
