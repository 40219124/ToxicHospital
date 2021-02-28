using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossfadeController : MonoBehaviour
{
    AudioSource source0;
    AudioSource source1;
    InfectionTracker healthEffects;

    void Start()
    {
        AudioSource[] sources = gameObject.GetComponents<AudioSource>();
        source0 = sources[0];
        source1 = sources[1];
        healthEffects = GameObject.FindObjectOfType<InfectionTracker>();

        foreach (AudioSource s in sources)
        {
            s.Play();
        }
    }

    public static void Crossfade(AudioSource s1, AudioSource s2, float mixFactor)
    {
        s1.volume = 1.0f - mixFactor;
        s2.volume = mixFactor;
    }

    void Update()
    {
        float infectionPercentage;

        if (healthEffects == null)
        {
            infectionPercentage = 0;
            healthEffects = GameObject.FindObjectOfType<InfectionTracker>();
        }
        else
        {
            infectionPercentage = healthEffects.GetInfectionPercentage();
        }
        Crossfade(source0, source1, infectionPercentage);
    }
}
