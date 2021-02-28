using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioListener))]
public class AudioListenerController : MonoBehaviour
{
    private static AudioListenerController instance;
    public static AudioListenerController Instance { get { return instance; } }

    private AudioListener listener;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            gameObject.SetActive(false);
        }

        listener = GetComponent<AudioListener>();
    }

    private void Start()
    {
        SceneManager.sceneLoaded += CheckForListeners;
    }

    private void CheckForListeners(Scene s, LoadSceneMode lsm)
    {
        if (s.name.Equals("InGame"))
        {
            GoToGameScene();
        }
        foreach(GameObject go in s.GetRootGameObjects())
        {
            foreach (AudioListener item in go.GetComponentsInChildren<AudioListener>())
            {
                if (item != listener)
                {
                    item.enabled = false;
                }
            }
        }
    }
    /// <summary>
    /// Call after game loaded
    /// </summary>
    public void GoToGameScene()
    {
        SetParentTo(PlayerController.Instance.transform); // ~~~ set to head level
        transform.Translate(Vector2.up * 2.0f);
    }

    /// <summary>
    /// Call before other scenes unloaded
    /// </summary>
    public void GoToBootScene()
    {
        SetParentTo(BootManager.Instance.transform);
    }

    private void SetParentTo(Transform t)
    {
        transform.SetParent(t);
        transform.localPosition = Vector3.zero;
    }
}
