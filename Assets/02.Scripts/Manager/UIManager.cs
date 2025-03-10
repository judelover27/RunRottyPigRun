using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : Singleton<UIManager> 
{
    public UIInventory inventory;
    public UICondition condition;
    public UIMenu menu;

    [SerializeField]public Stack<UIBase> menus;

    protected override void Awake()
    {
        base.Awake();

        if (menus == null)
            menus = new Stack<UIBase>();

    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnInputAction(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            ToggleMenu();
        }
    }
    public void ToggleMenu()
    {
        if (menus.Count > 0)
        {
            PopMenu().gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            if (menu.escapeMenu != null)
            {
                menu.escapeMenu.gameObject.SetActive(true);
                PushMenu(menu.escapeMenu);
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    public void PushMenu(UIBase menu)
    {
        menus.Push(menu);
    }

    public UIBase PopMenu()
    {
        if (menus.Count > 0)
            return menus.Pop();

        return null;
    }
}
