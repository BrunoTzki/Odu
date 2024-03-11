using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStrikingDistanceCheck : MonoBehaviour
{
    public GameObject PlayerTarget { get; set; }
    private EnemyScript _enemy;

    private void Awake() {
        PlayerTarget = GameObject.FindGameObjectWithTag(UtilityFunctions.PlayerTag);

        _enemy = GetComponentInParent<EnemyScript>();
    }

    private void OnTriggerEnter(Collider col) {
        if(col.gameObject == PlayerTarget){
            _enemy.SetStrickingDistanceBool(true);
        }
    }

    private void OnTriggerExit(Collider col) {
        if(col.gameObject == PlayerTarget){
            _enemy.SetStrickingDistanceBool(false);
        }
    }
}
