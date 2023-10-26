using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartAttackState : PlayerBaseState
{
    public PlayerStartAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext,playerStateFactory){}

    public override bool CheckSwitchStates()
    {
        //Debug.Log("Start Attack Update");
        //animation ending
        if(Ctx.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= Ctx.AnimEndPct && Ctx.Animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && Ctx.Animator.GetAnimatorTransitionInfo(0).normalizedTime==0){            
            //Debug.Log("Attack to End attack");
            SwitchState(Factory.EndAttack());
            return true;
        }
        return false;
    }

    public override void EnterState()
    {
        Ctx.Speed = 0.0f;

        Ctx.Animator.applyRootMotion = true;

        //Ctx.Animator.SetFloat(Ctx.AnimIDSpeed, Ctx.Speed);
        Ctx.ComboTimerRunning = false;

        Attack();
    }

    public override void ExitState()
    {
    }

    public override void InitializeSubState()
    {
    }

    public override void UpdateState()
    {
        if(CheckSwitchStates()) return;
        //CheckSwitchStates();
    }

    void Attack(){
        //Debug.Log("Attack");
        Ctx.Animator.runtimeAnimatorController = Ctx.CurrentTool.Combo[Ctx.ComboCounter].AnimatorOV;
        Ctx.Animator.SetTrigger(Ctx.AnimIDAttack);
        //Ctx.Animator.SetBool(Ctx.AnimIDAttack,true);

        Ctx.CurrentWeapon.Activate(Ctx.CurrentTool.Combo[Ctx.ComboCounter].Damage);

        Ctx.ComboCounter++;

        if(Ctx.ComboCounter >= Ctx.CurrentTool.Combo.Count){
            Ctx.ComboCounter = 0;
        }
    }
}
