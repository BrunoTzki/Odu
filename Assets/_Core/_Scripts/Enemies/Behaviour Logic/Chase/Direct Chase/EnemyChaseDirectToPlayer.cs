using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName ="Chase-Direct Chase", menuName = "Combat/ Enemy Logic/ Chase Logic/ Direct Chase")]
public class EnemyChaseDirectToPlayer : EnemyChaseSOBase
{
    [SerializeField] private float _movementSpeed = 1.75f;

    public override void DoEnterLogic(){

    }
    public override void DoExitLogic(){
        base.DoExitLogic();
    }
    public override void DoFrameUpdateLogic(){
        base.DoFrameUpdateLogic();

        Vector3 moveDirection = playerTransform.position - enemy.transform.position;
        moveDirection.y = 0f;
        moveDirection = moveDirection.normalized;

        enemy.MoveEnemy(moveDirection * _movementSpeed);

        enemy.transform.DOLookAt(playerTransform.position, enemy.RotateDuration, AxisConstraint.Y);
        //RotateToMoveDirection(moveDirection);
    }
    public override void DoPhysicsLogic(){

    }
    public override void DoAnimationTriggerEventLogic(EnemyScript.AnimationTriggerType triggerType){

    }
    public override void ResetValues(){

    }

    private void RotateToMoveDirection(Vector3 moveDirection){
        Quaternion rotation = Quaternion.LookRotation(moveDirection);
        enemy.RB.MoveRotation(rotation);
    }
}
