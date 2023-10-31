using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;

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
        } if(GameInput.Instance.IsDashing() && Ctx.DashTimeoutDelta <= 0.0f && Ctx.Grounded){
            EndAttackEarly();
            SwitchState(Factory.Dash());
            return true;
        }
        return false;
    }

    public override void EnterState()
    {
        if (GameInput.Instance.GetMove() != Vector2.zero){
            RotateDirection();
        }

        Ctx.Speed = 0.0f;

        //Ctx.Animator.applyRootMotion = true;

        Ctx.ComboTimerRunning = false;

        EnemyDetection();

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

    void EnemyDetection(){
        Transform nearestEnemy;
        if(GameInput.Instance.GetMove() == Vector2.zero){
            nearestEnemy = FindNearestInSphere();
            if(nearestEnemy != null){
                MoveToTarget(nearestEnemy);
            }
        } else {
            nearestEnemy = FindNearestInBox();
            if(nearestEnemy != null){
                MoveToTarget(nearestEnemy);
            }
        }
    }

    Transform FindNearestInSphere(){
        Collider[] hits = Physics.OverlapSphere(Ctx.transform.position,Ctx.ReachDistance);
        List<Transform> enemies = new();
        //Debug.Log(hits.Length);

        foreach(Collider hit in hits){
            if(hit.transform.TryGetComponent(out IDamageable damageable)){
                enemies.Add(hit.transform);
            }
        }
        //Debug.Log(enemies.Count);
        if(enemies.Count > 0)
            return UtilityFunctions.GetClosestTransform(Ctx.transform.position,enemies.ToArray());
        
        return null;
    }

    Transform FindNearestInBox(){
        float halfDistance = Ctx.ReachDistance * 0.5f;
        Collider[] hits = Physics.OverlapBox(Ctx.transform.position + Ctx.transform.forward * halfDistance, Vector3.one * halfDistance,Ctx.transform.rotation);
        List<Transform> enemies = new();
        //Debug.Log(hits.Length);

        foreach(Collider hit in hits){
            if(hit.transform.TryGetComponent(out IDamageable damageable)){
                enemies.Add(hit.transform);
            }
        }
        //Debug.Log(enemies.Count);
        if(enemies.Count > 0)
            return UtilityFunctions.GetClosestTransform(Ctx.transform.position,enemies.ToArray());
        
        return null;
    }

    void MoveToTarget(Transform target){
        Vector3 targetOffset = Vector3.MoveTowards(target.position, Ctx.transform.position, .90f);
        targetOffset.y = Ctx.transform.position.y;

        Vector3 directionToTarget = targetOffset - Ctx.transform.position;
        if(directionToTarget.magnitude > 0.3f){
            Ctx.transform.DOMove(targetOffset,Ctx.ReachDuration);
            //Debug.Log(directionToTarget.magnitude);
        }

        Ctx.transform.DOLookAt(target.transform.position, .2f,AxisConstraint.Y);
    }

    void EndAttackEarly(){
        Ctx.ComboTimerRunning = true;
        Ctx.ComboTimeoutDelta = Ctx.ComboTimerDelay;

        if(Ctx.ComboCounter == 0){
            //Debug.Log("Last Attack");
            Ctx.LastComboEnd = Time.time;
        }

        Ctx.CurrentWeapon.Deactivate();
    }

    void RotateDirection(){
        Vector3 inputDirection = new Vector3(GameInput.Instance.GetMove().x, 0.0f, GameInput.Instance.GetMove().y).normalized;
        Ctx.TargetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + Ctx.MainCamera.eulerAngles.y;
        //Ctx.transform.rotation = Quaternion.Euler(0.0f, Ctx.TargetRotation, 0.0f);
        Ctx.TargetDirection = Quaternion.Euler(0.0f, Ctx.TargetRotation, 0.0f) * Vector3.forward;

        Ctx.transform.DORotate(new Vector3(0f,Ctx.TargetRotation,0f),.2f);
    }
}
