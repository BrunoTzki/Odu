using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public List<IDamageable> _attackedObjects = new List<IDamageable>();
    private int _attackDamage;
    

    public void Activate(int newDamage){
        gameObject.SetActive(true);

        _attackDamage = newDamage;
    }

    public void Deactivate(){
        _attackedObjects.Clear();
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider col) {
        //Debug.Log("hit");
        if(col.TryGetComponent(out IDamageable hit)){
            //Debug.Log("not null");
            if(!_attackedObjects.Contains(hit)){
                _attackedObjects.Add(hit);
                hit.Damage(_attackDamage);
                //Debug.Log(hit);
                
                if(TextSpawner.Instance != null){
                    TextSpawner.Instance.SpawnPopupDamage(_attackDamage,col.transform.position);
                }
            }
        }
            

        
    }
}
