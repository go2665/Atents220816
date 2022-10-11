using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] spawnPrefabs;
    public float spawnInterval;
    
    WaitForSeconds interval;
    List<SpawnBase> children;

    private void Start()
    {
        interval = new WaitForSeconds(spawnInterval);
        children = new List<SpawnBase>(GetComponentsInChildren<SpawnBase>());

        StartCoroutine(SpawnRepeat());
    }

    IEnumerator SpawnRepeat()
    {
        while (true)
        {
            yield return interval;
            int index = Random.Range(0, spawnPrefabs.Length);
            GameObject obj = Instantiate(spawnPrefabs[index], transform);
            SpawnBase child = obj.GetComponent<SpawnBase>();
            if (child != null)
            {
                children.Add(child);
            }
            else
            {
                SpawnBase[] spawnBases = obj.GetComponentsInChildren<SpawnBase>();
                children.AddRange(spawnBases);
            }
        }
    }

    public void RemoveChild(SpawnBase spawnBase)
    {
        children.Remove(spawnBase);
    }

    public void UpdateSpeed(float newSpeed)
    {
        foreach(var child in children)
        {
            child.moveSpeed = newSpeed;
        }    
    }
}
