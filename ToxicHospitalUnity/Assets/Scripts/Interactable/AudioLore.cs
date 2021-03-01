using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "NewAudioLore", menuName = "ScriptableObjects/AudioLore", order = 1)]
public class AudioLore : LoreItem
{
    public AudioClip audioFile;
    [TextArea(3, 10)]
    public string transcript;

    private void Awake()
    {
        classification = LoreType.AudioLog;
    }
}
