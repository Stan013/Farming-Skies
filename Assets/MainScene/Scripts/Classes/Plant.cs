using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public Drop drop;
    public float plantScale = 0.005f;
    public string plantCardID;

    public Island attachedIsland;
    public int yield;
    public List<int> nutrientsUsages;
    public int buildableTaxCost;

    public void GiveDrop(Transform plot)
    {
        Drop plantDrop = Instantiate(drop, Vector3.zero, Quaternion.identity);
        plantDrop.transform.localScale = new Vector3(plantDrop.transform.localScale.x * plantScale, plantDrop.transform.localScale.y * plantScale, plantDrop.transform.localScale.z * plantScale);
        plantDrop.transform.localPosition = new Vector3(plot.position.x, 5f, plot.position.z);
        plantDrop.dropAmount = yield;
        plantDrop.AddDropToInventoryMarket();
    }
}
