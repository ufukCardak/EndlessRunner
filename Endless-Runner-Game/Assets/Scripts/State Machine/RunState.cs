using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class RunState : IState
{
    public void EnterState(PlayerMovement playerMovement)
    {
        Debug.Log("Entering Run");
        playerMovement.animator.SetBool("canRun", true);
    }

    public void ExitState(PlayerMovement playerMovement)
    {
        Debug.Log("Exiting Run");
        playerMovement.animator.SetBool("canRun", false);
    }

    public void UpdateState(PlayerMovement playerMovement)
    {
        if (Input.GetButtonDown("Jump")) // Jump 
        {
            playerMovement.ChangeState(new JumpState());
        }
        else if (playerMovement.IsGrounded() && playerMovement.horizontal == 0) // Idle
        {
            playerMovement.ChangeState(new IdleState());
        }
        else if (playerMovement.rb.velocity.y < 0 && !playerMovement.IsGrounded()) // Fall
        {
            playerMovement.ChangeState(new FallState());
        }
        else if (Input.GetKey(KeyCode.LeftShift) && !playerMovement.animator.GetBool("Jump")) // Slide
        {
            playerMovement.ChangeState(new SlideState());
        }
        else if (!playerMovement.IsGrounded() && playerMovement.IsWalled()) // WallSlide
        {
            playerMovement.ChangeState(new WallSlideState());
        }
    }
}
