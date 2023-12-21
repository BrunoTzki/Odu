using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{   
    public PlayerAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext,playerStateFactory){}

    private bool _canAttack;
    private bool _hasAttacked;
    private bool _comboEnded;

    public override bool CheckSwitchStates()
    {
        if(_canAttack){
            if(GameInput.Instance.GetMove() != Vector2.zero){
                SwitchState(Factory.Move());
                return true;
            } if(GameInput.Instance.IsDashing() && Ctx.DashTimeoutDelta <= 0.0f && Ctx.Grounded){
                SwitchState(Factory.Dash());
                return true;
            } if (_comboEnded){
                SwitchState(Factory.Idle());
                return true;
            }
        }
        return false;
    }

    public override void EnterState()
    {
        Ctx.Speed = 0.0f;
        Ctx.Animator.SetFloat(Ctx.AnimIDSpeed, Ctx.Speed);

        //initialize booleans
        _canAttack = true;
        _hasAttacked = false;
        _comboEnded = false;
        Ctx.Animator.applyRootMotion = true;

        //reset combo
        if(Ctx.ComboTimeoutDelta <= 0){
            Ctx.ComboCounter = 0;
        }
        
        //restart combo timer
        Ctx.ComboTimeoutDelta = Ctx.ComboTimerDelay;

        Attack();
    }

    public override void ExitState()
    {
        Ctx.Animator.applyRootMotion = false;
    }

    public override void InitializeSubState()
    {
    }

    public override void UpdateState()
    {
        if(CheckSwitchStates()){
            return;
        }
        if(GameInput.Instance.IsAttacking()){
            Attack();
        }
        ExitAttack();

        Debug.Log("Can Attack: " + _canAttack + "; Has Attacked: " + _hasAttacked + "; Combo Ended: " + _comboEnded,Ctx);
        

        HandleComboTimer();

        
    }

    void Attack(){
        if(Time.time - Ctx.LastComboEnd > Ctx.ComboWaitTime && Ctx.ComboCounter < Ctx.CurrentTool.Combo.Count){
            CancelDelayCombo();

            if(_canAttack && !_hasAttacked){
                _hasAttacked = true;
                _canAttack = false;

                Ctx.Animator.runtimeAnimatorController = Ctx.CurrentTool.Combo[Ctx.ComboCounter].AnimatorOV;
                Ctx.Animator.SetTrigger(Ctx.AnimIDAttack);

                Ctx.ComboCounter++;

                if(Ctx.ComboCounter >= Ctx.CurrentTool.Combo.Count){
                    Ctx.ComboCounter = 0;
                }
            }
        }
    }

    void ExitAttack(){
        //animation ended
        if(Ctx.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && Ctx.Animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")){
            Debug.Log("Ended",Ctx);
            _hasAttacked = false;
            _canAttack = true;
            return;
        }
        //animation ending
        if(Ctx.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= Ctx.AnimEndPct && Ctx.Animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")){            
            Debug.Log("Exiting",Ctx);
            //Ctx.Invoke("EndCombo", 1);
            DelayCombo(Ctx.ComboTimerDelay);
            
            if(!_canAttack && _hasAttacked){
                _canAttack = true;
            }
        }
    }

    private void EndCombo(){
        Ctx.ComboCounter = 0;
        Ctx.LastComboEnd = Time.time;

        _comboEnded = true;

        //SwitchState(Factory.Idle());
    }

    void HandleComboTimer(){
        if (Ctx.ComboTimeoutDelta <= 0){
            EndCombo();

            Ctx.ComboTimerRunning = false;
        }
    }

    void DelayCombo(float seconds){
        Ctx.ComboTimerRunning = true;
        Ctx.ComboTimeoutDelta = seconds;
    }

    void CancelDelayCombo(){
        Ctx.ComboTimerRunning = false;
    }
}
