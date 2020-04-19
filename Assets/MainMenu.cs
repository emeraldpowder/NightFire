using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static bool IsGameStarted;

    public Transform MenuCameraPosition;
    public GameObject DisableOnStart;
    public GameObject EnableOnStart;

    public Text BestTimeText;
    
    private Camera mainCamera;
    private Vector3 playCameraPosition;
    private Quaternion playCameraRotation;

    private void Start()
    {
        mainCamera = Camera.main;

        int best = PlayerPrefs.GetInt("BestTime", 0);
        int minutes = (int)best / 60;
        int seconds = (int)best % 60;
        BestTimeText.text = $"Best: {minutes:00}:{seconds:00}";

        Cursor.visible = !IsGameStarted;
        if (IsGameStarted)
        {
            DisableOnStart.SetActive(false);
            EnableOnStart.SetActive(true);
        }
        else
        {
            playCameraPosition = mainCamera.transform.position;
            playCameraRotation = mainCamera.transform.rotation;

            mainCamera.transform.position = MenuCameraPosition.position;
            mainCamera.transform.rotation = MenuCameraPosition.rotation;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && DisableOnStart.activeSelf) StartGame();
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
        Cursor.visible = false;
        EnableOnStart.SetActive(true);
        
        Destroy(this);
    }
}
