using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    protected List<GameObject> objectsInContact = new List<GameObject>();

    public void OnTriggerEnter(Collider collision)
    {
        this.objectsInContact.Add(collision.gameObject);
    }

    public void OnTriggerExit(Collider collision)
    {
        this.objectsInContact.Remove(collision.gameObject);
    }

    public bool IsOnFloor()
    {
        return this.objectsInContact.Count > 0;
    }
}
