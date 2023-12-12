using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public event Action<int> OnDamageTaken;

    [SerializeField] private int _maxHealth = 100;
    private int _currentHealth;

    public int MaxHealth { get{ return _maxHealth; }}
    public int CurrentHealth { get{ return _currentHealth; }}

    private void Awake() {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if(_currentHealth <= 0){
            Debug.Log("Dead");
        }
        OnDamageTaken?.Invoke(damage);
    }
}
