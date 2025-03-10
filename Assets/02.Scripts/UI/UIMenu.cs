using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenu : MonoBehaviour
{
    public UIBase escapeMenu;
    public UIBase optionMenu;

    private void Start()
    {
        UIManager.Instance.menu = this;
    }

}
