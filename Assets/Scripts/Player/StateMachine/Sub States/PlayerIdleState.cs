using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext,playerStateFactory){}

    public override bool CheckSwitchStates()
    {
        Debug.Log("Idle Update");
        if(GameInput.Instance.IsAttacking()){
            if(Time.time - Ctx.LastComboEnd > Ctx.ComboWaitTime){
                SwitchState(Factory.StartAttack());
                return true;
            }
        } if(GameInput.Instance.GetMove() != Vector2.zero){
            SwitchState(Factory.Move());
            return true;
        } if(GameInput.Instance.IsDashing() && Ctx.DashTimeoutDelta <= 0.0f && Ctx.Grounded){
            SwitchState(Factory.Dash());
            return true;
        }

        return false;
    }

    public override void EnterState()
    {
        //Ctx.TargetSpeed = 0.0f;
        Ctx.TargetRotation = Ctx.transform.eulerAngles.y;
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

        HandleMove();
    }

    void HandleMove(){
        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(Ctx.Controller.velocity.x, 0.0f, Ctx.Controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = GameInput.Instance.GetMove().magnitude;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < Ctx.TargetSpeed - speedOffset || currentHorizontalSpeed > Ctx.TargetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            Ctx.Speed = Mathf.Lerp(currentHorizontalSpeed, Ctx.TargetSpeed * inputMagnitude, Time.deltaTime * Ctx.SpeedChangeRate);

            // round speed to 3 decimal places
            Ctx.Speed = Mathf.Round(Ctx.Speed * 1000f) / 1000f;
        }
        else
        {
            Ctx.Speed = Ctx.TargetSpeed;
        }

        Ctx.AnimationBlend = Mathf.Lerp(Ctx.AnimationBlend, Ctx.TargetSpeed, Time.deltaTime * Ctx.SpeedChangeRate);
        if (Ctx.AnimationBlend < 0.01f) 
            Ctx.AnimationBlend = 0f;

        Ctx.TargetDirection = Quaternion.Euler(0.0f, Ctx.TargetRotation, 0.0f) * Vector3.forward;

        // move the player
        // _controller.Move(_targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        // update animator if using character
        if (Ctx.HasAnimator)
        {
            Ctx.Animator.SetFloat(Ctx.AnimIDSpeed, Ctx.Speed);
            //_animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
        }
    }
}
