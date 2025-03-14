﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class ItemSlot : MonoBehaviour
{
    public ItemData itemData;

    public Button button;
    public Image icon;
    public TextMeshProUGUI quantityText;
    private Outline outline;

    public UIInventory inventory;

    public int index;
    public bool equipped = false;
    public int quantity;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        //Debug.Log("onEnable");
        outline.enabled = equipped;
    }

    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = itemData.icon;
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;

        if(outline != null)
        {
            outline.enabled = equipped;
        }
    }

    public void Clear()
    {
        itemData = null;
        icon.gameObject.SetActive(false);
        quantityText.text = string.Empty;
    }

    public void OnClickButton()
    {
        inventory.SelectItem(index);
    }
}
