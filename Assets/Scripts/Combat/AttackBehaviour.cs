using BehaviourSystem;
using FMODUnity;
using UnityEngine;

namespace Combat
{
    public class AttackBehaviour : BaseBehaviour
    {
        protected Rigidbody _rigidbody;
        protected Collider _bodyCollider;
        protected GameObject _attackCollider;
        protected StudioEventEmitter _rushSFX;
        protected Transform _target;
        protected Vector3 _targetPosition;

        public virtual void SetData(Rigidbody rigidBody, Collider bodyCollider, GameObject attackCollider, Transform target = null)
        {
            _rigidbody = rigidBody;
            _bodyCollider = bodyCollider;
            _attackCollider = attackCollider;
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