using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TableBuyManager : MonoBehaviour
{
    [SerializeField] float checkRadius;
    [SerializeField] LayerMask checkLayer;
    [SerializeField] TextMeshProUGUI interactText;
    [SerializeField] TableManager currentTable;


    void Update()
    {
        Collider2D _collider = Physics2D.OverlapCircle(transform.position, checkRadius, checkLayer);
        if (_collider)
        {
            if (_collider.CompareTag("BuyTable"))
            {
                interactText.SetText("E To Buy Table");
                currentTable = _collider.transform.parent.GetComponent<TableManager>();
            }
        }
        else
        {
            interactText.SetText("");
            currentTable = null;
        }
        if (_collider != null && Input.GetKeyDown(KeyCode.E))
        {
            currentTable.BuyTable();
        }
    }
}
