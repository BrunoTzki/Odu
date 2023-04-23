using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Ataque")]
    [SerializeField] int damageAmount;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Colidiu");
            other.GetComponentInParent<PlayerStats>().TakeDamage(damageAmount);
        }
    }

}
