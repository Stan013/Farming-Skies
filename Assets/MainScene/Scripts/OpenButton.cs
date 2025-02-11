using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OpenButton : MonoBehaviour
{
    [SerializeField] private Button openButton;
    [SerializeField] private GameObject openWindow;

    public void Start()
    {
        openButton.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        openWindow.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
