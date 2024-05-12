using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    [SerializeField] GameObject tableObject;
    [SerializeField] AllTableManager AllTableManager;
    [SerializeField] GameObject interactionAreaObject;
    public Transform wayPointParent;
    public bool isStolen = false;

    private void Awake()
    {
        AllTableManager = FindObjectOfType<AllTableManager>();
        AllTableManager.availableTables.Add(this);
        AllTableManager.tablesLeft.Add(this);
    }

    public void TableIsStolen()
    {
        tableObject.SetActive(false);
        interactionAreaObject.SetActive(true);
    }
    public void BuyTable()
    {
        AllTableManager.BuyTable(this);
        tableObject.SetActive(true);
        interactionAreaObject.SetActive(false);
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
