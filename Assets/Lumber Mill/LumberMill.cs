using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class LumberMill : MonoBehaviour
{
    public Rigidbody FirewoodPrefab;
    public Transform[] FirewoodSpawns;

    public AudioSource WorkSound;

    private Collider[] colliders = new Collider[10];

    private void Update()
    {
        if (!MainMenu.IsGameStarted) return;

        int mask = LayerMask.GetMask("Tree");
        int count = Physics.OverlapBoxNonAlloc(transform.position + new Vector3(6, 4, 0), new Vector3(15, 30, 20) / 4,
            colliders, Quaternion.identity, mask);
        for (int i = 0; i < count; i++)
        {
            var tree = colliders[i].attachedRigidbody;
            if (tree != null && !tree.isKinematic)
            {
                StartCoroutine(ConsumeTree(tree));
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position + new Vector3(6, 4, 0), new Vector3(15, 30, 20) / 2);
    }

    private IEnumerator ConsumeTree(Rigidbody treeBody)
    {
        var tree = treeBody.gameObject;
        Destroy(treeBody);

        Vector3 moveTo = transform.position + new Vector3(-3, 2, 0);
        Vector3 velocity = Vector3.zero;
        float scaleVelocity = 0;
        
        WorkSound.Play();

        while (Vector3.Distance(tree.transform.position, moveTo) > 1f)
        {
            tree.transform.position =
                Vector3.SmoothDamp(tree.transform.position, moveTo, ref velocity, .2f, 10);
            tree.transform.localScale =
                Mathf.SmoothDamp(tree.transform.localScale.x, 0, ref scaleVelocity, 1.5f) * Vector3.one;
            yield return null;
        }

        Destroy(tree.gameObject);

        Rigidbody[] firewoods = new Rigidbody[FirewoodSpawns.Length];
        for (int i = 0; i < FirewoodSpawns.Length; i++)
        {
            Quaternion rotation = Quaternion.Euler(0, Random.Range(-20, 20f), 0);
            firewoods[i] = Instantiate(FirewoodPrefab, FirewoodSpawns[i].position, rotation);
            firewoods[i].isKinematic = true;
        }

        for (float i = 0; i < 1; i += Time.deltaTime / 1.4f)
        {
            for (int j = 0; j < firewoods.Length; j++)
            {
                firewoods[j].MovePosition(firewoods[j].position + Time.deltaTime * 7 * Vector3.left);
            }

            yield return null;
        }

        for (int j = 0; j < firewoods.Length; j++)
        {
            firewoods[j].isKinematic = false;
        }
    }
}