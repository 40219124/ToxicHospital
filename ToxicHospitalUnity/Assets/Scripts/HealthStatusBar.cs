using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthStatusBar : MonoBehaviour
{
    public enum eValueRepresented
    {
        infection,
        health
    }

    public eValueRepresented stat;

    private Image fill;
    private InfectionTracker healthEffects;

    // Start is called before the first frame update
    void Start()
    {
        fill = GetComponentsInChildren<Image>()[0];

        //set fill to max on start for health or min for infection
        fill.fillAmount = (int)stat;

        healthEffects = GameObject.FindObjectOfType<InfectionTracker>();

        //Debug.Log("Fill image = " + fill.name);
    }

    void Update()
    {
        if (stat == eValueRepresented.health)
            fill.fillAmount = healthEffects.GetHealthPercentage();


        if (stat == eValueRepresented.infection)
            fill.fillAmount = healthEffects.GetInfectionPercentage();
    }
}
