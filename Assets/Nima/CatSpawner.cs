using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSpawner : MonoBehaviour
{
    QueueManager queueManager;
    [SerializeField] GameObject catPrefab;

    [SerializeField] float minSpawnCoolDown = 1f;
    [SerializeField] float maxSpawnCooldown = 10f;
    [SerializeField] float spawnTimer;

    [SerializeField] bool isQueueAvailable;
    [SerializeField] bool isReadyToSpawn;
    int count = 0;

    private void Awake()
    {
        queueManager = FindObjectOfType<QueueManager>();
    }

    private void Start()
    {
        //spawnTimer = spawnCoolDown;
    }

    private void Update()
    {
        if (!isReadyToSpawn)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0)
            {
                isReadyToSpawn = true;
            }
        }
        if (!isQueueAvailable)
        {
            isQueueAvailable = queueManager.CheckQueue();
        }

        if (isQueueAvailable && isReadyToSpawn)
        {
            Spawn();
        }
        
    }

    void Spawn()
    {
        count++;
        isQueueAvailable = false;
        isReadyToSpawn = false;
        spawnTimer = Random.Range(minSpawnCoolDown,maxSpawnCooldown);
        GameObject a = Instantiate(catPrefab,transform.position,transform.rotation);
        a.name = count.ToString();
    }

}
