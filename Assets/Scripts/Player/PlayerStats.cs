using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public event Action<int> OnDamageTaken;

    public void TakeDamage(int damage)
    {
        int amount = -damage;
        OnDamageTaken?.Invoke(amount);
    }
}
