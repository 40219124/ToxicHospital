using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootstepPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    private float lastXPos;
    private float currentSpeed = 0.0f;
    [SerializeField]
    private float spaceBetweenSteps = 1.0f;
    private float toNextStep;

    [SerializeField]
    private List<AudioClip> steps = new List<AudioClip>();
    private int lastStepIndex = -1;

    private GroundChecker groundChecker;

    private void Awake()
    {
        toNextStep = spaceBetweenSteps;
        lastXPos = transform.position.x;

        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        groundChecker = transform.parent.GetComponentInChildren<GroundChecker>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovementUpdate();
        StepUpdate();
    }

    void CalculateMovementUpdate()
    {
        if (groundChecker == null || groundChecker.PlayerCanJump)
        {
            currentSpeed = Mathf.Abs(transform.position.x - lastXPos);
            lastXPos = transform.position.x;
        }
        else
        {
            currentSpeed = 0.0f;
        }
    }

    void StepUpdate()
    {
        toNextStep -= currentSpeed;
        if (toNextStep <= 0.0f)
        {
            toNextStep += spaceBetweenSteps;
            PlayStep();
        }
    }

    void PlayStep()
    {
        if(steps.Count == 0)
        {
            return;
        }
        AudioClip stepClip = steps[0];
        if(steps.Count > 1)
        {
            int randIndex = Random.Range(0, steps.Count);
            while(randIndex == lastStepIndex)
            {
                randIndex = Random.Range(0, steps.Count);
            }

            stepClip = steps[randIndex];
            lastStepIndex = randIndex;
        }

        audioSource.clip = stepClip;
        audioSource.Play();
    }
}
