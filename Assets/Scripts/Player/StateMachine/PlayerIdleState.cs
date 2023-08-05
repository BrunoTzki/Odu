using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext,playerStateFactory){}

    public override void CheckSwitchStates()
    {
        if(GameInput.Instance.GetMove() != Vector2.zero){
            SwitchState(Factory.Move());
        }
    }

    public override void EnterState()
    {
        Ctx.TargetSpeed = 0.0f;
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
