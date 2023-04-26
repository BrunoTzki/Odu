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

        private float _timer;
        private bool _wannaRush;
        private bool _isRushing;
        private bool _isDone;

        public override void Initiate()
        {
            base.Initiate();

            _timer = 0;
            _wannaRush = false;
            _isRushing = false;
            _isDone = false;
            
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
                Rush();
                TryTerminate();
            }
        }

        private void TryTerminate()
        {
            if (!HasReachedTarget()) return;
            
            _bodyCollider.enabled = true;
            _attackCollider.enabled = false;
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
            if (_timer >= _tempoDeCarga)
            {
                _wannaRush = true;
                StartRush();
            }
            else
            {
                _timer += Time.deltaTime;
                _targetPosition = _target.position;
            }
        }

        private void StartRush()
        {
            _bodyCollider.enabled = false;
            _attackCollider.enabled = true;


            var movementDirection = (_targetPosition - _rigidbody.transform.position).normalized;
            _targetPosition = _rigidbody.transform.position + movementDirection * _distanciaDeAvanco;
            
            _isRushing = true;
        }
        
        private void Rush()
        {
            var movementDirection = (_targetPosition - _rigidbody.transform.position).normalized;
            var distanceToTarget = GetTargetDistance();
            var distanceProgress01 = 1 - distanceToTarget / _distanciaDeAvanco;
            
            _rigidbody.velocity = movementDirection * (_velocidadeBaseDeAvanco * _curvaDeVelocidadeDeAvanco.Evaluate(distanceProgress01));
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
            _attackCollider.enabled = false;
            base.Terminate();
        }
    }
}