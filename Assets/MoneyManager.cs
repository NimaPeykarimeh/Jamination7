using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] int totalMoney;
    [SerializeField] TextMeshProUGUI moneyText;


    private void Start()
    {
        moneyText.SetText("$" + totalMoney.ToString());
    }
    public void GetMoney(int _amount)
    {
        totalMoney += _amount;
        moneyText.SetText("$" + totalMoney.ToString());
    }


    public bool SpendMoney(int _spendAmount)
    {
        if (_spendAmount <= totalMoney)
        {
            totalMoney -= _spendAmount;
            moneyText.SetText("$" + totalMoney.ToString());
            return true;
        }

        return false;
    }
}
