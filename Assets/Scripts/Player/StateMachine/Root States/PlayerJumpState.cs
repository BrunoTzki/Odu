using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext,playerStateFactory){
        IsRootState = true;
    }

    public override void CheckSwitchStates()
    {
        if(Ctx.VerticalVelocity < 0.0f){
            SwitchState(Factory.Fall());
        }
    }

    public override void EnterState()
    {
        InitializeSubState();

        HandleJump();

        // reset the jump timeout timer
        Ctx.JumpTimeoutDelta = Ctx.JumpTimeout;
    }

    public override void ExitState()
    {
        if(Ctx.HasAnimator){
            Ctx.Animator.SetBool(Ctx.AnimIDJump, false);
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
        CheckSwitchStates();
    }

    private void HandleJump(){
        // the square root of H * -2 * G = how much velocity needed to reach desired height
        Ctx.VerticalVelocity = Mathf.Sqrt(Ctx.JumpHeight * -2f * Ctx.Gravity);

        // update animator if using character
        if (Ctx.HasAnimator)
        {
            Ctx.Animator.SetBool(Ctx.AnimIDJump, true);
        }        
    }
}
