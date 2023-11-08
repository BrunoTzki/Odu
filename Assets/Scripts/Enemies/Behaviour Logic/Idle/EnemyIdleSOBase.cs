using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleSOBase : ScriptableObject
{
    protected EnemyScript enemy;
    protected Transform transform;
    protected GameObject gameObject;

    protected Transform playerTransform;

    public virtual void Initialize(GameObject gameObject, EnemyScript enemy){
        this.gameObject = gameObject;
        transform = gameObject.transform;
        this.enemy = enemy;

        playerTransform = GameObject.FindGameObjectWithTag(UtilityFunctions.PlayerTag).transform;
    }

    public virtual void DoEnterLogic(){ }
    public virtual void DoExitLogic(){ ResetValues(); }
    public virtual void DoFrameUpdateLogic(){
        if(enemy.IsAggroed){
            enemy.StateMachine.ChangeState(enemy.ChaseState);
        }
    }
    public virtual void DoPhysicsLogic(){ }
    public virtual void DoAnimationTriggerEventLogic(EnemyScript.AnimationTriggerType triggerType){ }
    public virtual void ResetValues(){ }
    
}
