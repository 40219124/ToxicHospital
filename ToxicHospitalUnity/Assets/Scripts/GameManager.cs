using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIToggleEvent : UnityEvent<string> { }
public class LoreItemEvent : UnityEvent<LoreItem> { }

public class GameManager : MonoBehaviour
{
    public static UIToggleEvent Pause;
    public static LoreItemEvent AddToInventory;


    [HideInInspector]
    public Inventory Inventory;

    void Awake()
    {
        if (Pause == null)
        {
            Pause = new UIToggleEvent();
        }

        //Always have the pause menu and inventory are available on load
        //SceneManager.LoadSceneAsync("PauseScreen", LoadSceneMode.Additive);
        //SceneManager.LoadSceneAsync("InventoryScene", LoadSceneMode.Additive);

        Inventory = Inventory.Instance;
        AddToInventory.AddListener(Inventory.AddItem);

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
