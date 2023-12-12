using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(EnemyScript enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState(){
        enemy.EnemyChaseInstance.DoEnterLogic();
    }

    public override void ExitState(){
        enemy.EnemyChaseInstance.DoExitLogic();
    }

    public override void FrameUpdate(){
        enemy.EnemyChaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate(){
        enemy.EnemyChaseInstance.DoPhysicsLogic();
    }

    public override void AnimationTriggerEvent(EnemyScript.AnimationTriggerType triggerType){
        enemy.EnemyChaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }
}
