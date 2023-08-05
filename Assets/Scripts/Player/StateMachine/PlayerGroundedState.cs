using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext,playerStateFactory){
        IsRootState = true;
    }

    public override void CheckSwitchStates()
    {
        if(GameInput.Instance.IsJumping() && Ctx.JumpTimeoutDelta <= 0.0f){
            SwitchState(Factory.Jump());
        } else if (!Ctx.Grounded){
            SwitchState(Factory.Fall());
        }
    }

    public override void EnterState()
    {
        InitializeSubState();

        // stop our velocity dropping infinitely when grounded
        Ctx.VerticalVelocity = -2f;

        // reset the fall timeout timer
        Ctx.FallTimeoutDelta = Ctx.FallTimeout;
        
        // stop our velocity dropping infinitely when grounded
        Ctx.VerticalVelocity = -2f;
    }

    public override void ExitState()
    {
    }

    public override void InitializeSubState()
    {
        if(GameInput.Instance.GetMove() == Vector2.zero){
            SetSubState(Factory.Idle());
        } else {
            SetSubState(Factory.Move());
        }
    }

    public override void UpdateState()
    {
        // jump timeout
        if (Ctx.JumpTimeoutDelta >= 0.0f)
        {
            Ctx.JumpTimeoutDelta -= Time.deltaTime;
        }

        if (Ctx.VerticalVelocity < 0.0f)
        {
            Ctx.VerticalVelocity = -2f;
        }

        CheckSwitchStates();
    }
}
