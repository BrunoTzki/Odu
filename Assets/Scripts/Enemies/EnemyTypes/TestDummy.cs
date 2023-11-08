using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDummy : MonoBehaviour, IDamageable
{
    public int MaxHealth { get => throw new System.NotImplementedException();}
    public int CurrentHealth { get => throw new System.NotImplementedException();}

    public void Damage(int damage)
    {
        Debug.Log("Hit: " + damage);

        //TextSpawner.Instance.SpawnPopupDamage(damage,transform.position);
    }

    public void Die()
    {
        throw new System.NotImplementedException();
    }
}
