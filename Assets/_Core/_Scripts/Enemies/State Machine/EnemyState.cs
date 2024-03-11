using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class EnemyState 
{
    protected EnemyScript enemy;
    protected EnemyStateMachine enemyStateMachine;

    public EnemyState(EnemyScript enemy, EnemyStateMachine enemyStateMachine){
        this.enemy = enemy;
        this.enemyStateMachine = enemyStateMachine;
    }

    public virtual void EnterState(){}

    public virtual void ExitState(){}

    public virtual void FrameUpdate(){}

    public virtual void PhysicsUpdate(){}

    public virtual void AnimationTriggerEvent(EnemyScript.AnimationTriggerType triggerType){}
}
