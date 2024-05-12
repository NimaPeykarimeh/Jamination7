using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    MoneyManager moneyManager;
    [SerializeField] int startingPrice = 150;
    [SerializeField] int tablePrice = 400;
    [SerializeField] GameObject tableObject;
    [SerializeField] AllTableManager AllTableManager;
    [SerializeField] GameObject interactionAreaObject;
    public Transform wayPointParent;
    public bool isStolen = false;
    [SerializeField] TextMeshProUGUI priceText;
    public CatMovement customer;

    private void Awake()
    {
        AllTableManager = FindObjectOfType<AllTableManager>();
        AllTableManager.availableTables.Add(this);
        AllTableManager.tablesLeft.Add(this);
        moneyManager = FindObjectOfType<MoneyManager>();
    }

    private void Start()
    {
        tablePrice = startingPrice;
    }

    public void TableIsStolen()
    {
        tableObject.SetActive(false);
        interactionAreaObject.SetActive(true);
        priceText.SetText("$" + tablePrice.ToString());
    }
    public void BuyTable()
    {
        if (moneyManager.SpendMoney(tablePrice))
        {
            tablePrice += 50;
            AllTableManager.BuyTable(this);
            tableObject.SetActive(true);
            interactionAreaObject.SetActive(false);
        }
        else
        {
            print("NotEnoughMoney");
        }
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
