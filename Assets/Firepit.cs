using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Firepit : MonoBehaviour
{
    public GameObject[] Logs;
    public Light FirepitLight;

    [Range(0, 1)] public float Health = 1;

    private Vector3 lightInitialPosition;
    private ParticleSystem[] LogParticles;
    private Collider[] colliders = new Collider[10];

    private void Start()
    {
        lightInitialPosition = FirepitLight.transform.position;

        LogParticles = Logs.Select(l => l.GetComponentInChildren<ParticleSystem>()).ToArray();
    }

    private void Update()
    {
        Health -= Time.deltaTime / 120; // 120s for full fire burnout

        FirepitLight.intensity = Mathf.Lerp(1, 4, Health) + Mathf.Sin(Time.time * 3 + Random.value * .5f) * .2f;
        FirepitLight.transform.position = lightInitialPosition + new Vector3(
                                              Mathf.Sin(Time.time * 2) + Mathf.Sin(Time.time * Mathf.PI * 1.2f),
                                              Mathf.Sin(Time.time * 2.2f) + Mathf.Sin(Time.time * Mathf.PI * 1.1f) +
                                              Mathf.Sin(Time.time * 3.123f),
                                              Mathf.Sin(Time.time * 2.4f) + Mathf.Sin(Time.time * Mathf.PI)
                                          ) * .05f; // For nice light cracking animation

        UpdateLogs();
        CheckAnyFuelThrowed();
    }

    private void UpdateLogs()
    {
        for (int i = 0; i < Logs.Length; i++)
        {
            bool active = (float) i / Logs.Length < Health;

            Vector3 target = Vector3.up * (active ? 0 : -1.25f);
            Logs[i].transform.localPosition = Vector3.MoveTowards(Logs[i].transform.localPosition, target,
                Time.deltaTime * 2);

            if (active && !LogParticles[i].isPlaying) LogParticles[i].Play();
            else if (!active && LogParticles[i].isPlaying) LogParticles[i].Stop();
        }
    }

    private void CheckAnyFuelThrowed()
    {
        int mask = LayerMask.GetMask("Firewood", "Tree");
        int count = Physics.OverlapSphereNonAlloc(transform.position + Vector3.up, 3f, colliders, mask);
        for (int i = 0; i < count; i++)
        {
            var fireThrowable = colliders[i].attachedRigidbody.GetComponent<FireThrowable>();
            if (fireThrowable != null)
            {
                StartCoroutine(ConsumeObject(fireThrowable, colliders[i]));
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + Vector3.up, 3f);
    }

    public IEnumerator ConsumeObject(FireThrowable target, Collider colliderInFire)
    {
        if (colliderInFire.attachedRigidbody.isKinematic)
        {
            // It's currently grabbed
            yield break;
        }

        Destroy(colliderInFire);
        Destroy(colliderInFire.attachedRigidbody);

        Health += target.HealthAddAmount;
        Debug.Log("+" + target.HealthAddAmount);

        Vector3 moveTo = transform.position + Vector3.down * 12;
        Vector3 velocity = Vector3.zero;

        while (Vector3.Distance(target.transform.position, moveTo) > 1f)
        {
            target.transform.position =
                Vector3.SmoothDamp(target.transform.position, moveTo, ref velocity, .2f, 10);
            yield return null;
        }
        
        Destroy(target.gameObject);
    }
}