using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using Movement;
using UnityEngine;

namespace Enemies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] protected HealthSystem _healthSystem;
        [SerializeField] protected PatrolVision _patrolVision;
        [SerializeField] protected Rigidbody _rigidBody;
        
        [Header("Comportamentos")]
        [SerializeField] protected MovementBehaviour _movimentoDePatrulha;
        [SerializeField] protected AttackBehaviour _ataque;
        
        protected enum CurrentBehaviour
        {
            Spawn,
            NoticePlayer,
            Patrol,
            Attack,
            Death
        }

        protected CurrentBehaviour _currentBehaviour;

        private void OnEnable()
        {
            _patrolVision.OnSeenPlayer += NoticePlayer;
            _ataque.OnTerminate += OnAttackEnded;
        }

        private void OnDisable()
        {
            _patrolVision.OnSeenPlayer -= NoticePlayer;
            _ataque.OnTerminate -= OnAttackEnded;
        }
        
        private void Start()
        {
            Spawn();
        }

        private void Update()
        {
            if (_patrolVision.HasSeenPlayer)
            {
                Tick();
            }
            else
            {
                Patrol();
            }
        }

        protected virtual void Tick()
        {
            switch (_currentBehaviour)
            {
                case CurrentBehaviour.Spawn:
                    break;
                case CurrentBehaviour.NoticePlayer:
                    break;
                case CurrentBehaviour.Patrol:
                    break;
                case CurrentBehaviour.Attack:
                    _ataque.Tick();
                    break;
                case CurrentBehaviour.Death:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        protected virtual void Patrol()
        {
            _currentBehaviour = CurrentBehaviour.Patrol;
            
            _movimentoDePatrulha.Tick();
        }

        protected virtual void NoticePlayer(Transform player)
        {
            _currentBehaviour = CurrentBehaviour.NoticePlayer;
            
            _ataque.SetData(_rigidBody, player);
            Attack();
        }
        
        protected virtual void Spawn()
        {
            _currentBehaviour = CurrentBehaviour.Spawn;
            
            _movimentoDePatrulha.SetData(_rigidBody);
            _movimentoDePatrulha.Initiate();
        }
        
        protected virtual void Attack()
        {
            _currentBehaviour = CurrentBehaviour.Attack;
            
            _ataque.Initiate();
        }
        
        private void OnAttackEnded()
        {
            Patrol();
        }

        protected virtual void Death()
        {
            _currentBehaviour = CurrentBehaviour.Death;
        }
    }
}
