using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    public PlayerMoveState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext,playerStateFactory){}

    public override void CheckSwitchStates()
    {
        if(GameInput.Instance.GetMove() == Vector2.zero){
            SwitchState(Factory.Idle());
        }
    }

    public override void EnterState()
    {
        Ctx.TargetSpeed = Ctx.MoveSpeed;
    }

    public override void ExitState()
    {
    }

    public override void InitializeSubState()
    {
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }
}
