using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerInteractor : MonoBehaviour
{
    public eInteractionRequirement CurrentCharacter = eInteractionRequirement.journalist;
    private List<BaseInteractable> interactables = new List<BaseInteractable>();
    private BaseInteractable bestInteractable;
    // ~~~ reference to player controller probably

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (interactables.Count == 0)
        {
            bestInteractable = null;
        }
        else if (interactables.Count == 1)
        {
            bestInteractable = interactables[0];
        }
        else if (interactables.Count > 1)
        {
            float shortestSq = ((Vector2)transform.position - (Vector2)interactables[0].transform.position).sqrMagnitude;
            int shortestI = 0;
            for (int i = 1; i < interactables.Count; ++i)
            {
                float currentSq = ((Vector2)transform.position - (Vector2)interactables[i].transform.position).sqrMagnitude;
                if (currentSq < shortestSq)
                {
                    shortestSq = currentSq;
                    shortestI = i;
                }
            }
            bestInteractable = interactables[shortestI];
        }
    }

    public bool TryInteractInput(eInteractionRequirement currentCharacter, ref BaseInteractable outInteractable, out ePlayerAction outAction)
    {
        if (outInteractable == null)
        {
            outInteractable = bestInteractable;
        }
        return TryInteract(currentCharacter, outInteractable, out outAction);
    }

    private bool TryInteract(eInteractionRequirement currentCharacter, BaseInteractable interactable, out ePlayerAction outAction)
    {
        if (interactable != null && interactable.CanInteract(currentCharacter))
        {
            outAction = interactable.TriggerInteraction(CurrentCharacter, transform);
            return true;
        }
        outAction = ePlayerAction.none;
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BaseInteractable bi = collision.GetComponent<BaseInteractable>();
        if (bi != null)
        {
            if (!interactables.Contains(bi))
            {
                interactables.Add(bi);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        BaseInteractable bi = collision.GetComponent<BaseInteractable>();
        if (bi != null)
        {
            if (interactables.Contains(bi))
            {
                interactables.Remove(bi);
            }
        }
    }
}
