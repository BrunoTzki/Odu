using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    public PlayerFallState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext,playerStateFactory){
        IsRootState = true;
    }

    public override void CheckSwitchStates()
    {
        if(Ctx.Grounded){
            SwitchState(Factory.Grounded());
        }
    }

    public override void EnterState()
    {
        InitializeSubState();
    }

    public override void ExitState()
    {
        if (Ctx.HasAnimator)
        {
            Ctx.Animator.SetBool(Ctx.AnimIDFreeFall, false);
        }
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
        HandleFall();
        
        CheckSwitchStates();
    }

    private void HandleFall(){
        // fall timeout
        if (Ctx.FallTimeoutDelta >= 0.0f)
        {
            Ctx.FallTimeoutDelta -= Time.deltaTime;
        }
        else
        {
            // update animator if using character
            if (Ctx.HasAnimator)
            {
                Ctx.Animator.SetBool(Ctx.AnimIDFreeFall, true);
            }
        }
    }
}
