using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ExposeMixer : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private string ChannelString;

    private string[] validChannels = new string[3] { "MasterVolume", "MusicVolume", "SFXVolume" };


    public void SetChannelLevel(float sliderValue)
    {
        //if the user-defined channel string is in the list of valid channel string then
        if (System.Array.Exists(validChannels, element => element == ChannelString))
        {
            //represents slider with logarithmic scale like it is in the audio mixer system and sets it accordingly
            mixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        }
    }
}