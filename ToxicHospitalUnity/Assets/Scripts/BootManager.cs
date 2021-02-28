using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public enum eScenes { none, boot, mainMenu, game, pause, inventory, overlay }

public class BootManager : MonoBehaviour
{
    private static BootManager instance = null;
    public static BootManager Instance { get { return instance; } }

    private EventSystem bootSystem;

    List<Scene> openScenes = new List<Scene>();
    Dictionary<eScenes, string> sceneNames = new Dictionary<eScenes, string>
    {
        {eScenes.boot, "Boot"},
        {eScenes.mainMenu, "StartScreen"},
        {eScenes.game, "InGame"}, // ~~~ Real game scene please
        {eScenes.overlay, "Overlay"},
        {eScenes.pause, "PauseScreen"},
        {eScenes.inventory, "InventoryScene"}
    };

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        bootSystem = GetComponentInChildren<EventSystem>();

        GoToMainMenu();
    }

    public void GoToMainMenu()
    {
        ClearOpenScenes();
        LoadAdditiveScene(eScenes.mainMenu);
    }

    public void GoToGameScene()
    {
        ClearOpenScenes();
        LoadAdditiveScene(eScenes.game);
        LoadAdditiveScene(eScenes.overlay);
        LoadAdditiveScene(eScenes.pause);
        LoadAdditiveScene(eScenes.inventory);
    }

    void ClearOpenScenes()
    {
        for (int i = openScenes.Count - 1; i >= 0; --i)
        {
            SceneManager.UnloadSceneAsync(openScenes[i]);
            openScenes.RemoveAt(i);
        }
    }

    public bool SceneLoaded(eScenes scene)
    {
        return openScenes.Find(s => s.name.Equals(sceneNames[scene])).name != null;
    }

    public void LoadAdditiveScene(eScenes scene)
    {
        if (!SceneLoaded(scene))
        {
            SceneManager.LoadSceneAsync(sceneNames[scene], LoadSceneMode.Additive);
        }
    }

    public void UnloadScene(eScenes scene)
    {
        if (SceneLoaded(scene))
        {
            int i = openScenes.FindIndex(x => x.name.Equals(sceneNames[scene]));
            SceneManager.UnloadSceneAsync(sceneNames[scene]);
            openScenes.RemoveAt(i);
        }
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        openScenes.Add(scene);

        // ~~~ maybe some stuff relating to cameras or event systems

        EventSystem[] systems = FindObjectsOfType<EventSystem>();
        foreach (EventSystem es in systems)
        {
            if (es != bootSystem)
            {
                es.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
