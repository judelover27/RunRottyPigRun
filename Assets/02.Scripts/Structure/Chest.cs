using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : StructureObject
{
    public bool isLock = false;
    public Animator animator;
    public float duration = 1f;
    public float dropRadius = 2f; 
    public float dropForce = 5f; 

    private Coroutine chestCoroutine;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void OnInteract()
    {
        Open();
    }

    public void Open()
    {
        if (chestCoroutine == null)
        chestCoroutine = StartCoroutine(OpenChest());
        
    }

    IEnumerator OpenChest()
    {
        float time = 0f;
        animator.SetTrigger("Open");

        while (time < duration)
        {
            time += Time.deltaTime;
            yield return null;
        }

        foreach (var prefab in data.dropPrefab)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-dropRadius, dropRadius), 
                0,
                Random.Range(-dropRadius, dropRadius) 
            );

            Vector3 spawnPosition = transform.position + randomOffset + Vector3.up * 1f; 
            Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0); // 랜덤한 Y축 회전

            GameObject drop = Instantiate(prefab, spawnPosition, randomRotation);

            Rigidbody rb = drop.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 forceDirection = new Vector3(randomOffset.x, 1, randomOffset.z).normalized;
                rb.AddForce(forceDirection * dropForce, ForceMode.Impulse);
            }
        }

        Destroy(gameObject);
    }
}

