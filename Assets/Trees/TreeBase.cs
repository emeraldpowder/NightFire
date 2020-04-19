using UnityEngine;

public class TreeBase : MonoBehaviour
{
    public GameObject TreeObjectPrefab;

    public int TimeToGrowMin = 30;
    public int TimeToGrowMax = 45;

    public bool DestroyIfMaxHpBig;

    private Rigidbody currentTreeObject;
    private Camera mainCamera;

    private void Start()
    {
        currentTreeObject = GetComponentInChildren<Rigidbody>();
        mainCamera = Camera.main;
    }

    [ContextMenu("PositionAllTrees")]
    public void PositionAllTrees()
    {
        foreach (TreeBase treeBase in FindObjectsOfType<TreeBase>())
        {
            treeBase.PositionTree();
        }
    }

    private void PositionTree()
    {
        if (Physics.Raycast(transform.position + Vector3.up * 100, Vector3.down, out var hit, 200,
            LayerMask.GetMask("Ground")))
        {
            transform.position = hit.point - hit.normal * .1f;

            Vector3 normal = hit.normal;

            while (Vector3.Angle(normal, Vector3.up) > 8) normal.y += .1f;
            normal.Normalize();

            Vector3 fwd = Vector3.Cross(normal, Vector3.right).normalized;
            fwd = Quaternion.AngleAxis(Random.Range(0, 360), normal) * fwd;
            transform.rotation = Quaternion.LookRotation(fwd, normal);

            transform.localScale = Vector3.one * Random.Range(0.9f, 1.1f);
        }
    }

    private void Update()
    {
        if (currentTreeObject != null && currentTreeObject.transform.parent != transform)
        {
            // Player has taken the tree
            currentTreeObject = null;
            Invoke(nameof(GrowAnotherTreeObject), Random.Range(TimeToGrowMin, TimeToGrowMax));
        }

        if (DestroyIfMaxHpBig && Firepit.Instance.MaxHealthSeconds >= 200)
        {
            Destroy(this);
            if (currentTreeObject != null) Destroy(currentTreeObject);
        }
    }

    private void GrowAnotherTreeObject()
    {
        if (IsVisibleToCamera())
        {
            Invoke(nameof(GrowAnotherTreeObject), 10);
        }
        else
        {
            currentTreeObject = Instantiate(TreeObjectPrefab, transform).GetComponent<Rigidbody>();
        }
    }

    public bool IsVisibleToCamera()
    {
        Vector3 visTest = mainCamera.WorldToViewportPoint(transform.position);
        return (visTest.x >= 0 && visTest.y >= 0) && (visTest.x <= 1 && visTest.y <= 1) && visTest.z >= 0;
    }
}