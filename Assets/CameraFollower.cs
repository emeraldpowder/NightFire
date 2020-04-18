using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform Target;
    
    private Vector3 velocity;

    private void Start()
    {
        
    }

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, Target.position, ref velocity, 0.25f);
    }
}
