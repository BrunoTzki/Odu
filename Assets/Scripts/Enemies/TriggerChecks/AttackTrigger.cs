using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    private int _attackDamage;
    public void Activate(int newDamage){
        gameObject.SetActive(true);

        _attackDamage = newDamage;
    }

    public void Deactivate(){
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider col) {
        //Debug.Log("collision");
        if(col.TryGetComponent(out PlayerStats hit)){
            //Debug.Log("Hit");
            hit.TakeDamage(_attackDamage);
        }
        
    }
}
