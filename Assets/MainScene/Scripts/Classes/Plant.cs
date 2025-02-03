using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public Drop drop;
    private float plantScale = 0.0075f;
    private float spawnInterval = 0.5f;
    public string plantCardID;

    public int yield;
    public int water;
    public int nitrogen;
    public int phosphorus;
    public int potassium;
    public int magnesium;
    public int sulfur;
    public int calcium;

    public void GiveDrop(Transform plot)
    {
        //StartCoroutine(SpawnDrops(plot));
    }

    private IEnumerator SpawnDrops(Transform plot)
    {
        for (int i = 0; i < yield; i++)
        {
            Drop plantDrop = Instantiate(drop, Vector3.zero, Quaternion.identity);
            plantDrop.transform.localScale = new Vector3(plantScale, plantScale, plantScale);
            plantDrop.transform.localPosition = new Vector3(plot.position.x, 5f, plot.position.z);
            plantDrop.MoveToInventory();
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
