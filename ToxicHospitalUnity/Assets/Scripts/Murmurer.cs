using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Murmurer : MonoBehaviour
{
    public float timeToCircuit = 5.0f;
    public float circuitRipples = 2.0f;
    public float rippleVariation = 0.5f;

    public float murmurerThreshold = 0.5f;

    private float timeBetweenMurmurs = 15.0f;
    private float murmurDelayVariation = 5.0f;
    private float currentTimeToMurmur = 0.0f;

    private float additionalTimeOffset = 0.0f;
    private float directionMultiplier = 1.0f;

    public AudioClip audioClip;
    public AudioSource audioSource;

    private static InfectionTracker infection;

    private void Awake()
    {
        directionMultiplier = Random.Range(0, 2) == 0 ? -1.0f : 1.0f;
        additionalTimeOffset = Random.Range(0.0f, 10.0f);
        timeToCircuit *= 1.0f + Random.Range(-0.5f, 2.0f);
        circuitRipples = Mathf.Pow(Random.Range(0.3f, 1.0f), 2.0f) * 10.0f;
        rippleVariation = Random.Range(0.0f, rippleVariation);
    }

    private void Start()
    {
        ResetMurmurTimer();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        if(infection == null)
        {
            infection = FindObjectOfType<InfectionTracker>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (infection.GetInfectionPercentage() > murmurerThreshold)
        {
            Vector3 position = Vector3.zero;
            position.x = Mathf.Sin(directionMultiplier * 2.0f * Mathf.PI * (Time.time + additionalTimeOffset) / timeToCircuit);
            position.z = Mathf.Cos(directionMultiplier * 2.0f * Mathf.PI * (Time.time + additionalTimeOffset) / timeToCircuit);

            position *= 1.0f + (Mathf.Sin(2.0f * circuitRipples * Mathf.PI * (Time.time + additionalTimeOffset) / timeToCircuit) * rippleVariation);

            transform.localPosition = position;


            currentTimeToMurmur -= Time.deltaTime;
            if(currentTimeToMurmur <= 0.0f)
            {
                ResetMurmurTimer();
                audioSource.Play();
            }
        }
    }

    private void ResetMurmurTimer()
    {
        currentTimeToMurmur = timeBetweenMurmurs + Random.Range(-murmurDelayVariation, murmurDelayVariation);
    }
}
