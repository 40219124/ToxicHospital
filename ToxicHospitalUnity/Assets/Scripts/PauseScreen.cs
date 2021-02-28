using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    public void UIGoToMainMenu()
    {
        BootManager.Instance.GoToMainMenu();
    }
}
