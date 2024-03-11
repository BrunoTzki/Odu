using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseToolSO : ScriptableObject
{
    public abstract List<AttackSO> Combo { get; }

    public abstract GameObject ToolWeapon { get; }

    public abstract void Skill1();

    public abstract void Skill2();
}
