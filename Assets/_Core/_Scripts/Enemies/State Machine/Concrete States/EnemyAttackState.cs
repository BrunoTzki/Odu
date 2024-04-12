using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(EnemyScript enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {

    }

    public override void EnterState(){
        enemy.EnemyAttackInstance.DoEnterLogic();
    }

    public override void ExitState(){
        enemy.EnemyAttackInstance.DoExitLogic();
    }

    public override void FrameUpdate(){
        enemy.EnemyAttackInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate(){
        enemy.EnemyAttackInstance.DoPhysicsLogic();
    }

    public override void AnimationTriggerEvent(EnemyScript.AnimationTriggerType triggerType){
        enemy.EnemyAttackInstance.DoAnimationTriggerEventLogic(triggerType);
    }
}
