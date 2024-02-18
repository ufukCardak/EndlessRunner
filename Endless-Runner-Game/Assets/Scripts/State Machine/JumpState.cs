using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : IState
{
    public void EnterState(PlayerMovement playerMovement)
    {
        Debug.Log("Entering Jump");
        playerMovement.animator.SetBool("isSliding", false);
        playerMovement.Jump();
        playerMovement.animator.SetBool("Jump", true);
    }

    public void ExitState(PlayerMovement playerMovement)
    {
        Debug.Log("Exiting Jump");
        playerMovement.animator.SetBool("Jump", false);
    }

    public void UpdateState(PlayerMovement playerMovement)
    {
        
        if (playerMovement.IsGrounded() && playerMovement.horizontal != 0 && !playerMovement.animator.GetBool("Jump")) // Run
        {
            playerMovement.ChangeState(new RunState());
        }
        else if (playerMovement.IsGrounded() && playerMovement.horizontal == 0 && playerMovement.rb.velocity == Vector2.zero) // Idle
        {
            playerMovement.ChangeState(new IdleState());
        }
        else if (playerMovement.rb.velocity.y < 0 && !playerMovement.IsGrounded()) // Fall
        {
            playerMovement.ChangeState(new FallState());
        }
        else if (!playerMovement.IsGrounded() && playerMovement.IsWalled()) // WallSlide
        {
            playerMovement.ChangeState(new WallSlideState());
        }
        else if (Input.GetButtonDown("Jump")) // Jump 
        {
            playerMovement.ChangeState(new JumpState());
        }
    }
}
