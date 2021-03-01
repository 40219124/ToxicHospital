using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFXManager : MonoBehaviour
{
    private static PlayerSFXManager instance;
    public static PlayerSFXManager Instance { get { return instance; } }

    [SerializeField]
    private List<AudioSource> audioSources = new List<AudioSource>();
    private int lastSource = -1;

    public AudioClip swapCharacter;

    [System.Serializable]
    public class PlayerSFXClips
    {
        public List<AudioClip> jumpStarts = new List<AudioClip>();
        public List<AudioClip> jumpEnds = new List<AudioClip>();
        public List<AudioClip> deathSounds = new List<AudioClip>();
    }

    [SerializeField]
    private PlayerSFXClips carsonClips;
    [SerializeField]
    private PlayerSFXClips millieClips;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            enabled = false;
        }
    }

    public void PlayActionSFX(eInteractionRequirement character, ePlayerAction action, bool actionStart = true)
    {
        if(action == ePlayerAction.swapping)
        {
            PlayAudioClip(swapCharacter);
        }
        else if(character == eInteractionRequirement.porter)
        {
            ChooseSFXFromChar(millieClips, action, actionStart);
        }
        else
        {
            ChooseSFXFromChar(carsonClips, action, actionStart);
        }
    }

    private void ChooseSFXFromChar(PlayerSFXClips character, ePlayerAction action, bool actionStart = true)
    {
        switch (action)
        {
            //case ePlayerAction.pushing:
            // ~~~ case death
            case ePlayerAction.jumping:
                if (actionStart)
                {
                    PlayAudioClip(GetClipFromGroup(character.jumpStarts));
                }
                else
                {
                    PlayAudioClip(GetClipFromGroup(character.jumpEnds));
                }
                break;
            default:
                break;
        }
    }

    private AudioClip GetClipFromGroup(List<AudioClip> clips)
    {
        return clips[Random.Range(0, clips.Count)];
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
