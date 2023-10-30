using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilityFunctions 
{
    public static Transform GetClosestTransform(Vector3 currentPosition, Transform[] transforms){
        Transform bestTarget = null;
        float closestDistanceSqrd = Mathf.Infinity;
        foreach(Transform potentialTarget in transforms){
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqrd){
                closestDistanceSqrd = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }
}
