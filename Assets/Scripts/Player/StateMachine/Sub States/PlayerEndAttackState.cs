using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEndAttackState : PlayerBaseState
{
    public PlayerEndAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext,playerStateFactory){}

    public override bool CheckSwitchStates()
    {
        //Debug.Log("End Attack Update");
        if(GameInput.Instance.GetMove() != Vector2.zero){
            SwitchState(Factory.Move());
            //Ctx.Animator.Play("Idle Walk Run Blend", 0, 0);
            return true;
        } if(GameInput.Instance.IsDashing() && Ctx.DashTimeoutDelta <= 0.0f && Ctx.Grounded){
            SwitchState(Factory.Dash());
            return true;
        } if(Ctx.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.97f){ //animation ended
            //Debug.Log("End Attack to Idle");
            SwitchState(Factory.Idle());
            return true;
        }

        return false;
    }

    public override void EnterState()
    {
    }

    public override void ExitState()
    {
        Ctx.ComboRunning = true;
        Ctx.ComboTimeout = Ctx.ComboTimerDelay;

        Ctx.CurrentWeapon.Deactivate();

        Ctx.Animator.applyRootMotion = false;
    }

    public override void InitializeSubState()
    {
    }

    public override void UpdateState()
    {
        if(CheckSwitchStates()) return;
        //CheckSwitchStates();
    }
}
