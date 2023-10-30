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
            nearestEnemy = FindNearest();
            if(nearestEnemy != null){
                MoveToTarget(nearestEnemy);
            }
        } else {
            if(Physics.SphereCast(Ctx.transform.position, Ctx.FrontCastRadius, Ctx.TargetDirection, out RaycastHit hit, Ctx.ReachDistance - Ctx.FrontCastRadius)){
                nearestEnemy = hit.collider.transform;
                if(nearestEnemy.TryGetComponent(out IDamageable damageable)){
                    MoveToTarget(nearestEnemy);
                }
            }
        }
    }

    Transform FindNearest(){
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

    void MoveToTarget(Transform target){
        Vector3 targetOffset = Vector3.MoveTowards(target.position, Ctx.transform.position, .90f);
        targetOffset.y = Ctx.transform.position.y;

        Ctx.transform.DOLookAt(target.transform.position, .2f,AxisConstraint.Y);
        Ctx.transform.DOMove(targetOffset,Ctx.ReachDuration);
        //Ctx.transform.DOMove(target.position,Ctx.ReachDuration);
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
}
