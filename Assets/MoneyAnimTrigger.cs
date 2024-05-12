using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyAnimTrigger : MonoBehaviour
{
    Animator animator;
    public TextMeshProUGUI moneyAnimText;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void TriggerAnimation(bool isSpending)
    {
        if (isSpending)
        {
            animator.SetTrigger("Spend");
        }
        else
        {
            animator.Play("GetMoney");
        }
    }

}
