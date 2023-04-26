using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Ataque")]

    [SerializeField] protected Collider _attackCollider;
    [SerializeField] int damageAmount;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInParent<PlayerStats>().TakeDamage(damageAmount);
            _attackCollider.enabled = false;
        }
    }
}
