using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDummy : MonoBehaviour, IDamageable
{
    public void Damage(int damage)
    {
        Debug.Log("Hit: " + damage);

        //TextSpawner.Instance.SpawnPopupDamage(damage,transform.position);
    }
}
