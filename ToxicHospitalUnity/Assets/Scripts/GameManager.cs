using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIToggleEvent : UnityEvent<string> { }
public class LoreItemEvent : UnityEvent<LoreItem> { }

public class GameManager : MonoBehaviour
{

    public static bool ShowAudioTranscripts = true;
    public static UIToggleEvent PauseEvent;
    public static UIToggleEvent ToggleIventoryEvent;
    public static LoreItemEvent AddToInventory;



    [HideInInspector]
    public Inventory Inventory;


    void Awake()
    {
        if (PauseEvent == null)
        {
            PauseEvent = new UIToggleEvent();
        }

        if (ToggleIventoryEvent == null)
        {
            ToggleIventoryEvent = new UIToggleEvent();
        }

        if (AddToInventory == null)
        {
            AddToInventory = new LoreItemEvent();
        }

        //Always have the pause menu and inventory are available on load
        /*SceneManager.LoadSceneAsync("PauseScreen", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("InventoryScene", LoadSceneMode.Additive);*/

        Inventory = Inventory.Instance;
        AddToInventory.AddListener(Inventory.AddItem);

    }


    void Update()
    {

        //check for pause
        if (Input.GetButtonDown("Pause"))
        {
            PauseEvent.Invoke("pause");
        }

        //check for inventory open/close
        if (Input.GetButtonDown("Inventory"))
        {
            ToggleIventoryEvent.Invoke("inventory");
        }

    }

    public static void ToggleTranscripts()
    {
        ShowAudioTranscripts = !ShowAudioTranscripts;
    }
}
