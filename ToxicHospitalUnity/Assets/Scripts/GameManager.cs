using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseEvent : UnityEvent<string> { }

public class GameManager : MonoBehaviour
{
    public static PauseEvent Pause;

    void Awake()
    {
        if (Pause == null)
        {
            Pause = new PauseEvent();
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
