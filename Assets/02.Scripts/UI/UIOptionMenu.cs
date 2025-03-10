using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOptionMenu : UIBase
{
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void OnBackButton()
    {
        UIManager.Instance.PopMenu();
        gameObject.SetActive(false);
    }
}
