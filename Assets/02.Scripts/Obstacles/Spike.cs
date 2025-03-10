using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private SpikeTrap parentTrap; // 부모의 SpikeTrap 참조

    private void Start()
    {
        parentTrap = GetComponentInParent<SpikeTrap>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (parentTrap != null)
        {
            parentTrap.OnChildTrigger(other); // 부모의 함수 호출
        }
    }
}

