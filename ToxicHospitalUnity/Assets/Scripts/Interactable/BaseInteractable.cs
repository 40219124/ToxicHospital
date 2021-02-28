using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum eInteractionRequirement
{
    none = 0,
    porter = 1 << 0,
    journalist = 1 << 1,
    eitherCharacter = porter | journalist,
    triggers = 1 << 2
}

[RequireComponent(typeof(Collider2D))]
public class BaseInteractable : MonoBehaviour
{
    [SerializeField]
    protected eInteractionRequirement InteractionRequirements;

    protected int requiredTriggerCount = 0;
    protected int currentTriggerCount = 0;

    protected enum eActivateInteraction { never, afterAnimation, afterDelay, immediately }
    [SerializeField]
    protected eActivateInteraction interactionDelay = eActivateInteraction.afterDelay;
    public bool animationFinished = false;
    public float animationTime = 0.5f;
    protected Timer activationTimer = null;

    [SerializeField]
    protected bool interactionStopsPlayer = true; // ~~~ interaction type enum later to feed back more info to player
    [SerializeField]
    protected ePlayerAction PlayerActionOnInteract = ePlayerAction.none;

    protected bool runningInteraction = false;

    protected enum eTriggerType { none, oneAndDone, toggle, temporary }
    [Space(10.0f)]
    [SerializeField]
    protected eTriggerType triggerType = eTriggerType.none;
    protected bool triggerActive = false;
    [SerializeField]
    protected eActivateInteraction deactivationType = eActivateInteraction.afterDelay;
    [SerializeField]
    protected float deactivationDelay = 1.0f;
    protected Timer deactivationTimer = null;
    [SerializeField]
    protected List<BaseInteractable> triggerTargets = new List<BaseInteractable>();

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        activationTimer = TimerManager.Instance.CreateNewTimer(animationTime);
        deactivationTimer = TimerManager.Instance.CreateNewTimer(deactivationDelay);
        foreach (BaseInteractable bi in triggerTargets)
        {
            bi.AddToTotalTriggers();
        }
    }

    protected virtual void Update()
    {
        //Logger.Log($"Activation progress: {activationTimer.Progress}, Deactivation progress: {deactivationTimer.Progress}");
    }

    public void AddToTotalTriggers()
    {
        requiredTriggerCount++;
    }

    public void AddToCurrentTriggers()
    {
        currentTriggerCount++;
        if (currentTriggerCount == requiredTriggerCount)
        {
            TriggerInteraction(eInteractionRequirement.triggers, transform);
        }
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
        return (triggerType & InteractionRequirements) != eInteractionRequirement.none && currentTriggerCount == requiredTriggerCount && !runningInteraction;
    }

    /// <summary>
    /// Returns true if the player is stopped from moving by this interaction
    /// </summary>
    /// <param name="triggerType"></param>
    /// <param name="trigger"></param>
    /// <returns></returns>
    virtual public ePlayerAction TriggerInteraction(eInteractionRequirement triggerType, Transform trigger /*~~~ trigger class maybe*/)
    {
        if (!CanInteract(triggerType))
        {
            return ePlayerAction.none;
        }
        StartCoroutine(ManageInteraction());
        return (interactionStopsPlayer ? ePlayerAction.waiting : ePlayerAction.none);
    }

    /// <summary>
    /// Controls the flow of an interaction to allow animations to play before the effect is realised
    /// </summary>
    /// <returns></returns>
    virtual protected IEnumerator ManageInteraction()
    {
        runningInteraction = true;
        yield return StartCoroutine(WaitBeforeInteraction(interactionDelay, activationTimer));
        DoInteractionAction();
        if (triggerType != eTriggerType.none && triggerTargets.Count > 0)
        {
            StartCoroutine(ManageTriggering());
        }
        runningInteraction = false;
    }

    /// <summary>
    /// Controls
    /// </summary>
    /// <returns></returns>
    virtual protected IEnumerator ManageTriggering()
    {
        if (triggerActive)
        {
            if (triggerType == eTriggerType.toggle)
            {
                EndTriggering();
            }
            else if (triggerType == eTriggerType.temporary)
            {
                deactivationTimer.Restart();
            }
        }
        else
        {
            ActAsTrigger();
            if (triggerType == eTriggerType.temporary)
            {
                yield return WaitBeforeInteraction(deactivationType, deactivationTimer);
                EndTriggering();
            }
        }
    }

    /// <summary>
    /// Waits for the appropriate time before allowing an interaction to take place
    /// </summary>
    /// <returns></returns>
    virtual protected IEnumerator WaitBeforeInteraction(eActivateInteraction waitType, Timer timer = null)
    {
        switch (waitType)
        {
            case eActivateInteraction.afterDelay:
                timer.Restart();
                while (!timer.IsFinished)
                {
                    yield return null;
                }
                break;
            case eActivateInteraction.afterAnimation:
                while (!animationFinished)
                {
                    yield return null;
                }
                animationFinished = false;
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

    virtual public bool PlayerMustWait
    {
        get { return runningInteraction; }
    }
    virtual public ePlayerAction GetPlayerAction
    {
        get { return PlayerActionOnInteract; }
    }
    /// <summary>
    /// Inform objects triggered by this that it is active
    /// </summary>
    virtual protected void ActAsTrigger()
    {
        triggerActive = true;
        foreach (BaseInteractable bi in triggerTargets)
        {
            bi.AddToCurrentTriggers();
        }
    }
    /// <summary>
    /// Inform objects triggered by this that it is no longer active
    /// </summary>
    virtual protected void EndTriggering()
    {
        triggerActive = false;
        foreach (BaseInteractable bi in triggerTargets)
        {
            bi.SubFromCurrentTriggers();
        }
    }

    /// <summary>
    /// Performs the interaction action
    /// </summary>
    virtual protected void DoInteractionAction()
    {
        Logger.LogWarning("You should be using an overriden method.");
    }

    /// <summary>
    /// Call from event in animation when the interaction should happen
    /// </summary>
    public void SetAnimationFinished()
    {
        animationFinished = true;
    }
}
