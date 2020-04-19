using UnityEngine;

public class PlayerGrabTrigger : MonoBehaviour
{
    public float GrabRadius = 0.3f;
    public Rigidbody GrabbedObject;
    
    private int grabLayerMask;

    private void Start()
    {
        grabLayerMask = LayerMask.GetMask("Firewood", "Tree");
    }

    public void Grab()
    {
        Collider[] inGrabTrigger = Physics.OverlapSphere(transform.position, GrabRadius, grabLayerMask);
        if (inGrabTrigger.Length > 0)
        {
            Rigidbody target = inGrabTrigger[0].attachedRigidbody;

            if (target.isKinematic)
            {
                if (target.gameObject.layer == LayerMask.NameToLayer("Tree"))
                {
                    // That's static tree, cut it 
                    target.isKinematic = false;
                    target.transform.SetParent(null, true);
                    target.AddForceAtPosition(
                        (transform.position - target.position).normalized * 15,
                        target.centerOfMass + Vector3.up * 3);
                }
            }
            else
            {
                GrabbedObject = target;
                GrabbedObject.isKinematic = true;
                GrabbedObject.transform.SetParent(transform, true);
                GrabbedObject.transform.Translate(-transform.forward*.5f, Space.World);
            }
        }
    }

    public void Release()
    {
        if (GrabbedObject != null)
        {
            GrabbedObject.transform.SetParent(null, true);
            GrabbedObject.isKinematic = false;
            GrabbedObject = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, GrabRadius);
    }
}