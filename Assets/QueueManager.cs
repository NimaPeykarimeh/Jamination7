using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    public Queue[] QueueList;

    public bool CheckQueue()
    {
        foreach (Queue _que in QueueList)
        {
            if (_que.catInQueue == null)
            {
                return true;
            }
        }
        return false;
    }


    public Transform AddToQueue(CatMovement _catObject)
    {
        for (int i = 0; i < QueueList.Length; i++)
        {
            if (QueueList[i].catInQueue == null)
            {
                QueueList[i].catInQueue = _catObject;
                return QueueList[i].QueueObject.transform;
            }
        }
        return null;
    }

    public bool IsFirstInQueue(CatMovement _Cat)
    {
        return _Cat == QueueList[0].catInQueue;
    }

    public void SendTheFirstCatInQueue()
    {
        for (int i = 0; i < QueueList.Length - 1; i++)
        {
            print(i);
            QueueList[i].catInQueue = QueueList[i + 1].catInQueue;
            if (QueueList[i].catInQueue != null)
            {
                QueueList[i].catInQueue.targetQueueTransform = QueueList[i].QueueObject.transform;
                QueueList[i].catInQueue.currentState = CatMovement.CatStages.Entering;
            }

        }
        QueueList[QueueList.Length - 1].catInQueue = null;
    }

    [Serializable]
    public class Queue
    {
        public CatMovement catInQueue;
        public GameObject QueueObject;
    }
}
