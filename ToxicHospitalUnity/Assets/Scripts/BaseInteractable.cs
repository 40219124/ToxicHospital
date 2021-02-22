using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum eInteractionRequirement { none = 0, porter = 1 << 0, journalist = 1 << 1, eitherCharacter = porter | journalist }

[RequireComponent(typeof(Collider2D))]
public class BaseInteractable : MonoBehaviour
{
    protected eInteractionRequirement interactionRequirement = eInteractionRequirement.none;
    public eInteractionRequirement InteractionRequirement { get; protected set; }
    [SerializeField]
    protected List<eInteractionRequirement> InteractionRequirements = new List<eInteractionRequirement>();

    protected int requiredTriggerCount = 0;
    protected int currentTriggerCount = 0;

    protected enum eActivateInteraction { never, afterAnimation, afterDelay, immediately }
    [SerializeField]
    protected eActivateInteraction interactionDelay = eActivateInteraction.afterDelay;
    public bool AnimationFinished = false;
    public float AnimationTime = 0.5f;

    [SerializeField]
    protected bool interactionStopsPlayer = true; // ~~~ interaction type enum later to feed back more info to player

    protected bool runningInteraction = false;

    protected void Awake()
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

    /// <summary>
    /// Returns true if the player can trigger this interaction currently
    /// </summary>
    /// <param name="triggerType"></param>
    /// <returns></returns>
    virtual public bool CanInteract(eInteractionRequirement triggerType)
    {
        return (triggerType & interactionRequirement) != eInteractionRequirement.none && currentTriggerCount == requiredTriggerCount && !runningInteraction;
    }

    /// <summary>
    /// Returns true if the player is stopped from moving by this interaction
    /// </summary>
    /// <param name="triggerType"></param>
    /// <param name="trigger"></param>
    /// <returns></returns>
    virtual public bool TriggerInteraction(eInteractionRequirement triggerType, Transform trigger /*~~~ trigger class maybe*/)
    {
        if (!CanInteract(triggerType))
        {
            return false;
        }
        StartCoroutine(ManageInteraction());
        return interactionStopsPlayer;
    }

    /// <summary>
    /// Controls the flow of an interaction to allow animations to play before the effect is realised
    /// </summary>
    /// <returns></returns>
    virtual protected IEnumerator ManageInteraction()
    {
        runningInteraction = true;
        yield return StartCoroutine(WaitBeforeInteraction());
        DoInteractionAction();
        runningInteraction = false;
    }

    /// <summary>
    /// Waits for the appropriate time before allowing an interaction to take place
    /// </summary>
    /// <returns></returns>
    virtual protected IEnumerator WaitBeforeInteraction()
    {
        switch (interactionDelay)
        {
            case eActivateInteraction.afterDelay:
                for (float delay = AnimationTime; delay > 0.0f; delay -= Time.deltaTime)
                {
                    yield return null;
                }
                break;
            case eActivateInteraction.afterAnimation:
                while (!AnimationFinished)
                {
                    yield return null;
                }
                break;
            case eActivateInteraction.never:
                while (true)
                {
                    yield return null;
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Performs the interaction action
    /// </summary>
    virtual protected void DoInteractionAction()
    {
        Debug.LogWarning("You should be using an overriden method.");
    }
}
