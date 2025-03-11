using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject objButton;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(objButton.transform.parent.name == "HomeScreen")
        {
            objButton.transform.localPosition = new Vector3(objButton.transform.localPosition.x + 200, objButton.transform.localPosition.y, objButton.transform.localPosition.z);
        }
        else
        {
            objButton.transform.localPosition = new Vector3(objButton.transform.localPosition.x, objButton.transform.localPosition.y + 150, objButton.transform.localPosition.z);

        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (objButton.transform.parent.name == "HomeScreen")
        {
            objButton.transform.localPosition = new Vector3(objButton.transform.localPosition.x - 200, objButton.transform.localPosition.y, objButton.transform.localPosition.z);
        }
        else
        {
            objButton.transform.localPosition = new Vector3(objButton.transform.localPosition.x, objButton.transform.localPosition.y - 150, objButton.transform.localPosition.z);

        }
    }
}
