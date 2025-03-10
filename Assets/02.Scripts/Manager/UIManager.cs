using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : Singleton<UIManager>
{
    public UIInventory inventory;
    public UICondition condition;
    public UIMenu menu;

    [SerializeField] public Stack<UIBase> menus;

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

    public void OnEscape(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        ToggleMenu();
    }

    public void ToggleMenu()
    {
        if (menus.Count > 0)
        {
            PopMenu().gameObject.SetActive(false);
            if (menus.Count == 0)
            {
                CharacterManager.Instance.Player.controller.LockCursor();
                SetCanMove();
            }
            Debug.Log($"{menus.Count}");
        }
        else
        {
            if (menu.escapeMenu != null)
            {
                menu.escapeMenu.gameObject.SetActive(true);
                PushMenu(menu.escapeMenu);
                CharacterManager.Instance.Player.controller.UnlockCursor();
                SetCanMove();
                Debug.Log($"{menus.Count}");
            }
        }
    }

    public void OnTab(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (inventory.IsOpen())
            {
                PopMenu();
                inventory.gameObject.SetActive(false);
                CharacterManager.Instance.Player.controller.LockCursor();
                SetCanMove();
                Debug.Log($"{menus.Count}");
            }
            else if (!inventory.IsOpen() && menus.Count == 0)
            {
                inventory.gameObject.SetActive(true);
                PushMenu(inventory);
                CharacterManager.Instance.Player.controller.UnlockCursor();
                SetCanMove();
                Debug.Log($"{menus.Count}");
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
        {
            return menus.Pop();
        }

        return null;
    }

    public void SetCanMove()
    {
        if(menus.Count > 0)
        {
            CharacterManager.Instance.Player.controller.canMove = false;
        }
        else
        {
            CharacterManager.Instance.Player.controller.canMove = true;
        }
    }
}
