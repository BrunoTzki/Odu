using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;

namespace Enemies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] protected HealthSystem _healthSystem;
        [SerializeField] protected PatrolVision _patrolVision;
        [SerializeField] protected float _speed;

        private void OnEnable()
        {
            _patrolVision.OnSeenPlayer += NoticePlayer;
        }

        private void OnDisable()
        {
            _patrolVision.OnSeenPlayer -= NoticePlayer;
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
            
        }
        
        protected virtual void Patrol()
        {
            
        }

        protected virtual void NoticePlayer()
        {
            Debug.Log("Encontrei o jogador!");
        }
        
        protected virtual void Spawn()
        {
            
        }
        
        protected virtual void Attack()
        {
            
        }

        protected virtual void Death()
        {
            
        }
    }
}
