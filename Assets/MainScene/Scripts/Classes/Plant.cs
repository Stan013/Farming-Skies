using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] private Drop drop;
    [SerializeField] private float m_DropAmount;
    public float dropAmount { get { return m_DropAmount; } }
    private float plantScale = 0.0075f;
    private float spawnInterval = 0.5f;
    public string plantCardID;

    public void GiveDrop(Transform plot)
    {
        StartCoroutine(SpawnDrops(plot));
    }

    private IEnumerator SpawnDrops(Transform plot)
    {
        for (int i = 0; i < dropAmount; i++)
        {
            Drop plantDrop = Instantiate(drop, Vector3.zero, Quaternion.identity);
            plantDrop.transform.localScale = new Vector3(plantScale, plantScale, plantScale);
            plantDrop.transform.localPosition = new Vector3(plot.position.x, 5f, plot.position.z);
            plantDrop.MoveToInventory();
            yield return new WaitForSeconds(spawnInterval);
        }
    }


}
