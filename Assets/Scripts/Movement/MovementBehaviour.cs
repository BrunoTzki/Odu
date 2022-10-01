using UnityEngine;

namespace Movement
{
    public class MovementBehaviour : ScriptableObject
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

        public virtual void Initiate()
        {
            
        }
        
        public virtual void Tick()
        {
            
        }
        
        public virtual void Terminate()
        {
            
        }
    }
}