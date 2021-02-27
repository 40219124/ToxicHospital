using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfectionTracker : MonoBehaviour
{
    public float InfectionProgress = 0;
    public float MaxInfection = 100;
    public float RecoveryRate;
    public bool DepthBasedInfection;
    public float BloomIntensityMultiplier = 1;
    public float damageRate = 5;


    [SerializeField] private float ratePerSecond = 0;
    [SerializeField] private Material spriteGlow;
    [SerializeField] private Color baseColour;
    [SerializeField] float maxHealth = 100;


    public float currentHealth = 100;
    private bool infecting = false;
    private float depth;
    private Collider2D playerCollider;
    private float intensity;
    private float infectionPercent;

    private void Start()
    {
        playerCollider = gameObject.GetComponent<Collider2D>();
    }

    private float GetDepthInArea(Collider2D trigger)
    {

        Vector3 distanceToCentre = trigger.transform.position - transform.position;
        //Debug.Log("Distance to centre = " + distanceToCentre);

        //get the edge of the player collider to the centre of the trigger collider
        //float actualDistanceToCentre = Mathf.Abs(distanceToCentre.x) - (playerCollider.bounds.size.x / 2);

        //bounding size of trigger minus the distance to centre plus half of the bounding size gives the remaining distance to the outside of the trigger box on any axis from the deepest side
        float lateralDepth = (trigger.bounds.size.x / 2) - (Mathf.Abs(distanceToCentre.x) - playerCollider.bounds.size.x);
        float verticallDepth = (trigger.bounds.size.y / 2) - (Mathf.Abs(distanceToCentre.y) - playerCollider.bounds.size.y);

        //Debug.Log("Depth x in trigger = " + lateralDepth);
        //Debug.Log("Depth y in trigger = " + verticallDepth);

        if (lateralDepth < 0 && verticallDepth < 0)
        {
            //player is not inside of trigger
            return 0;
        }
        else if (lateralDepth > verticallDepth)
        {
            //player is deepest horizontally
            return lateralDepth;
        }
        else
        {
            //player is deepest vertically
            return verticallDepth;
        }

    }

    private void IncrementInfection()
    {
        if (infecting)
        {
            if (DepthBasedInfection)
            {
                InfectionProgress += depth * Time.deltaTime;
            }
            else
            {
                InfectionProgress += ratePerSecond * Time.deltaTime;
            }

        }
        else
        {
            if (InfectionProgress > 0)
            {
                InfectionProgress -= RecoveryRate * Time.deltaTime;
            }
        }

        infectionPercent = InfectionProgress / MaxInfection;
    }

    private void IncrementBloom()
    {
        intensity = Mathf.Max(1, infectionPercent * BloomIntensityMultiplier);
        spriteGlow.SetColor("_HDR", baseColour * Mathf.Pow(2, intensity));
    }

    private void TakeDamage()
    {
        if (infectionPercent.Equals(1.0f))
        {
            currentHealth -= damageRate * Time.deltaTime;
        }

    }

    private void EvaluateDeath()
    {
        if (currentHealth <= 0)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            CheckpointInteractable.LoadCheckpoint(this);
        }
    }

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }


    //OnTriggerStay used so that the rate will keep being added if entering another nearby zone before leaving current one
    void OnTriggerStay2D(Collider2D other)
    {


        if (other.tag == "Infection")
        {
            infecting = true;

            if (DepthBasedInfection)
            {
                depth = GetDepthInArea(other);
            }
        }
    }

    //TODO change from boolean check to count of triggers entered and exited to tell if still being affected.
    void OnTriggerExit2D(Collider2D other)
    {
        depth = 0;

        if (other.tag == "Infection")
        {
            infecting = false;
        }
    }


    void Update()
    {

        IncrementInfection();
        IncrementBloom();
        TakeDamage();
        EvaluateDeath();

    }
}
