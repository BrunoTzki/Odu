using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName = "New Health Data", menuName = "Combat/Health Data")]
    public class HealthData : ScriptableObject
    {
        [SerializeField] private int _initialHealthPoints;
        [SerializeField] private int _increasedHealthPoints;
        [SerializeField] private int _bonusHealthPoints;
        
        private int _currentHealthPoints;

        public int InitialHealthPoints => _initialHealthPoints;
        public int IncreasedHealthPoints => _increasedHealthPoints;
        public int BonusHealthPoints => _bonusHealthPoints;
        public int CurrentHealthPoints => _currentHealthPoints;

        public void Setup()
        {
            _currentHealthPoints = _initialHealthPoints + _increasedHealthPoints + _bonusHealthPoints;
        }

        public void TakeDamage(int damageAmount)
        {
            _currentHealthPoints -= damageAmount;
        }

        public void Heal(int healAmount)
        {
            _currentHealthPoints += healAmount;
        }
    }
}