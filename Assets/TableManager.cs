using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    [SerializeField] AllTableManager AllTableManager;
    public Transform wayPointParent;
    public bool isAvailable = false;

    private void Awake()
    {
        AllTableManager = FindObjectOfType<AllTableManager>();
        AllTableManager.availableTables.Add(this);
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < wayPointParent.childCount; i++) 
        {
            Gizmos.DrawSphere(wayPointParent.GetChild(i).position, 0.2f);
            if (i < wayPointParent.childCount - 1)
            {
                Gizmos.DrawLine(wayPointParent.GetChild(i).position, wayPointParent.GetChild(i + 1).position);
            }
        }
    }
}
