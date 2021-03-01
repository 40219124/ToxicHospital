using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private Selectable startOption;
    public static UIToggleEvent PauseEvent;

    void Awake()
    {
        if (PauseEvent == null)
        {
            PauseEvent = new UIToggleEvent();
        }
    }

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
        PauseEvent.Invoke("pause");
    }

    public void UIQuitGame()
    {
        Application.Quit();
    }
}
