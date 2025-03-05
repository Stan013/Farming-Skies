using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    public bool hasPressed;

    public void SetPressed()
    {
        if(!hasPressed)
        {
            hasPressed = true;
            GetComponent<Image>().color = Color.white;
        }
    }
}
