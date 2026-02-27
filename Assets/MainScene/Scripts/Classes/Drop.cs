using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Drop : MonoBehaviour
{
    public string dropType;
    private int moveSpeed = 15;
    private int rotationSpeed = 500;

    public void AddDropToInventory(InventoryItem attachedInventoryItem)
    {
        switch (dropType)
        {
            case "Buildable":
                if (name == "WaterDrop")
                {
                    GameManager.UM.Water += 1;
                }
                if (name == "fertiliserDrop")
                {
                    GameManager.UM.Fertiliser += 1;
                }
                break;
            case "Product":
                break;
            case "Plant":
                attachedInventoryItem.ItemQuantity += 3;
                break;
        }
        StartCoroutine(MoveDropCoroutine());
    }

    private IEnumerator MoveDropCoroutine()
    {
        Vector3 targetPosition = new Vector3(transform.position.x, 25f, transform.position.z);

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
            yield return null;
        }
        if (transform.position.y > 20f)
        {
            Destroy(gameObject);
        }
    }
}
