using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement2D : MonoBehaviour
{
    Rigidbody2D rb2;
    Vector2 input;
    [SerializeField] float currentHorizontalSpeed;
    [SerializeField] float currentVerticalSpeed;
    [SerializeField] Transform spriteBody;
    [SerializeField] FoodManager foodManager;

    [SerializeField] Movement2D.MovingType movingType;
    [Header("Speed Values")]
    [Range(1, 15)]
    [SerializeField] float movementSpeed = 5f;
    [Range(0.01f, 10)]
    [SerializeField] float speedUpAccelaration = 10f;
    [Range(0.01f, 10)]
    [SerializeField] float speedDownAccelaration = 10f;
    [Range(0.01f, 10)]
    [SerializeField] float stopAccelaration = 10f;

    [Header("Plarformer")]
    [Range(1f,20f)]
    [SerializeField] float jumpVelocity;
    [Range(0.5f, 5f)]
    [SerializeField] float jumpUpAcceleration;
    [Range(0.5f, 5f)]
    [SerializeField] float jumpDownAcceleration;
    [Range(0f, 0.3f)]
    [SerializeField] float jumpToleranceDuration;
    [SerializeField] float jumpToleranceTimer;
    [SerializeField] bool canJump;
    [Space]
    [SerializeField] Transform footTransform;
    [SerializeField] float rayDistance = 1f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] bool isGrounded;
    [SerializeField] float circleRadius = 0.5f;
    public Animator animator;

    [Space]
    [SerializeField] float dist;
    
    public enum MovingType
    {
        TopDown,
        Platformer
    }
    private void Awake()
    {

        animator = GetComponent<Animator>();
        rb2 = GetComponent<Rigidbody2D>();
        
    }
    private void Update()
    {
        if (!foodManager.isCooking)
        {
            if (movingType == MovingType.TopDown)
            {
                GetTopDownInput();
                UpdateTopDownSpeed();
                MoveTopDownPlayer();
                FlipThePlayer();
            }
            else if (movingType == MovingType.Platformer)
            {
                CheckGround();
                GetPlatformerInput();
                UpdatePlatformerSpeed();
                MoveTopDownPlayer();
                FlipThePlayer();
            }
        }
        else rb2.velocity = Vector2.zero;


    }

    void GetPlatformerInput()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void CheckGround()
    {
        RaycastHit2D hit2D = Physics2D.CircleCast(footTransform.position, circleRadius, -transform.up, rayDistance, groundLayer);
        if (hit2D )
        {
            Debug.DrawRay(footTransform.position, -transform.up * rayDistance, Color.green);
            isGrounded = true;
            canJump = true;
        }
        else
        {
            Debug.DrawRay(footTransform.position, -transform.up * rayDistance, Color.red);
            isGrounded = false;
            
        }

        if (!isGrounded)
        {
            jumpToleranceTimer -= Time.deltaTime;
            if (jumpToleranceTimer <= 0)
            {
                canJump = false;
            }
        }
    }
    void Jump()
    {
        if (canJump)
        {
            currentVerticalSpeed = jumpVelocity;
            jumpToleranceTimer = jumpToleranceDuration;
            canJump = false;
        }
    }

    void UpdatePlatformerSpeed()
    {
        if (input.x != 0)
        {

            if (input.x * currentHorizontalSpeed >= 0)
            {
                float xDist = Mathf.Abs(input.x * movementSpeed - currentHorizontalSpeed);
                currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, input.x * movementSpeed, Time.deltaTime * speedUpAccelaration * movementSpeed);// /(xDist)
                dist = xDist;

            }
            else
            {
                float xDist = Mathf.Abs(0 - currentHorizontalSpeed);
                currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, 0, Time.deltaTime * speedDownAccelaration * movementSpeed);
                dist = xDist;
            }
        }
        else
        {
            float xDist = Mathf.Abs(0 - currentHorizontalSpeed);
            dist = xDist;
            currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, 0, Time.deltaTime * stopAccelaration * movementSpeed);
        }

        if (!isGrounded)
        {
            if (currentVerticalSpeed >= 0)
            {
                currentVerticalSpeed -= jumpUpAcceleration * Time.deltaTime * jumpVelocity;
            }
            else if(currentVerticalSpeed < 0)
            {
                currentVerticalSpeed -= jumpDownAcceleration * Time.deltaTime * jumpVelocity;
            }
        }
        else
        {
            if (currentVerticalSpeed != 0)
            {
                currentVerticalSpeed = Mathf.MoveTowards(currentVerticalSpeed,0,jumpDownAcceleration * Time.deltaTime * jumpVelocity);
            }
        }
    }

//TopDown
    void FlipThePlayer()
    {
        if (currentHorizontalSpeed > 0)
        {
            spriteBody.eulerAngles = new Vector3(0f,0f,0f);
        }
        else if (currentHorizontalSpeed < 0)
        {
            spriteBody.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    void GetTopDownInput()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input = input.normalized;
    }

    void UpdateTopDownSpeed()
    {
        if (input.x != 0)
        {
            
            if (input.x * currentHorizontalSpeed >= 0)
            {
                float xDist = Mathf.Abs(input.x * movementSpeed - currentHorizontalSpeed);
                currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, input.x * movementSpeed, Time.deltaTime * speedUpAccelaration * movementSpeed);// /(xDist)
                dist = xDist;

            }
            else
            {
                float xDist = Mathf.Abs(0 - currentHorizontalSpeed);
                currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, 0, Time.deltaTime * speedDownAccelaration * movementSpeed);
                dist = xDist;   
            }
        }
        else
        {
            float xDist = Mathf.Abs(0 - currentHorizontalSpeed);
            dist = xDist;
            currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, 0, Time.deltaTime * stopAccelaration * movementSpeed );
        }

        if (input.y != 0)
        {
            
            if (input.y * currentVerticalSpeed >= 0)
            {
                float yDist = Mathf.Abs(input.y * movementSpeed - currentVerticalSpeed);
                currentVerticalSpeed = Mathf.MoveTowards(currentVerticalSpeed, input.y * movementSpeed, Time.deltaTime * speedUpAccelaration * movementSpeed);// /(yDist)
            }
            else
            {
                float yDist = Mathf.Abs(0 - currentVerticalSpeed);
                currentVerticalSpeed = Mathf.MoveTowards(currentVerticalSpeed, 0, Time.deltaTime * speedDownAccelaration * movementSpeed);
            }
        }
        else
        {
            float yDist = Mathf.Abs(0 - currentVerticalSpeed);
            currentVerticalSpeed = Mathf.MoveTowards(currentVerticalSpeed, 0, Time.deltaTime * stopAccelaration * movementSpeed);
        }
    }
    void MoveTopDownPlayer()
    {
        rb2.velocity = new Vector2(currentHorizontalSpeed,currentVerticalSpeed);
        animator.SetFloat("speed", rb2.velocity.magnitude);
    }
}
