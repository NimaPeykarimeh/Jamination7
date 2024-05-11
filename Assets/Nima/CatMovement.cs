using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatMovement : MonoBehaviour
{
    Rigidbody2D rb2;
    QueueManager queueManager;
    public Transform targetQueueTransform;
    Vector2 movingDirection;

    public bool firstInQueue;
    public CatStages currentState;

    public TableManager currentTableManager;
    [SerializeField] AllTableManager allTableManager;
    
    [SerializeField] float movementSpeed = 3f;

    [SerializeField] bool isReadyToMove;
    [SerializeField] bool isTableAvailable;

    [SerializeField] int tableWayPointCount;
    [SerializeField] int tableWayPointIndex;
    [SerializeField] List<Transform> tableWayPoints;


    [Header("Patiente")]
    [SerializeField] Image patienceBar;
    [Header("Ordering")]
    [SerializeField] float orderingDuration = 10f;
    [SerializeField] float orderingTimer;


    [Header("WaitingForTable")]
    [SerializeField] float waitingForTableDuration = 10f;
    [SerializeField] float waitingForTableTimer ;

    public enum CatStages
    {
        Entering,
        WaitingInQueue,
        LookingForTable,
        MovingToTable,
        WaitingForOrder,
        Eating,
        Exiting
    }

    private void Awake()
    {
        rb2 = GetComponent<Rigidbody2D>();
        queueManager = FindObjectOfType<QueueManager>();
        currentState = CatStages.Entering;
        targetQueueTransform = queueManager.AddToQueue(this);
        patienceBar.gameObject.SetActive(false);
    }

    private void Start()
    {
        allTableManager = FindObjectOfType<AllTableManager>();
        
    }


    void GetATable()
    {
        allTableManager.GetRandomTable(this);
        print(gameObject.name);
        if (currentTableManager != null ) 
        {
            print(gameObject.name);
            print(currentTableManager.gameObject.name);
            isTableAvailable = true;
            tableWayPointCount = currentTableManager.wayPointParent.childCount - 1;
            tableWayPointIndex = tableWayPointCount - 1;
            foreach (Transform _point in currentTableManager.wayPointParent)
            {
                tableWayPoints.Add(_point);
            }
            patienceBar.gameObject.SetActive(false);
            currentState = CatStages.MovingToTable;
        }
    }

    private void Update()
    {
        if (currentState == CatStages.Entering)
        {
            movingDirection = (targetQueueTransform.position - transform.position).normalized;
            rb2.velocity = movingDirection * movementSpeed;
            if (Vector2.Distance(transform.position, targetQueueTransform.position) < 0.3f)
            {
                rb2.velocity = Vector2.zero;
                currentState = CatStages.WaitingInQueue;
                patienceBar.gameObject.SetActive(true);
                waitingForTableTimer = waitingForTableDuration;
            }
        }

        if (currentState == CatStages.WaitingInQueue && queueManager.IsFirstInQueue(this))
        {
            
            
            currentState = CatStages.LookingForTable;
        }

        if (currentState == CatStages.WaitingInQueue || currentState == CatStages.LookingForTable)
        {
            waitingForTableTimer -= Time.deltaTime;
            float _ratio = waitingForTableTimer / waitingForTableDuration;
            patienceBar.fillAmount = _ratio;
        }


        if (currentState == CatStages.LookingForTable)
        {
            
            GetATable();
        }

        if (currentState == CatStages.MovingToTable)
        {
            
            movingDirection = (tableWayPoints[tableWayPointIndex].position - transform.position).normalized;
            rb2.velocity = movingDirection * movementSpeed;
            if (Vector2.Distance(transform.position, tableWayPoints[tableWayPointIndex].position) < 0.3f)
            {
                tableWayPointIndex--;
                if (tableWayPointIndex < 0)
                {
                    orderingTimer = orderingDuration;
                    patienceBar.gameObject.SetActive(true);
                    currentState = CatStages.WaitingForOrder;
                    rb2.velocity = Vector2.zero;
                }
            }
        }

        if (currentState == CatStages.WaitingForOrder)
        {
            orderingTimer -= Time.deltaTime;
            float _ratio = orderingTimer / orderingDuration;
            patienceBar.fillAmount = _ratio;

            if (orderingTimer <= 0)
            {
                currentState = CatStages.Exiting;
                tableWayPointIndex = 0;
                allTableManager.availableTables.Add(currentTableManager);
            }
        }

        if (currentState == CatStages.Exiting)
        {
            movingDirection = (tableWayPoints[tableWayPointIndex].position - transform.position).normalized;
            rb2.velocity = movingDirection * movementSpeed;
            if (Vector2.Distance(transform.position, tableWayPoints[tableWayPointIndex].position) < 0.3f)
            {
                tableWayPointIndex++;
                if (tableWayPointIndex > tableWayPointCount)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        //if (!isTableAvailable)
        //{
        //    GetATable();
        //}
        //else if(isTableAvailable && isReadyToMove)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, tableWayPoints[tableWayPointIndex].position,movementSpeed * Time.deltaTime);
        //    if (Vector2.Distance(transform.position, tableWayPoints[tableWayPointIndex].position) < 0.1f)
        //    {
        //        tableWayPointIndex--;
        //        if (tableWayPointIndex < 0)
        //        {

        //            isReadyToMove = false;
        //        }
        //    }
        //}
    }

}
