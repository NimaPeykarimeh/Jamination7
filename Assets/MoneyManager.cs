using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    AudioSource m_AudioSource;
    [SerializeField] int totalMoney;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] MoneyAnimTrigger moneyAnimTrigger;
    [SerializeField] AudioClip spendSound;
    [SerializeField] AudioClip getMoneySound;

    [SerializeField] Animator totalMoneyAnimator;
    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        moneyText.SetText("$" + totalMoney.ToString());
    }
    public void GetMoney(int _amount)
    {
        totalMoney += _amount;
        moneyAnimTrigger.moneyAnimText.SetText("$" + _amount.ToString());
        m_AudioSource.PlayOneShot(getMoneySound);
        moneyText.SetText("$" + totalMoney.ToString());
        moneyAnimTrigger.TriggerAnimation(false);
    }


    public bool SpendMoney(int _spendAmount)
    {
        if (_spendAmount <= totalMoney)
        {
            m_AudioSource.PlayOneShot(spendSound);
            totalMoney -= _spendAmount;
            moneyAnimTrigger.moneyAnimText.SetText("$" + _spendAmount.ToString());
            moneyAnimTrigger.TriggerAnimation(true);
            moneyText.SetText("$" + totalMoney.ToString());
            return true;
        }
        else
        {
            totalMoneyAnimator.SetTrigger("NotEnough");
        }
        return false;
    }
}
