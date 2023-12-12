using UnityEngine;

namespace BehaviourSystem
{
    public class BaseBehaviour : ScriptableObject
    {
        public delegate void BehaviourInitiatedDelegate();
        public delegate void BehaviourTerminatedDelegate();

        public event BehaviourInitiatedDelegate OnInitiate;
        public event BehaviourTerminatedDelegate OnTerminate;
        
        public virtual void Initiate()
        {
            OnInitiate?.Invoke();
        }
    
        public virtual void Tick()
        {
        
        }
    
        public virtual void Terminate()
        {
            OnTerminate?.Invoke();
        }
    }
}