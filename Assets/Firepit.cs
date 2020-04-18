using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Firepit : MonoBehaviour
{
    public GameObject[] Logs;
    public Light FirepitLight;

    [Range(0, 1)] public float Health = 1;

    private Vector3 lightInitialPosition;

    void Start()
    {
        lightInitialPosition = FirepitLight.transform.position;
        
        UpdateLogsActive();
    }

    private void UpdateLogsActive()
    {
        for (int i = 0; i < Logs.Length; i++)
        {
            Logs[i].SetActive((float) i / Logs.Length < Health);
        }
    }

    void Update()
    {
        FirepitLight.intensity = Mathf.Lerp(1, 4, Health) + Mathf.Sin(Time.time*3 + Random.value * .5f)*.2f;
        FirepitLight.transform.position = lightInitialPosition + new Vector3(
                                              Mathf.Sin(Time.time*2)+Mathf.Sin(Time.time*Mathf.PI*1.2f),
                                              Mathf.Sin(Time.time*2.2f)+Mathf.Sin(Time.time*Mathf.PI*1.1f)+Mathf.Sin(Time.time*3.123f),
                                              Mathf.Sin(Time.time*2.4f)+Mathf.Sin(Time.time*Mathf.PI)
                                          )*.05f;
            

        UpdateLogsActive();
    }
}