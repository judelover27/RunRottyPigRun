using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damage = 5;
    void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerCondition condition = other.GetComponent<PlayerCondition>();
            condition.TakePhysicalDamage(damage);
        }
    }
}
