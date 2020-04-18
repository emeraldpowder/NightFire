using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerGrabTrigger : MonoBehaviour
{
    private HashSet<Rigidbody> inGrabTrigger = new HashSet<Rigidbody>();
    public Rigidbody GrabbedObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Grabbable"))
        {
            inGrabTrigger.Add(other.attachedRigidbody);
            Debug.Log("add");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Grabbable"))
        {
            inGrabTrigger.Remove(other.attachedRigidbody);
            Debug.Log("remove");
        }
    }

    public void Grab()
    {
        if (inGrabTrigger.Count > 0)
        {
            GrabbedObject = inGrabTrigger.First();
            GrabbedObject.isKinematic = true;
            GrabbedObject.transform.SetParent(transform, true);
        }
    }

    public void Release()
    {
        if (GrabbedObject != null)
        {
            GrabbedObject.transform.SetParent(null,true);
            GrabbedObject.isKinematic = false;
            GrabbedObject = null;
        }
    }
}
