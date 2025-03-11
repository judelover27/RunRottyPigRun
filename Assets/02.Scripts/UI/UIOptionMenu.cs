using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOptionMenu : UIBase
{
    public Dropdown cameraDropdown; 
    public Camera thirdPersonCamera;
    public Camera firstPersonCamera;
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void OnBackButton()
    {
        UIManager.Instance.PopMenu();
        gameObject.SetActive(false);
    }


    public void ChangeCameraMode(int index)
    {
        switch (index)
        {
            case 0: 
                thirdPersonCamera.gameObject.SetActive(true);
                firstPersonCamera.gameObject.SetActive(false);
                CharacterManager.Instance.Player.controller.camYRot = -15f;
                CharacterManager.Instance.Player.interaction.camera = CharacterManager.Instance.Player.interaction.cameraThird;
                break;

            case 1: 
                thirdPersonCamera.gameObject.SetActive(false);
                firstPersonCamera.gameObject.SetActive(true);
                CharacterManager.Instance.Player.controller.camYRot = 0f;
                CharacterManager.Instance.Player.interaction.camera = CharacterManager.Instance.Player.interaction.cameraFirst;
                break;
        }
    }
}
