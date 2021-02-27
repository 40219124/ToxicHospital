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

    private UISliderOverride popup;
    private AudioLore recording;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        popup = gameObject.GetComponent<UISliderOverride>();
        source = gameObject.GetComponent<AudioSource>();
        nowPlaying = gameObject.GetComponentInChildren<TextMeshProUGUI>();

    }

    public void Activate(AudioLore incoming)
    {
        recording = incoming;
        if (GameManager.ShowAudioTranscripts)
        {
            UITranscript.Instance.Show(recording);
        }

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
