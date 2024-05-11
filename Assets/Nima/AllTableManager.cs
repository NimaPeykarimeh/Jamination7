using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllTableManager : MonoBehaviour
{
    QueueManager queueManager;
    public List<TableManager> availableTables;
    public bool tableAvailable;

    private void Awake()
    {
        queueManager = FindObjectOfType<QueueManager>();
    }

    public void GetRandomTable(CatMovement _cat)
    {
        if (availableTables.Count != 0)
        {
            int _randomInt = Random.Range(0, availableTables.Count);
            _cat.currentTableManager = availableTables[_randomInt];
            availableTables.Remove(availableTables[_randomInt]);
            queueManager.SendTheFirstCatInQueue();

        }

        

    }


}
