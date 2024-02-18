using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("References")]
    public Rigidbody2D rb;
    public Animator animator;

    [Header("Movement")]
    public float horizontal;
    [SerializeField] float speed;
    bool facingRight = true;
    public bool isRunnig;


    [Header("Jump & DoubleJump")]
    [SerializeField] float jumpHeight;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;
    [SerializeField] Vector2 groundCheckVector;
    [SerializeField] float groundCheckRadius;
    [SerializeField] float doubleJumpingPower = 6f;
    [SerializeField] public bool doubleJump;

    [Header("WallSliding & WallJumping")]
    [SerializeField] float wallJumpingTime = 0.2f;
    [SerializeField] float wallJumpingDuration = 0.4f;
    Vector2 wallJumpingPower = new Vector2(8f, 16f);
    [SerializeField] float wallCheckRadius, wallSlidingSpeed;
    float  wallJumpingDirection, wallJumpingCounter;
    public bool isWallSliding, isWallJumping;
    bool isWallDetected;
    [SerializeField] Transform wallCheck;
    [SerializeField] LayerMask wallLayer;

    IState currentState;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        currentState = new IdleState();
        currentState.EnterState(this);
    }

    public void ChangeState(IState newState)
    {
        currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }
    private void Update()
    {
        currentState.UpdateState(this);

        horizontal = Input.GetAxis("Horizontal");

        if (!isWallJumping)
        {
            Flip();
        }
    }

    public void IsSliding()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.SetBool("isSliding",false);
        }
    }

    public void Jump()
    {
        if (IsGrounded() || doubleJump)
        {
            if (IsGrounded())
            {
                doubleJump = false;
            }

            if (!doubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
                doubleJump = true;
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, doubleJumpingPower);
                doubleJump = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }

    }

    public bool IsGrounded()
    {
        //return Physics2D.OverlapBox(groundCheck.position, groundCheckVector,0, groundLayer);
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    public bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer);
    }

    public void WallSlide()
    {
        rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
    }

    public void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }


        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f && !IsGrounded())
        {
            isWallJumping = true;
            isWallSliding = false;

            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            animator.SetBool("WallSlide", false);
            animator.SetBool("Jump", true);

            if (transform.localScale.x != wallJumpingDirection)
            {
                facingRight = !facingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
        animator.SetBool("Jump", false);
    }

    private void Flip()
    {
        if (facingRight && horizontal < 0f || !facingRight && horizontal > 0f)
        {
            facingRight = !facingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
    }
}
