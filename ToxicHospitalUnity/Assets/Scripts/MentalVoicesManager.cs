using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MentalVoicesManager : MonoBehaviour
{
    private static MentalVoicesManager instance;
    public static MentalVoicesManager Instance { get { return instance; } }

    [SerializeField]
    private List<AudioClip> voiceClips = new List<AudioClip>();

    [SerializeField]
    private Transform murmurPrefab;

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

    private void Start()
    {
        for (int i = 0; i < voiceClips.Count; ++i)
        {
            float threshold = (i + 0.5f) / (voiceClips.Count + 1.0f);

            Transform _object = Instantiate(murmurPrefab, transform);
            Murmurer mur = _object.GetComponent<Murmurer>();
            mur.murmurerThreshold = threshold;
            mur.audioClip = voiceClips[i];
        }
    }
}
