using System.Collections;
using Tools;
using UnityEngine;

namespace Movement
{
    [CreateAssetMenu(fileName = "Novo Vagante Aleatório", menuName = "Inimigos/Comportamentos/Vagante/Aleatório")]
    public class RandomWandererMovementBehaviour : MovementBehaviour
    {
        [Header("Vagante Aleatório")] 
        [SerializeField] private Vector3 _rangeDeMovimento;
        
        [Header("Descanso")]
        [SerializeField] private bool _descansa;
        [SerializeField] private float _tempoDeDescanso;

        private bool _isResting;

        public override void Initiate()
        {
            base.Initiate();

            _isResting = false;
            SetNewTarget();
        }
        
        public override void Tick()
        {
            base.Tick();
            
            if (_isResting) return;
            
            var movementDirection = (_targetPosition - _rigidbody.transform.position).normalized;
            var movementWithSpeed = movementDirection * _velocidade;
            
            _rigidbody.velocity = movementWithSpeed;

            if (!ReachedTargetPosition()) return;
            
            TryRest();
        }

        private bool ReachedTargetPosition()
        {
            return Vector3.Distance(_rigidbody.transform.position, _targetPosition) < 0.1f;
        }

        private void TryRest()
        {
            if (_descansa)
            {
                ScriptableObjectCoroutineRunner.Instance.RunCoroutine(RestCoroutine());
            }
            else
            {
                SetNewTarget();
            }
        }

        private IEnumerator RestCoroutine()
        {
            _isResting = true;
            _rigidbody.velocity = Vector3.zero;

            yield return new WaitForSeconds(_tempoDeDescanso);
            
            SetNewTarget();
            _isResting = false;
        }

        private void SetNewTarget()
        {
            var randomX = Random.Range(-_rangeDeMovimento.x, _rangeDeMovimento.x);
            var randomZ = Random.Range(-_rangeDeMovimento.z, _rangeDeMovimento.z);

            var randomPosition = new Vector3(randomX, 0, randomZ);
            
            _targetPosition = _rigidbody.transform.position + randomPosition;
        }
    }
}