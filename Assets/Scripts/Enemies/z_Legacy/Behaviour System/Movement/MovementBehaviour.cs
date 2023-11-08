using BehaviourSystem;
using UnityEngine;

namespace Movement
{
    public class MovementBehaviour : BaseBehaviour
    {
        [Header("Comportamento de Movimento")]
        [SerializeField] protected float _velocidade;
        
        protected Rigidbody _rigidbody;
        protected Transform _target;
        protected Vector3 _targetPosition;

        public virtual void SetData(Rigidbody rigidBody, Transform target = null)
        {
            _rigidbody = rigidBody;
            _target = target;
        }

        public override void Initiate()
        {
            base.Initiate();
        }
        
        public override void Tick()
        {
            base.Tick();
        }
        
        public override void Terminate()
        {
            base.Terminate();
        }
    }
}