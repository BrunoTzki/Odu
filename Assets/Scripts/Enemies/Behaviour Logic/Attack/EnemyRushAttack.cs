using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName ="Attack-Rush Attack", menuName = "Combat/ Enemy Logic/ Attack Logic/ Rush Attack")]
public class EnemyRushAttack : EnemyAttackSOBase
{
    
    [SerializeField] private float _timeTillExit = 1f;
    private float _exitTimer;

    [SerializeField] private float _rushDistance = 9f;
    [SerializeField] private float _rushVelocity = 42f;
    [SerializeField] private AnimationCurve _rushVelocityCurve;
    private Vector3 _targetPos;
    private bool _canRush;
    private float _rushTimer;


    public override void DoEnterLogic(){
        Debug.Log(_rushVelocityCurve.length);
        _canRush=true;
        /* codigo desnecessario
        Vector3 targetPos = playerTransform.position;
        targetPos.y = enemy.transform.position.y;
        enemy.transform.DOLookAt(targetPos, .2f);
        */

        _targetPos = enemy.transform.position + enemy.transform.forward * _rushDistance;
    }
    public override void DoExitLogic(){
        base.DoExitLogic();
    }
    public override void DoFrameUpdateLogic(){
        enemy.MoveEnemy(Vector3.zero);
        SwitchStateTimer();

        if(_canRush){
            Rush();
        }
        if(_rushTimer >= _rushVelocityCurve.length){//check if reached target position
            _canRush = false;
            _rushTimer = 0f;
        }
        
    }
    public override void DoPhysicsLogic(){

    }
    public override void DoAnimationTriggerEventLogic(EnemyScript.AnimationTriggerType triggerType){

    }
    public override void ResetValues(){
        
    }

    void Rush(){
        float distanceToTargetSqrd = (_targetPos - enemy.transform.position).magnitude;
        float distanceProgress01 = 1 - distanceToTargetSqrd / _rushDistance;

        Vector3 velocity = enemy.transform.forward * (_rushVelocity * _rushVelocityCurve.Evaluate(distanceProgress01));

        _rushTimer += Time.deltaTime;

        enemy.MoveEnemy(velocity);
    }
    void SwitchStateTimer(){
        if(!enemy.IsWithinStrikingDistance){
            _exitTimer += Time.deltaTime;

            if(_exitTimer >= _timeTillExit){
                enemy.StateMachine.ChangeState(enemy.ChaseState);
            }
        } else {
            _exitTimer = 0f;
        }
    }
}
