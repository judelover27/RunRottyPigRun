using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEscapeMenu : UIBase
{
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void OnOptionButton()
    {
        UIManager.Instance.PushMenu(UIManager.Instance.menu.optionMenu);
        UIManager.Instance.menu.optionMenu.gameObject.SetActive(true);
    }

    public void OnQuitButton()
    {
        GameManager.Instance.QuitGame();
    }



    public void OnBackButton()
    {
        UIManager.Instance.PopMenu();
        UIManager.Instance.SetCanMove();
        CharacterManager.Instance.Player.controller.LockCursor();
        gameObject.SetActive(false);
    }



}
