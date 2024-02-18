using UnityEngine;

public interface IState
{
    void EnterState(PlayerMovement playerMovement);
    void UpdateState(PlayerMovement playerMovement);
    void ExitState(PlayerMovement playerMovement);
}
