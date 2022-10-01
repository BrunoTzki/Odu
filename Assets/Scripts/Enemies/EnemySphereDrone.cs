using Movement;
using UnityEngine;

namespace Enemies
{
    public class EnemySphereDrone : Enemy
    {
        [SerializeField] private MovementBehaviour _comportamentoDeMovimentoEmPatrulha;
        [SerializeField] private Rigidbody _rigidBody;
        
        private void Start()
        {
            Spawn();
        }
        
        protected override void Tick()
        {
            base.Tick();
        }

        protected override void Patrol()
        {
            base.Patrol();
            
            _comportamentoDeMovimentoEmPatrulha.Tick();
        }

        protected override void Spawn()
        {
            base.Spawn();
            
            _comportamentoDeMovimentoEmPatrulha.SetData(_rigidBody);
            _comportamentoDeMovimentoEmPatrulha.Initiate();
        }

        protected override void Attack()
        {
            base.Attack();
        }

        protected override void Death()
        {
            base.Death();
        }
    }
}