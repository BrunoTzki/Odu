using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/ Tools")]
public class TestToolSO : BaseToolSO
{
    [SerializeField] private List<AttackSO> _combo;
    public override List<AttackSO> Combo => _combo;

    [SerializeField] private GameObject _toolWeapon;
    public override GameObject ToolWeapon => _toolWeapon;

    public override void Skill1()
    {
    }

    public override void Skill2()
    {
    }
}
