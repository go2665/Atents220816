using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeFactory : Singleton<SlimeFactory>
{
    public GameObject slimePrefab;
        
    public int poolSize = 128;

    Slime[] pool;
    Queue<Slime> readyQueue;

    protected override void Initialize()
    {
        base.Initialize();

        pool = new Slime[poolSize];
        readyQueue = new Queue<Slime>(poolSize);

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(slimePrefab, this.transform);
            obj.name = $"Slime_{i}";
            Slime slime = obj.GetComponent<Slime>();
            slime.onDisable += () =>
            {
                readyQueue.Enqueue(slime);
            };
            pool[i] = slime;
            obj.SetActive(false);
        }        
    }


    public Slime GetSlime()
    {
        if (readyQueue.Count > 0)
        {
            Slime slime = readyQueue.Dequeue();
            slime.gameObject.SetActive(true);
            return slime;
        }
        else
        {
            int newSize = poolSize * 2;
            Slime[] newPool = new Slime[newSize];
            for(int i=0;i<poolSize;i++)
            {
                newPool[i] = pool[i];
            }            
            for (int i=poolSize; i<newSize;i++)
            {
                GameObject obj = Instantiate(slimePrefab, this.transform);
                obj.name = $"Slime_{i}";
                Slime slime = obj.GetComponent<Slime>();
                slime.onDisable += () =>
                {
                    readyQueue.Enqueue(slime);
                };
                newPool[i] = slime;
                obj.SetActive(false);
            }

            return GetSlime();
        }
    }
}
