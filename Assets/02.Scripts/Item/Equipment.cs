using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour
{
    public Equipment curEquip;
    public Transform equipParent;

    public void EquipNew(ItemData data)
    {
        //unequip
        curEquip = Instantiate(data.dropPrefab, equipParent).GetComponent<Equipment>();
    }

    public void UnEquip()
    {
        if (curEquip != null)
        {
            Destroy(curEquip.gameObject);
            curEquip = null;
        }
    }

}

