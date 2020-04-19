using UnityEngine;

public class LanternSpawner : MonoBehaviour
{
    public Rigidbody LanternPrefab;

    public int RespawnTime = 30;
    
    private Rigidbody currentLantern;
    private Camera mainCamera;

    private bool lanternSpawnScheduled;
    
    void Start()
    {
        currentLantern = GetComponentInChildren<Rigidbody>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (currentLantern == null && !lanternSpawnScheduled)
        {
            // Player has taken lantern
            lanternSpawnScheduled = true;
            Invoke(nameof(SpawnAnotherLantern), RespawnTime);
        }
    }

    private void SpawnAnotherLantern()
    {
        if (IsVisibleToCamera())
        {
            Invoke(nameof(SpawnAnotherLantern), 10);
        }
        else
        {
            lanternSpawnScheduled = false;
            currentLantern = Instantiate(LanternPrefab, transform).GetComponent<Rigidbody>();
        }
    }

    public bool IsVisibleToCamera()
    {
        Vector3 visTest = mainCamera.WorldToViewportPoint(transform.position);
        return (visTest.x >= 0 && visTest.y >= 0) && (visTest.x <= 1 && visTest.y <= 1) && visTest.z >= 0;
    }
}
