using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    protected List<GameObject> objectsInContact;

    public void OnCollisionEnter(Collision collision)
    {
        this.objectsInContact.Add(collision.gameObject);
    }

    public void OnCollisionExit(Collision collision)
    {
        this.objectsInContact.Remove(collision.gameObject);
    }

    public bool IsOnFloor()
    {
        return objectsInContact.Count > 0;
    }
}
