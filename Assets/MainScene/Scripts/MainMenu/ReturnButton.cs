using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReturnButton : MonoBehaviour
{
    [SerializeField] private Button returnButton;
    [SerializeField] private GameObject gameModeWindow;

    void Start()
    {
        returnButton.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        gameModeWindow.SetActive(false);
    }
}
