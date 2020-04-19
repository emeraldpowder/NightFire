using System;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform Target;
    
    private Vector3 velocity;

    private void Awake()
    {
        transform.position = Target.position;
    }

    private void Update()
    {
        if (!MainMenu.IsGameStarted) return;
        
        transform.position = Vector3.SmoothDamp(transform.position, Target.position, ref velocity, 0.18f, 40);
    }
}
