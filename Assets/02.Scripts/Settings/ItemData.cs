using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Essential,
    Special,
    Consumable
}

public enum ConsumableType
{
    Health,
    Hunger
}

[Serializable]

public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}

[Serializable]

public abstract class Specialty : MonoBehaviour
{
    public abstract void SpecialUtil();
}



[CreateAssetMenu(fileName = "Item", menuName = "new Item")]//새로만들기창에 생성
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType itemType;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;
    [SerializeReference]
    public Specialty[] specialties;

    public void UseSpecial()
    {
        foreach (var specialty in specialties)
        {
            specialty.SpecialUtil(); //  모든 특수 기능 실행
        }
    }

}