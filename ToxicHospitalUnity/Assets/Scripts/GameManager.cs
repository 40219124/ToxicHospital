using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIToggleEvent : UnityEvent<string> { }

public class GameManager : MonoBehaviour
{
    public static UIToggleEvent Pause;

    void Awake()
    {
        if (Pause == null)
        {
            Pause = new UIToggleEvent();
        }
    }
    void Update()
    {

        //check for pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause.Invoke("pause");
        }
    }
}
