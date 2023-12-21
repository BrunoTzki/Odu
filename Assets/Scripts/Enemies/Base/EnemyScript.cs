using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEditorInternal;
using UnityEngine;

public class EnemyScript : MonoBehaviour, IDamageable, IEnemyMoveable, ITriggerCheckable
{
    public event Action<int,int> OnDamaged;
    [SerializeField] private int _maxHealth = 200;
    [SerializeField] private float _rotateDuration = .2f;
    private int _currentHealth;

    private Rigidbody _rb;

    public int MaxHealth { get => _maxHealth; }
    public float RotateDuration { get => _rotateDuration; }
    public int CurrentHealth { get => _currentHealth; }
    public Rigidbody RB { get => _rb; }

    public bool IsAggroed { get; set; }
    public bool IsWithinStrikingDistance { get; set; }

    #region Scriptable Object Variables

    [SerializeField] private EnemyIdleSOBase _enemyIdle;
    [SerializeField] private EnemyChaseSOBase _enemyChase;
    [SerializeField] private EnemyAttackSOBase _enemyAttack;

    public EnemyIdleSOBase EnemyIdleInstance { get; set; }
    public EnemyChaseSOBase EnemyChaseInstance { get; set; }
    public EnemyAttackSOBase EnemyAttackInstance { get; set; }

    #endregion

    #region State Machine Variables

    public EnemyStateMachine StateMachine { get; set; }
    public EnemyIdleState IdleState { get; set; }
    public EnemyChaseState ChaseState { get; set; }
    public EnemyAttackState AttackState { get; set; }

    #endregion

    #region Animation Triggers

    public enum AnimationTriggerType{
        EnemyDamaged,
        PlayFootstepSound,

    }

    private void AnimationTriggerEvent(AnimationTriggerType triggerType){
        StateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);
    }

    #endregion

    private void Awake() {
        EnemyIdleInstance = Instantiate(_enemyIdle);
        EnemyChaseInstance = Instantiate(_enemyChase);
        EnemyAttackInstance = Instantiate(_enemyAttack);

        StateMachine = new EnemyStateMachine();

        IdleState = new EnemyIdleState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);
    }

    private void Start() {
        _currentHealth = _maxHealth;

        _rb = GetComponent<Rigidbody>();

        EnemyIdleInstance.Initialize(gameObject, this);
        EnemyChaseInstance.Initialize(gameObject, this);
        EnemyAttackInstance.Initialize(gameObject, this);

        StateMachine.Initialize(IdleState);
    }

    private void Update() {
        StateMachine.CurrentEnemyState.FrameUpdate();
    }

    private void FixedUpdate() {
        StateMachine.CurrentEnemyState.PhysicsUpdate();
    }

    #region Health Functions
    public void Damage(int damage)
    {
        _currentHealth -= damage;
        OnDamaged?.Invoke(_currentHealth,_maxHealth);

        if(_currentHealth <= 0f){
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
    #endregion


    public void MoveEnemy(Vector3 velocity)
    {
        _rb.velocity = velocity;
    }

    #region Distance Checks

    public void SetAggroStatus(bool isAggroed)
    {
        IsAggroed = isAggroed;
    }

    public void SetStrickingDistanceBool(bool isWithinStrikingDistance)
    {
        IsWithinStrikingDistance = isWithinStrikingDistance;
    }

    #endregion

    public void RunCoroutine(IEnumerator coroutine){
        StartCoroutine(coroutine);
    }
}
