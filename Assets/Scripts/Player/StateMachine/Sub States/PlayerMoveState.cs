using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    public PlayerMoveState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext,playerStateFactory){}

    private float _rotationVelocity;

    public override bool CheckSwitchStates()
    {
        if(GameInput.Instance.IsAttacking() == true && !Ctx.Animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")){
            SwitchState(Factory.StartAttack());
            return true;
        } if(GameInput.Instance.GetMove() == Vector2.zero){
            SwitchState(Factory.Idle());
            return true;
        } if(GameInput.Instance.IsDashing() && Ctx.DashTimeoutDelta <= 0.0f && Ctx.Grounded){
            SwitchState(Factory.Dash());
            return true;
        }
        return false;
    }

    public override void EnterState()
    {
        Ctx.TargetSpeed = Ctx.MoveSpeed;
        
    }

    public override void ExitState()
    {
        Ctx.TargetSpeed = 0.0f;
    }

    public override void InitializeSubState()
    {
    }

    public override void UpdateState()
    {
        if(CheckSwitchStates()) return;
        //Ctx.HandleMove();
        HandleMove();

        //CheckSwitchStates();
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

        // normalise input direction
        Vector3 inputDirection = new Vector3(GameInput.Instance.GetMove().x, 0.0f, GameInput.Instance.GetMove().y).normalized;

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (GameInput.Instance.GetMove() != Vector2.zero)
        {
            Ctx.TargetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + Ctx.MainCamera.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(Ctx.transform.eulerAngles.y, Ctx.TargetRotation, ref _rotationVelocity, Ctx.RotationSmoothTime);

            // rotate to face input direction relative to camera position
            Ctx.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }


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
