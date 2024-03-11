using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    
    public EnemyIdleState(EnemyScript enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState(){
        enemy.EnemyIdleInstance.DoEnterLogic();
    }

    public override void ExitState(){
        enemy.EnemyIdleInstance.DoExitLogic();
    }

    public override void FrameUpdate(){
        enemy.EnemyIdleInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate(){
        enemy.EnemyIdleInstance.DoPhysicsLogic();
    }

    public override void AnimationTriggerEvent(EnemyScript.AnimationTriggerType triggerType){
        enemy.EnemyIdleInstance.DoAnimationTriggerEventLogic(triggerType);
    }

    
}
