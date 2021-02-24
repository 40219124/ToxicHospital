using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIToggleEvent : UnityEvent<string> { }
public class LoreItemEvent : UnityEvent<LoreItem> { }

public class GameManager : MonoBehaviour
{
    public static UIToggleEvent PauseEvent;
    public static UIToggleEvent ToggleIventoryEvent;
    public static LoreItemEvent AddToInventory;


    [HideInInspector]
    public Inventory Inventory;

    public List<LoreItem> TEST = new List<LoreItem>();

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
        SceneManager.LoadSceneAsync("PauseScreen", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("InventoryScene", LoadSceneMode.Additive);

        Inventory = Inventory.Instance;
        AddToInventory.AddListener(Inventory.AddItem);





    }
    void Update()
    {

        //check for pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseEvent.Invoke("pause");
        }

        //check for inventory open/close
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleIventoryEvent.Invoke("inventory");
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //strictly for testing purposes DELETE AFTERWARDS
            Debug.LogWarning("Remove this testing code when finished.");
            foreach (LoreItem t in TEST)
            {
                AddToInventory.Invoke(t);
            }
        }
    }
}
