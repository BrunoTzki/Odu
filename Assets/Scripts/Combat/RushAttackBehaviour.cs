using System.Collections;
using Tools;
using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName = "Novo Ataque de Avanço", menuName = "Inimigos/Comportamentos/Ataque/Avanço")]
    public class RushAttackBehaviour : AttackBehaviour
    {
        [Header("Carga")]
        [SerializeField] private float _tempoDeCarga;

        [Header("Avanço")]
        [SerializeField] private float _velocidadeBaseDeAvanco;
        [SerializeField] private AnimationCurve _curvaDeVelocidadeDeAvanco;
        [SerializeField] private float _distanciaDeAvanco;
        [SerializeField] private bool _descansaAposAvanco;
        [SerializeField] private float _tempoDeDescanso;

        [Header("Perseguição")]
        [SerializeField] private float _distanciaPerseguicao;

        private float _timer;
        private bool _wannaRush;
        private bool _isRushing;
        private bool _isDone;
        private bool _isChasing;
        private Transform _playerTransform;
        private bool _playerDetected;

        private Vector3 _currentDirection;

        public override void Initiate()
        {
            base.Initiate();

            _timer = 0;
            _wannaRush = false;
            _isRushing = false;
            _isDone = false;
            _isChasing = false;
            _playerTransform = null;
            _playerDetected = false;

            _rigidbody.velocity = Vector3.zero;
        }

        public override void Tick()
        {
            base.Tick();

            if (_isDone) return;

            if (!_wannaRush)
            {
                ChargeAndAim();
            }
            else
            {
                if (!_isChasing)
                {
                    Rush();
                    TryTerminate();
                }
                else
                {
                    ChasePlayer();
                    TryTerminate();
                }
            }
        }

        private void TryTerminate()
        {
            if (!HasReachedTarget())
                return;

            _bodyCollider.enabled = true;
            _attackCollider.SetActive(true);
            _isRushing = false;

            if (_descansaAposAvanco)
            {
                _rigidbody.velocity = Vector3.zero;

                ScriptableObjectCoroutineRunner.Instance.RunCoroutine(RestCoroutine());
            }
            else
            {
                Terminate();
            }
        }

        private IEnumerator RestCoroutine()
        {
            _isDone = true;

            yield return new WaitForSeconds(_tempoDeDescanso);

            Terminate();
        }

        private void ChargeAndAim()
        {
            _timer += Time.deltaTime;

            if (_playerTransform != null)
            {
                _currentDirection = (_playerTransform.position - _rigidbody.transform.position).normalized;
                _rigidbody.rotation = Quaternion.LookRotation(_currentDirection);
                _targetPosition = _playerTransform.position;
            }

            if (_wannaRush)
            {
                StartRush();
                _wannaRush = false;
            }

            if (_timer >= _tempoDeCarga)
            {
                _wannaRush = true;
                _timer = 0;
            }
            

            else
            {
                if (!_playerDetected && Vector3.Distance(_rigidbody.transform.position, _target.position) <= _distanciaPerseguicao)
                {
                    _playerTransform = _target;
                    _playerDetected = true;
                }

                if (_playerTransform != null)
                {
                    _targetPosition = _playerTransform.position;
                }
            }

        }



        private void StartRush()
        {
            _bodyCollider.enabled = false;
            _attackCollider.SetActive(true);


            _rigidbody.rotation = Quaternion.LookRotation(_currentDirection);

            var movementDirection = (_targetPosition - _rigidbody.transform.position).normalized;

            _targetPosition = _rigidbody.transform.position + movementDirection * _distanciaDeAvanco;

            _isRushing = true;
        }

        private void Rush()
        {
            _bodyCollider.enabled = false;
            _attackCollider.SetActive(true);

            var movementDirection = (_targetPosition - _rigidbody.transform.position).normalized;
            var distanceToTarget = GetTargetDistance();
            var distanceProgress01 = 1 - distanceToTarget / _distanciaDeAvanco;

            _rigidbody.rotation = Quaternion.LookRotation(_currentDirection);
            _rigidbody.velocity = movementDirection * (_velocidadeBaseDeAvanco * _curvaDeVelocidadeDeAvanco.Evaluate(distanceProgress01));
        }

        private void ChasePlayer()
        {
            _bodyCollider.enabled = true;
            _attackCollider.SetActive(false);
            var movementDirection = (_playerTransform.position - _rigidbody.transform.position).normalized;
            _rigidbody.velocity = movementDirection * _velocidadeBaseDeAvanco;
        }

        private bool HasReachedTarget()
        {
            return GetTargetDistance() < 0.1f;
        }

        private float GetTargetDistance()
        {
            var transformPositionCorrected = new Vector2(_rigidbody.transform.position.x, _rigidbody.transform.position.z);
            var targetPositionCorrected = new Vector2(_targetPosition.x, _targetPosition.z);

            return Vector3.Distance(transformPositionCorrected, targetPositionCorrected);
        }

        public override void Terminate()
        {
            _bodyCollider.enabled = true;
            _attackCollider.SetActive(false);
            base.Terminate();
        }
    }
}
