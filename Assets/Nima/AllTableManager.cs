using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllTableManager : MonoBehaviour
{
    QueueManager queueManager;
    public List<TableManager> availableTables;
    public List<TableManager> tablesLeft;
    public List<TableManager> stolenTables;

    public bool tableAvailable;
    [SerializeField] bool isThatLastTable;
    [SerializeField] GameObject gameOverPanel;
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
            availableTables[_randomInt].customer = _cat;
            availableTables.Remove(availableTables[_randomInt]);
            queueManager.SendTheFirstCatInQueue();

        }
    }

    public void StealThisTable(TableManager _thisTable)
    {
        _thisTable.TableIsStolen();
        _thisTable.customer.ExitTheRestaurant(false);
        availableTables.Remove(_thisTable);
        tablesLeft.Remove(_thisTable);
        stolenTables.Add(_thisTable);
        
    }

    public void IsThatLastTable()
    {
        if (tablesLeft.Count == 0)
        {
            AllTablesHasStolen();
        }
        

    }

    public void AllTablesHasStolen()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0.0f;
        print("YOU LOSE");
    }

    public void BuyTable(TableManager _thisTable)
    {
        if (stolenTables.Count > 0)
        {
            foreach (TableManager _table in stolenTables)
            {
                if (_table == _thisTable)
                {
                    availableTables.Add(_thisTable);
                    tablesLeft.Add(_thisTable);
                    stolenTables.Remove(_thisTable);
                    break;
                }
            }

        }
        else
        {
            availableTables.Add(_thisTable);
            tablesLeft.Add(_thisTable);
            stolenTables.Remove(_thisTable);
        }
        
    }

    public bool IsThisTableHasStolen(TableManager _thisTable)
    {
        foreach (TableManager _table in tablesLeft)
        {
            if (_table == _thisTable)
            {
                return false;
            }
        }

        return true;
    }
    public void ReturnThisTable(TableManager _thisTable)
    {
        foreach (TableManager _table in tablesLeft)
        {
            if (_thisTable == _table)
            {
                availableTables.Add(_thisTable);
                return;
            }
        }
    }

    public void StealRandomTable(CatMovement _cat)
    {
        if (tablesLeft.Count > 0)
        {
            int _randomInt = Random.Range(0, tablesLeft.Count);
            _cat.currentTableManager = tablesLeft[_randomInt];
            
            queueManager.SendTheFirstCatInQueue();

            //tablesLeft.Remove(tablesLeft[_randomInt]);
            //foreach (TableManager _table in tablesLeft)
            //{
            //    if (availableTables.Count > _randomInt &&  availableTables[_randomInt] != null)
            //    {
            //        if (availableTables[_randomInt] == _table)
            //        {
            //            availableTables.Remove(availableTables[_randomInt]);
            //            return;
            //        }
            //    }
                
            //}
        }
    }

}
