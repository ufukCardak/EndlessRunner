using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    [SerializeField] Transform playerSprite;
    
    float horizontal;
    [SerializeField] float speed,jumpHeight;
    bool facingRight = true;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;
    [SerializeField] Vector2 groundCheckVector;

    bool isWallJumping;
    float wallJumpingDirection;
    float wallJumpingTime = 0.2f;
    float wallJumpingCounter;
    float wallJumpingDuration = 0.4f;
    Vector2 wallJumpingPower = new Vector2(8f, 16f);

    [SerializeField] Transform wallCheck;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] float wallCheckRadius, wallSlidingSpeed;
    bool isWallSliding,isWallDetected;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //animator = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(new Vector2(rb.velocity.x,jumpHeight));
            animator.SetBool("Jump", true);
        }

        if (horizontal != 0)
        {
            animator.SetBool("canRun", true);
        }
        else
        {
            animator.SetBool("canRun", false);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (animator.GetBool("isSliding") == true)
            {
                return;
            }
            animator.SetBool("isSliding", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.SetBool("isSliding", false);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            animator.SetBool("WallSlide",!animator.GetBool("WallSlide"));
        }

        

        WallSlide();
        WallJump();

        if (!isWallJumping)
        {
            Flip();
        }

        IsFalling();
    }
    private void FixedUpdate()
    {
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }

    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, groundCheckVector,0, groundLayer);

    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            Debug.Log("wall detected");
            animator.SetBool("WallSlide", true);
        }
        else
        {
            isWallSliding = false;
            animator.SetBool("WallSlide", false);
        }
    }

    private void WallJump()
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


        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

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
        Gizmos.DrawWireCube(groundCheck.position, groundCheckVector);

        Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
    }

    private void IsFalling()
    {
        if(rb.velocity.y < 0 && !IsGrounded())
        {
            animator.SetBool("Jump", false);
            animator.SetBool("isFalling", true);
        }
        else
        {
            animator.SetBool("isFalling", false);
        }
    }
}
