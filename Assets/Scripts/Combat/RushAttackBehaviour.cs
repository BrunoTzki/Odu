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
        [SerializeField] private float _velocidadeDeAvanco;
        [SerializeField] private bool _descansaAposAvanco;
        [SerializeField] private float _tempoDeDescanso;

        private float _timer;
        private bool _wannaRush;
        private bool _isDone;
        
        public override void Initiate()
        {
            base.Initiate();

            _timer = 0;
            _wannaRush = false;
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
                TryTerminate();
            }
        }

        private void TryTerminate()
        {
            if (!ReachedTarget()) return;
            
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
                Rush();
            }
            else
            {
                _timer += Time.deltaTime;
            }

            _targetPosition = _target.position;
        }

        private void Rush()
        {
            var movementDirection = (_targetPosition - _rigidbody.transform.position).normalized;
            var movementWithSpeed = movementDirection * _velocidadeDeAvanco;

            _rigidbody.velocity = movementWithSpeed;
        }

        private bool ReachedTarget()
        {
            return Vector3.Distance(_rigidbody.transform.position, _targetPosition) < 0.1f;
        }

        public override void Terminate()
        {
            base.Terminate();
        }
    }
}