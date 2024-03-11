using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEndAttackState : PlayerBaseState
{
    public PlayerEndAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext,playerStateFactory){}

    public override bool CheckSwitchStates()
    {
        if (GameInput.Instance.IsAttacking()){
            if(Ctx.ComboCounter != 0){
                SwitchState(Factory.StartAttack());
                return true;
            }

        } /*if(GameInput.Instance.GetMove() != Vector2.zero){
            SwitchState(Factory.Move());
            //Ctx.Animator.Play("Idle Walk Run Blend", 0, 0);
            return true;

        } */
        if(GameInput.Instance.IsDashing() && Ctx.DashTimeoutDelta <= 0.0f && Ctx.Grounded){
            SwitchState(Factory.Dash());
            return true;

        } if(Ctx.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f){ //animation ended
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
        Ctx.ComboTimerRunning = true;
        Ctx.ComboTimeoutDelta = Ctx.ComboTimerDelay;

        if(Ctx.ComboCounter == 0){
            Ctx.LastComboEnd = Time.time;
        }

        Ctx.CurrentWeapon.Deactivate();

        //Ctx.Animator.applyRootMotion = false;

        //Ctx.Animator.SetBool(Ctx.AnimIDAttack,false);
    }

    public override void InitializeSubState()
    {
    }

    public override void UpdateState()
    {
        if(CheckSwitchStates()) return;
    }
}
