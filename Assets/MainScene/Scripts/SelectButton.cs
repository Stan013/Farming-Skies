using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour
{
    [SerializeField] private Button selectButton;
    [SerializeField] private GameObject selectWindow;

    void Start()
    {
        selectButton.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        selectWindow.SetActive(true);
    }
}
