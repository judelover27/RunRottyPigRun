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
    Condition hunger { get => UIManager.Instance.condition.hunger; }
    Condition stamina { get => UIManager.Instance.condition.stamina; }

    public float noHungerHealthDecay;

    public event Action onTakeDamage;

    
    private void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (hunger.curValue <= 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        if (health.curValue <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
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

    public bool UseStamina(float amount)
    {
        if (stamina.recovering)
        {
            if (stamina.curValue > stamina.recoveringValue)
            {
                stamina.recovering = false;
                return true;
            }

            return false;
        }

        stamina.Subtract(amount);

        if (stamina.curValue <= 0)
        {
            stamina.recovering = true;
        }

        return true;
    }
}
