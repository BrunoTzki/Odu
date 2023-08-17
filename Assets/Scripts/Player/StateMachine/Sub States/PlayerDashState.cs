using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    public PlayerDashState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext,playerStateFactory){}

    private float _dashTimeDelta;

    public override void CheckSwitchStates()
    {
        if(_dashTimeDelta <= 0 && GameInput.Instance.GetMove() == Vector2.zero){
            SwitchState(Factory.Idle());
        } else if (_dashTimeDelta <= 0 && GameInput.Instance.GetMove() != Vector2.zero){
            SwitchState(Factory.Move());
        }
    }

    public override void EnterState()
    {
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
        DashTimer();

        CheckSwitchStates();
    }

    private void DashTimer(){
        if(_dashTimeDelta >= 0.0f)
            _dashTimeDelta -= Time.deltaTime;
    }
}
