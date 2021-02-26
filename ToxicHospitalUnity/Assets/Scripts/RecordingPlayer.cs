using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RecordingPlayer : MonoBehaviour
{
    static RecordingPlayer instance;
    public static RecordingPlayer Instance { get { return instance; } }

    private AudioSource source;
    private TextMeshProUGUI nowPlaying;

    private UISlider popup;
    private AudioLore recording;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        popup = gameObject.GetComponent<UISlider>();
        source = gameObject.GetComponent<AudioSource>();
        nowPlaying = gameObject.GetComponentInChildren<TextMeshProUGUI>();

    }

    public void Activate(AudioLore incoming)
    {
        recording = incoming;
        StartCoroutine("PlayAudio");
    }

    IEnumerator PlayAudio()
    {
        popup.Show();
        source.clip = recording.audioFile;
        nowPlaying.text = "Now playing: " + recording.objectName;
        source.Play();
        yield return new WaitForSeconds(source.clip.length);
        popup.Hide();
    }
}