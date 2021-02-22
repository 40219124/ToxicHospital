using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerInteractor : MonoBehaviour
{
    public eInteractionRequirement CurrentCharacter = eInteractionRequirement.journalist;
    private List<BaseInteractable> interactables = new List<BaseInteractable>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if(interactables.Count == 0)
            {
                // Do nothing
            }
            else if (interactables.Count == 1)
            {
                TryInteract(interactables[0]);
            }
            else if(interactables.Count > 1)
            {
                float shortestSq = ((Vector2)transform.position - (Vector2)interactables[0].transform.position).sqrMagnitude;
                int shortestI = 0;
                for(int i = 1; i < interactables.Count; ++i)
                {
                    float currentSq = ((Vector2)transform.position - (Vector2)interactables[i].transform.position).sqrMagnitude;
                    if(currentSq < shortestSq)
                    {
                        shortestSq = currentSq;
                        shortestI = i;
                    }
                }

                TryInteract(interactables[shortestI]);
            }
        }
    }

    private bool TryInteract(BaseInteractable interactable)
    {
        if (interactable.CanInteract(CurrentCharacter))
        {
            interactable.TriggerInteraction(CurrentCharacter, transform);
            return true;
        }
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
