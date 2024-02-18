using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : IState
{
    public void EnterState(PlayerMovement playerMovement)
    {
        Debug.Log("Entering Fall");
        playerMovement.animator.SetBool("Jump", false);
        playerMovement.animator.SetBool("isFalling", true);

    }

    public void ExitState(PlayerMovement playerMovement)
    {
        Debug.Log("Exiting Fall");
        playerMovement.animator.SetBool("isFalling", false);
    }

    public void UpdateState(PlayerMovement playerMovement)
    {

        if (playerMovement.IsGrounded() && playerMovement.horizontal != 0) // Run
        {
            playerMovement.ChangeState(new RunState());
        }
        else if (Input.GetButtonDown("Jump") && (playerMovement.IsGrounded() || playerMovement.doubleJump)) // Jump 
        {
            playerMovement.ChangeState(new JumpState());
        }
        else if (playerMovement.IsGrounded() && playerMovement.horizontal == 0 && playerMovement.rb.velocity == Vector2.zero) // Idle
        {
            playerMovement.ChangeState(new IdleState());
        }
        else if (!playerMovement.IsGrounded() && playerMovement.IsWalled()) // WallSlide
        {
            playerMovement.ChangeState(new WallSlideState());
        }

    }

}
