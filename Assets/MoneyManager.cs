using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] int totalMoney;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] MoneyAnimTrigger moneyAnimTrigger;

    private void Start()
    {
        moneyText.SetText("$" + totalMoney.ToString());
    }
    public void GetMoney(int _amount)
    {
        totalMoney += _amount;
        moneyAnimTrigger.moneyAnimText.SetText("$" + _amount.ToString());

        moneyText.SetText("$" + totalMoney.ToString());
        moneyAnimTrigger.TriggerAnimation(false);
    }


    public bool SpendMoney(int _spendAmount)
    {
        if (_spendAmount <= totalMoney)
        {
            totalMoney -= _spendAmount;
            moneyAnimTrigger.moneyAnimText.SetText("$" + _spendAmount.ToString());
            moneyAnimTrigger.TriggerAnimation(false);
            moneyText.SetText("$" + totalMoney.ToString());
            return true;
        }

        return false;
    }
}
