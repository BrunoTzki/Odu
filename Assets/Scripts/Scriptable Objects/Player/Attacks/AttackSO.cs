using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/ Player/ Attacks")]
public class AttackSO : ScriptableObject
{
    public AnimatorOverrideController AnimatorOV;
    public int Damage;
    
}
