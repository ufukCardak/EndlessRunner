using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlideState : IState
{
    public void EnterState(PlayerMovement playerMovement)
    {
        Debug.Log("Entering WallSlide");
        playerMovement.animator.SetBool("WallSlide", true);
        playerMovement.isWallSliding = true;
        playerMovement.doubleJump = true;
    }

    public void ExitState(PlayerMovement playerMovement)
    {
        Debug.Log("Exiting WallSlide");
        playerMovement.animator.SetBool("WallSlide", false);
        playerMovement.isWallSliding = false;
        //playerMovement.doubleJump = true;
    }

    public void UpdateState(PlayerMovement playerMovement)
    {
        playerMovement.WallSlide();
        playerMovement.WallJump();

        if (playerMovement.IsGrounded() && playerMovement.horizontal != 0 && !playerMovement.animator.GetBool("isSliding")) // Run
        {
            playerMovement.ChangeState(new RunState());
        }
        else if (Input.GetButtonDown("Jump") && !playerMovement.isWallSliding && !playerMovement.isWallJumping) // Jump 
        {
            playerMovement.ChangeState(new JumpState());
        }
        else if (playerMovement.IsGrounded() && playerMovement.horizontal == 0 && playerMovement.rb.velocity == Vector2.zero) // Idle
        {
            playerMovement.ChangeState(new IdleState());
        }
        else if (playerMovement.rb.velocity.y < 0 && !playerMovement.IsGrounded() && !playerMovement.IsWalled()) // Fall
        {
            playerMovement.ChangeState(new FallState());
        }
        else if (!playerMovement.IsGrounded() && playerMovement.IsWalled()) // WallSlide
        {
            playerMovement.ChangeState(new WallSlideState());
        }
    }

}
