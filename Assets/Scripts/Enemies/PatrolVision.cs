using UnityEngine;

namespace Enemies
{
    public class PatrolVision : MonoBehaviour
    {
        public delegate void SeenPlayerDelegate(Transform player);
        public event SeenPlayerDelegate OnSeenPlayer;

        private bool _hasSeenPlayer;
        public bool HasSeenPlayer => _hasSeenPlayer;
        
        private void OnTriggerEnter(Collider other)
        {
            if (_hasSeenPlayer) return;

            if (!other.CompareTag("Player")) return;
            
            _hasSeenPlayer = true;

            OnSeenPlayer?.Invoke(other.transform);
        }
    }
}