using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;

namespace Enemies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] protected HealthSystem _healthSystem;
        [SerializeField] protected float _speed;

        private void Update()
        {
            Tick();
        }

        protected virtual void Tick()
        {
            
        }
    }
}
