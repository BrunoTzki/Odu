using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroCheck : MonoBehaviour
{
    public GameObject PlayerTarget { get; set; }
    private EnemyScript _enemy;

    private void Awake() {
        PlayerTarget = GameObject.FindGameObjectWithTag(UtilityFunctions.PlayerTag);

        _enemy = GetComponentInParent<EnemyScript>();
    }

    private void OnTriggerEnter(Collider col) {
        if(col.gameObject.CompareTag(UtilityFunctions.PlayerTag)){
            _enemy.SetAggroStatus(true);
        }
    }

    private void OnTriggerExit(Collider col) {
        if(col.gameObject.CompareTag(UtilityFunctions.PlayerTag)){
            _enemy.SetAggroStatus(false);
        }
    }
}
