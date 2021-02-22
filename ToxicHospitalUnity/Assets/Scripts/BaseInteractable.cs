using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum eInteractionRequirement { none = 0, porter = 1 << 0, journalist = 1 << 1, eitherCharacter = porter | journalist }

[RequireComponent(typeof(Collider2D))]
public class BaseInteractable : MonoBehaviour
{
    private eInteractionRequirement interactionRequirement = eInteractionRequirement.none;
    public eInteractionRequirement InteractionRequirement { get; private set; }
    [SerializeField]
    private List<eInteractionRequirement> InteractionRequirements = new List<eInteractionRequirement>();

    private int requiredTriggerCount = 0;
    private int currentTriggerCount = 0;

    private void Awake()
    {
        foreach (eInteractionRequirement ir in InteractionRequirements)
        {
            interactionRequirement |= ir;
        }
    }

    public void AddToTotalTriggers()
    {
        requiredTriggerCount++;
    }

    public void AddToCurrentTriggers()
    {
        currentTriggerCount++;
    }

    public void SubFromCurrentTriggers()
    {
        currentTriggerCount--;
    }


    virtual public bool CanInteract(eInteractionRequirement triggerType)
    {
        return (triggerType & interactionRequirement) != eInteractionRequirement.none && currentTriggerCount == requiredTriggerCount;
    }

    virtual public void TriggerInteraction(eInteractionRequirement triggerType, Transform trigger /*~~~ trigger class maybe*/)
    {
        if (!CanInteract(triggerType))
        {
            return;
        }
        Debug.LogWarning("You should be using an overriden method.");
    }
}
