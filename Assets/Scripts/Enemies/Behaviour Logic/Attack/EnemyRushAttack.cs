using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName ="Attack-Rush Attack", menuName = "Combat/ Enemy Logic/ Attack Logic/ Rush Attack")]
public class EnemyRushAttack : EnemyAttackSOBase
{
    
    [SerializeField] private float _timeTillExit = 1f;
    private float _exitTimer;

    [SerializeField] private float _rushTimerMax = 0.4f;
    [SerializeField] private float _rushVelocity = 42f;
    [SerializeField] private AnimationCurve _rushVelocityCurve;
    [SerializeField] private int _attackDamage = 10;
    [SerializeField] private float _restTime = 2f;
    private bool _canRush = true;
    private float _rushTimer;

    private AttackTrigger _attackTrigger;

    public override void Initialize(GameObject gameObject, EnemyScript enemy){
        base.Initialize(gameObject,enemy);

        _attackTrigger = enemy.GetComponentInChildren<AttackTrigger>();
        _attackTrigger.Deactivate();
    }


    public override void DoEnterLogic(){
        enemy.MoveEnemy(Vector3.zero);
        //_canRush=true;
    }
    public override void DoExitLogic(){
        base.DoExitLogic();
    }
    public override void DoFrameUpdateLogic(){
        
        SwitchStateTimer();

        if(_canRush){
            Rush();

            if(_rushTimer >= _rushTimerMax){
                StopAttack();
            } else if(Vector3.Distance(enemy.transform.position,playerTransform.position) <= 0.1f){
                StopAttack();
            }
        } else {
            Vector3 RotateDirection = playerTransform.position - enemy.transform.position;
            RotateDirection.y = 0f;
            RotateDirection = RotateDirection.normalized;

            RotateToDirection(RotateDirection);
        }
        
        
    }
    
    public override void DoPhysicsLogic(){

    }
    public override void DoAnimationTriggerEventLogic(EnemyScript.AnimationTriggerType triggerType){

    }
    public override void ResetValues(){
        
    }

    void Rush(){
        float rushTimerDelta = _rushTimerMax - _rushTimer;
        float rushProgress01 = 1 - rushTimerDelta / _rushTimerMax;

        Vector3 velocity = enemy.transform.forward * (_rushVelocity * _rushVelocityCurve.Evaluate(rushProgress01));
        
        _rushTimer += Time.deltaTime;

        enemy.MoveEnemy(velocity);

        _attackTrigger.Activate(_attackDamage);
    }

    void StopAttack(){
        enemy.MoveEnemy(Vector3.zero);
        _canRush = false;
        _rushTimer = 0f;

        _attackTrigger.Deactivate();

        enemy.RunCoroutine(Rest());
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

    IEnumerator Rest(){
        yield return new WaitForSeconds(_restTime);

        _canRush = true;
    }

    private void RotateToDirection(Vector3 moveDirection){
        Quaternion rotation = Quaternion.LookRotation(moveDirection);
        enemy.RB.MoveRotation(rotation);
    }
}
