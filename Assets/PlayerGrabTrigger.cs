using UnityEngine;

public class PlayerGrabTrigger : MonoBehaviour
{
    public float GrabRadius = 0.3f;
    public Rigidbody GrabbedObject;

    public GameObject LanternPickedPrefab;

    public AudioSource ChopTreeSound;
    public AudioSource GrabSound;
    public AudioSource GrabShroomSound;
    public AudioSource LanternPickSound;

    private int grabLayerMask;

    private void Start()
    {
        grabLayerMask = LayerMask.GetMask("Firewood", "Tree", "Lantern", "Mushroom");
    }

    public void Grab()
    {
        Collider[] inGrabTrigger = Physics.OverlapSphere(transform.position, GrabRadius, grabLayerMask);
        if (inGrabTrigger.Length > 0)
        {
            Rigidbody target = inGrabTrigger[0].attachedRigidbody;

            if (target.gameObject.layer == LayerMask.NameToLayer("Tree") && target.isKinematic)
            {
                // That's static tree, cut it 
                target.isKinematic = false;
                target.transform.SetParent(null, true);
                target.AddForceAtPosition(
                    (transform.position - target.position).normalized * 15,
                    target.centerOfMass + Vector3.up * 3);

                ChopTreeSound.Play();
            }
            else if (target.gameObject.layer == LayerMask.NameToLayer("Lantern"))
            {
                Firepit.Instance.AddHealth(0.16f, 0);

                Destroy(target.gameObject);
                Instantiate(LanternPickedPrefab, target.position, Quaternion.identity);

                LanternPickSound.Play();
            }
            else if (!target.isKinematic || target.gameObject.layer == LayerMask.NameToLayer("Mushroom"))
            {
                GrabbedObject = target;
                GrabbedObject.isKinematic = true;
                GrabbedObject.transform.SetParent(transform, true);
                GrabbedObject.transform.Translate(-transform.forward * .5f, Space.World);

                if (target.gameObject.layer == LayerMask.NameToLayer("Mushroom")) GrabShroomSound.Play();
                else GrabSound.Play();
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