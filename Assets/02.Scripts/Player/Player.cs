﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;
    public PlayerAnimationHandler animationHandler;
    public PlayerInteraction interaction;
    public HeadGear headGear;
    public Weapon weapon;

    public ItemData itemData;
    public Action addItem;

    public Transform dropPosition;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        animationHandler = GetComponent<PlayerAnimationHandler>();
        interaction = GetComponent<PlayerInteraction>();
    }
}