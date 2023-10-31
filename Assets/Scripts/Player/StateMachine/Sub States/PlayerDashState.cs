using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    public PlayerDashState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext,playerStateFactory){}

    private float _dashTimeDelta;

    public override bool CheckSwitchStates()
    {
        if(_dashTimeDelta <= 0f && GameInput.Instance.GetMove() == Vector2.zero){
            SwitchState(Factory.Idle());
            return true;
        } else if (_dashTimeDelta <= 0f && GameInput.Instance.GetMove() != Vector2.zero){
            SwitchState(Factory.Move());
            return true;
        }

        return false;
    }

    public override void EnterState()
    {
        if (GameInput.Instance.GetMove() != Vector2.zero){
            RotateDirection();
        }
        

        Ctx.Speed = Ctx.DashSpeed;
        _dashTimeDelta = Ctx.DashTime;

        if (Ctx.HasAnimator)
        {
            Ctx.Animator.SetBool(Ctx.AnimIDDash, true);
        }
    }

    public override void ExitState()
    {
        Ctx.DashTimeoutDelta = Ctx.DashTimeout;
        if (Ctx.HasAnimator)
        {
            Ctx.Animator.SetBool(Ctx.AnimIDDash, false);
        }
    }

    public override void InitializeSubState()
    {
    }

    public override void UpdateState()
    {
        if(CheckSwitchStates()) return;

        DashTimer();

        //CheckSwitchStates();
    }

    private void DashTimer(){
        if(_dashTimeDelta >= 0.0f)
            _dashTimeDelta -= Time.deltaTime;
    }

    void RotateDirection(){
        Vector3 inputDirection = new Vector3(GameInput.Instance.GetMove().x, 0.0f, GameInput.Instance.GetMove().y).normalized;
        Ctx.TargetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + Ctx.MainCamera.eulerAngles.y;
        Ctx.transform.rotation = Quaternion.Euler(0.0f, Ctx.TargetRotation, 0.0f);
        Ctx.TargetDirection = Quaternion.Euler(0.0f, Ctx.TargetRotation, 0.0f) * Vector3.forward;
    }
}
