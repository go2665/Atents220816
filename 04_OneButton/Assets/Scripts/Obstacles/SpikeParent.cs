using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeParent : MonoBehaviour
{
    const float spikeWidth = 0.8f;

    private void Awake()
    {
        GameObject spike = transform.GetChild(0).gameObject;
        int count = Random.Range(0, 4);
        for(int i=1;i<count;i++)
        {
            GameObject add = Instantiate(spike, transform);
            add.transform.Translate(spikeWidth * i * Vector3.right);
        }

        Spike[] spikes = GetComponentsInChildren<Spike>();
        foreach(var temp in spikes)
        {
            temp.onDie += OnSpikeDestroy;
        }
    }

    void OnSpikeDestroy()
    {
        if( transform.childCount < 1)
        {
            Destroy(this.gameObject);
        }
    }
}
