using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void TakePhysicalDamage(int damage);
}

public class PlayerCondition : MonoBehaviour, IDamagable
{

    Condition health { get => UIManager.Instance.condition.health; }
    Condition stamina { get => UIManager.Instance.condition.stamina; }

    public float noHungerHealthDecay;

    public event Action onTakeDamage;

    
    private void Update()
    {

        if(!CharacterManager.Instance.Player.controller.isRun)
        stamina.Add(stamina.passiveValue * Time.deltaTime);


        if (health.curValue <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    /*public void Eat(float amount)
    {
        hunger.Add(amount);
    }*/

    public void refillStamina(float amount)
    {
        stamina.Add(amount);
    }

    public void Die()
    {
        Debug.Log("player die");
    }

    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }



}
