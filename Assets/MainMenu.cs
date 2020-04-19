using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public static bool IsGameStarted;

    public Transform MenuCameraPosition;
    public GameObject DisableOnStart;
    
    private Camera mainCamera;
    private Vector3 playCameraPosition;
    private Quaternion playCameraRotation;

    void Start()
    {
        mainCamera = Camera.main;
        
        if (!IsGameStarted)
        {
            playCameraPosition = mainCamera.transform.position;
            playCameraRotation = mainCamera.transform.rotation;
            
            mainCamera.transform.position = MenuCameraPosition.position;
            mainCamera.transform.rotation = MenuCameraPosition.rotation;
        }
    }

    public void StartGame()
    {
        DisableOnStart.SetActive(false);
        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        for (float i = 0; i < 1; i+=Time.deltaTime/.7f)
        {
            float t = i > 0.5f ? (1-(1-i)*(1-i)*2) : i*i*2;
            mainCamera.transform.position = Vector3.Lerp(MenuCameraPosition.position, playCameraPosition, t);
            mainCamera.transform.rotation = Quaternion.Slerp(MenuCameraPosition.rotation, playCameraRotation, t);
            
            yield return null;
        }
        
        mainCamera.transform.position = playCameraPosition;
        mainCamera.transform.rotation = playCameraRotation;

        IsGameStarted = true;
        
        Destroy(gameObject);
    }
}
