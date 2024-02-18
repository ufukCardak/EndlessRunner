using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideState : IState
{
    public void EnterState(PlayerMovement playerMovement)
    {
        Debug.Log("Entering Slide");
        playerMovement.animator.SetBool("isSliding", true);
        playerMovement.doubleJump = true;
    }

    public void ExitState(PlayerMovement playerMovement)
    {
        Debug.Log("Exiting Slide");
        playerMovement.animator.SetBool("isSliding", false);
    }

    public void UpdateState(PlayerMovement playerMovement)
    {
        playerMovement.IsSliding();

        if (Input.GetButtonDown("Jump")) // Jump 
        {
            playerMovement.animator.SetBool("isSliding", false);
            playerMovement.ChangeState(new JumpState());
        }
        else if (playerMovement.IsGrounded() && playerMovement.horizontal != 0 && !playerMovement.animator.GetBool("isSliding") && !playerMovement.animator.GetBool("Jump")) // Run
        {
            playerMovement.ChangeState(new RunState());
        }
        else if (playerMovement.IsGrounded() && playerMovement.horizontal == 0 && playerMovement.rb.velocity == Vector2.zero) // Idle
        {
            playerMovement.ChangeState(new IdleState());
        }
        else if (playerMovement.rb.velocity.y < 0 && !playerMovement.IsGrounded() && !playerMovement.animator.GetBool("isSliding")) // Fall
        {
            playerMovement.ChangeState(new FallState());
        }
        else if (!playerMovement.IsGrounded() && playerMovement.IsWalled()) // WallSlide
        {
            playerMovement.ChangeState(new WallSlideState());
        }
    }
}
