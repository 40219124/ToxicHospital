using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private Selectable startOption;

    private void Start()
    {
        startOption.Select();
    }

    public void UIOpenGame()
    {
        BootManager.Instance.GoToGameScene();
    }

    public void UIOpenSettings()
    {
        // ~~~ open settings screen
    }

    public void UIQuitGame()
    {
        Application.Quit();
    }
}
