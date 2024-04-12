using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName ="Idle-Random Wander", menuName = "Combat/ Enemy Logic/ Idle Logic/ Random Wander")]
public class EnemyIdleRandomWander : EnemyIdleSOBase
{
    [SerializeField] private float _randomMovementRange = 5f;
    [SerializeField] private float _randomMovementSpeed = 1f;
    [SerializeField] private float _wanderTimeLimit = 10f;

    private Vector3 _targetPos;
    private Vector3 _direction;

    private float _lastPointChosenTime;

    public override void DoEnterLogic(){
        ChangeTargetPosition();
    }
    public override void DoExitLogic(){
        base.DoExitLogic();
    }
    public override void DoFrameUpdateLogic(){
        base.DoFrameUpdateLogic();

        _direction = (_targetPos - enemy.transform.position).normalized;

        enemy.MoveEnemy(_direction * _randomMovementSpeed);

        if(Time.time - _lastPointChosenTime >= _wanderTimeLimit){
            ChangeTargetPosition();
        }
        if((enemy.transform.position - _targetPos).sqrMagnitude < 0.01f){
            ChangeTargetPosition();
        }
    }
    public override void DoPhysicsLogic(){ }
    public override void DoAnimationTriggerEventLogic(EnemyScript.AnimationTriggerType triggerType){ }
    public override void ResetValues(){ }

    private Vector3 GetRandomPointInCircle(){
        Vector2 circlePos = UnityEngine.Random.insideUnitCircle;

        return enemy.transform.position + new Vector3(circlePos.x, 0f, circlePos.y) * _randomMovementRange;
    }

    private void ChangeTargetPosition(){
        //Debug.Log("enemy: Change Position", enemy);
        _targetPos = GetRandomPointInCircle();
        _lastPointChosenTime = Time.time;

        enemy.RB.inertiaTensorRotation = Quaternion.identity;
        enemy.transform.DOLookAt(_targetPos, enemy.RotateDuration);
    }
}
