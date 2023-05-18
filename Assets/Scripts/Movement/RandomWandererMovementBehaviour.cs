using System.Collections;
using Tools;
using UnityEngine;

namespace Movement
{
    [CreateAssetMenu(fileName = "Novo Vagante Aleatório", menuName = "Inimigos/Comportamentos/Movimento/Vagante/Aleatório")]
    public class RandomWandererMovementBehaviour : MovementBehaviour
    {
        [Header("Vagante Aleatório")]
        [SerializeField] private Vector3 _rangeDeMovimento;

        [Header("Perseguição")]
        [SerializeField] private float _distanciaPerseguicao;

        [Header("Descanso")]
        [SerializeField] private bool _descansa;
        [SerializeField] private float _tempoDeDescanso;

        private bool _isResting;
        private bool _isChasing;
        private Transform _playerTransform;
        private Quaternion _currentRotation;

        public override void Initiate()
        {
            base.Initiate();

            _isResting = false;
            _isChasing = false;
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            SetNewTarget();
        }

        public override void Tick()
        {
            base.Tick();

            //if (_isResting) return;

            if (_isChasing)
            {
                ChasePlayer();
            }
            else
            {


                var movementDirection = (_targetPosition - _rigidbody.transform.position).normalized;
                var movementWithSpeed = movementDirection * _velocidade;

                _currentRotation = Quaternion.LookRotation(movementWithSpeed);
                _rigidbody.velocity = movementWithSpeed;
                _rigidbody.rotation = _currentRotation;


                if (!ReachedTargetPosition()) return;

                TryRest();
                TryStartChase();
            }
        }

        private bool ReachedTargetPosition()
        {
            return Vector3.Distance(_rigidbody.transform.position, _targetPosition) < 0.1f;
        }

        private void TryRest()
        {
            //if (_descansa)
            //{
            //    ScriptableObjectCoroutineRunner.Instance.RunCoroutine(RestCoroutine());
            //}

                SetNewTarget();
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
            if (_isChasing) return;

            var randomX = Random.Range(-_rangeDeMovimento.x, _rangeDeMovimento.x);
            var randomZ = Random.Range(-_rangeDeMovimento.z, _rangeDeMovimento.z);

            var randomPosition = new Vector3(randomX, 0, randomZ);

            _targetPosition = _rigidbody.transform.position + randomPosition;
        }

        private void TryStartChase()
        {
            if (_isChasing) return;

            if (Vector3.Distance(_rigidbody.transform.position, _playerTransform.position) <= _distanciaPerseguicao)
            {
                _isChasing = true;
            }
        }


        private void ChasePlayer()
        {
            if (!_isChasing) return;

            var movementDirection = (_playerTransform.position - _rigidbody.transform.position).normalized;
            var movementWithSpeed = movementDirection * _velocidade;

            _currentRotation = Quaternion.LookRotation(movementWithSpeed);
            _rigidbody.velocity = movementWithSpeed;
            _rigidbody.rotation = _currentRotation;


            if (Vector3.Distance(_rigidbody.transform.position, _playerTransform.position) > _distanciaPerseguicao)
            {
                _isChasing = false;
                SetNewTarget();
            }
        }

        public override void Terminate()
        {
            base.Terminate();

            _rigidbody.velocity = Vector3.zero;
        }
    }
}
