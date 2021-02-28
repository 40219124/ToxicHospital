using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSFXController : MonoBehaviour
{
    private static MenuSFXController instance;
    public static MenuSFXController Instance { get { return instance; } }

    [SerializeField]
    private List<AudioSource> audioSources = new List<AudioSource>();
    private int lastSource = -1;

    [SerializeField]
    private AudioClip changeSelected;
    [SerializeField]
    private AudioClip confirmSelected;
    [SerializeField]
    private AudioClip declineMenu;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            enabled = false;
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnSceneLoad(Scene s, LoadSceneMode lsm)
    {
        foreach (GameObject go in s.GetRootGameObjects())
        {
            foreach (Button item in go.GetComponentsInChildren<Button>())
            {
                item.onClick.AddListener(OnAnyButtonClick);
            }
        }
    }

    public void OnItemSelected()
    {
        //Logger.Log("New selection");
        PlayAudioClip(changeSelected);
    }

    private void OnAnyButtonClick()
    {
        //Logger.Log("ClickSelection");
        PlayAudioClip(confirmSelected);
    }

    private void PlayAudioClip(AudioClip clip)
    {
        int audioIndex = GetNextSourceIndex;
        audioSources[audioIndex].clip = clip;
        audioSources[audioIndex].Play();
        lastSource = audioIndex;
    }

    private int GetNextSourceIndex
    {
        get
        {
            return (lastSource + 1) % audioSources.Count;
        }
    }
}
