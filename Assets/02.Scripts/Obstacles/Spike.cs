using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCollider : MonoBehaviour
{
    private SpikeTrap parentTrap; // 부모의 SpikeTrap 참조

    private void Start()
    {
        parentTrap = GetComponentInParent<SpikeTrap>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (parentTrap != null)
        {
            parentTrap.OnChildCollision(collision); // 부모의 함수 호출
        }
    }
}

