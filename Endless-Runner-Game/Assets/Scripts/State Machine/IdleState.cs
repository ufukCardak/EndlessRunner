using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    public void EnterState(PlayerMovement playerMovement)
    {
        Debug.Log("Entering Idle");
    }

    public void ExitState(PlayerMovement playerMovement)
    {
        Debug.Log("Exiting Idle");
    }

    public void UpdateState(PlayerMovement playerMovement)
    {
        if (playerMovement.IsGrounded() && playerMovement.horizontal != 0) // Run
        {
            playerMovement.ChangeState(new RunState());
        }
        else if (Input.GetButtonDown("Jump")) // Jump 
        {
            playerMovement.ChangeState(new JumpState());
        }
        else if(playerMovement.rb.velocity.y < 0 && !playerMovement.IsGrounded()) // Fall
        {
            playerMovement.ChangeState(new FallState());
        }
        else if (!playerMovement.IsGrounded() && playerMovement.IsWalled()) // WallSlide
        {
            playerMovement.ChangeState(new WallSlideState());
        }
    }
}
