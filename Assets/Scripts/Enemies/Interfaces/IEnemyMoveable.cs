using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyMoveable
{
    Rigidbody RB { get; }

    void MoveEnemy(Vector3 velocity);
}
