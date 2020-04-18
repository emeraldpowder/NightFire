using UnityEngine;

public class TreeBase : MonoBehaviour
{
    public Renderer BaseRenderer;
    public GameObject TreeObjectPrefab;

    public int TimeToGrowMin = 30;
    public int TimeToGrowMax = 45;
    
    private Rigidbody currentTreeObject;

    private void Start()
    {
        currentTreeObject = GetComponentInChildren<Rigidbody>();
    }

    private void Update()
    {
        if (currentTreeObject != null && !currentTreeObject.isKinematic)
        {
            // Player has taken the tree
            currentTreeObject = null;
            Invoke(nameof(GrowAnotherTreeObject), Random.Range(TimeToGrowMin, TimeToGrowMax));
        }
    }

    private void GrowAnotherTreeObject()
    {
        Debug.Log(gameObject.name + "  " + BaseRenderer.isVisible);
        if (BaseRenderer.isVisible)
        {
            Invoke(nameof(GrowAnotherTreeObject), 10);
        }
        else
        {
            currentTreeObject = Instantiate(TreeObjectPrefab, transform).GetComponent<Rigidbody>();
        }
    }
}
