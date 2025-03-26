using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resource : MonoBehaviour
{
    public bool moving;
    public Vector3 targetPosition;
    public Vector3 startPosition;
    public Image resourceImage;
    public float elapsedTime;
    public Color startColor;
    public float speed;

    public void Start()
    {
        startPosition = transform.position;
        startColor = resourceImage.color;
        elapsedTime = 0f;
        speed = 20f / (GameManager.CRM.biggestResourceCost * GameManager.CRM.craftSpeed);
    }

    public void Update()
    {
        if (moving)
        {
            if (transform.position != targetPosition)
            {
                elapsedTime += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime);
                float alpha = Mathf.Clamp01(1f - elapsedTime);
                resourceImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            }
            else
            {
                transform.position = targetPosition;
                resourceImage.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
                //GameManager.CRM.craftUI.craftResources.Remove(GetComponent<Resource>());
                Destroy(gameObject);
            }
        }
    }
}
