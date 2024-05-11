using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class CatMovement : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb2;
    QueueManager queueManager;
    Vector2 movingDirection;
    FoodMenuManager foodMenuManager;
    [SerializeField] Transform spriteBody;

    public Transform targetQueueTransform;
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

    [Header("Eating")]
    [SerializeField] FoodMenuManager.FoodList currentFood;
    [SerializeField] float eatingDuration = 10f;
    [SerializeField] float eatingTimer;
    [SerializeField] TextMeshProUGUI foodOrderText;
    [SerializeField] GameObject table;
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
        foodMenuManager = FindObjectOfType<FoodMenuManager>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        allTableManager = FindObjectOfType<AllTableManager>();
        
    }

    void FlipTheBody()
    {
        if (movingDirection.x > 0)
        {
            spriteBody.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if (movingDirection.x < 0)
        {
            spriteBody.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    void GetATable()
    {
        allTableManager.GetRandomTable(this);
        if (currentTableManager != null ) 
        {
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

    public void FoodServed(FoodMenuManager.FoodList _servedFood)
    {
        if (currentState == CatStages.WaitingForOrder)
        {
            if (_servedFood == currentFood)
            {
                currentState = CatStages.Eating;
                foodOrderText.SetText("Eating...");
                eatingTimer = eatingDuration;
            }
            else
            {
                print("WRONG FOOD");
                ExitTheRestaurant(true);
            }
        }
        else
        {
            print("NO NEED FOR FOOD");
        }
        
    }

    void OrderFood()
    {
        currentFood = foodMenuManager.GetRandomFood();
        foodOrderText.SetText(currentFood.ToString());
    }

    void ExitTheRestaurant(bool _isAngry)
    {
        if (_isAngry) 
        {
            currentTableManager.gameObject.SetActive(false);
            table.SetActive(true);
        }
        else
        {
            allTableManager.availableTables.Add(currentTableManager);
        }
        currentState = CatStages.Exiting;
        tableWayPointIndex = 0;
        patienceBar.gameObject.SetActive(false);
        foodOrderText.SetText("");
    }

    private void Update()
    {
        animator.SetFloat("Dir_y",movingDirection.y);
        FlipTheBody();
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
                    OrderFood();
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
                ExitTheRestaurant(true);
                
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

        if (currentState == CatStages.Eating)
        {
            eatingTimer -= Time.deltaTime;
            float _ratio = eatingTimer / eatingDuration;
            patienceBar.fillAmount = _ratio;
            if (eatingTimer <= 0)
            {
                ExitTheRestaurant(false);
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
