using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwapToMillieCheck : MonoBehaviour
{
    private List<int> colliders = new List<int>();

    public bool CanSwapToMillie
    {
        get { return colliders.Count == 0; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            AddCollider(collision.GetInstanceID());
        }
    }

    private void AddCollider(int id)
    {
        if (!colliders.Contains(id))
        {
            colliders.Add(id);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            RemoveCollider(collision.GetInstanceID());
        }
    }

    private void RemoveCollider(int id)
    {
        if (colliders.Contains(id))
        {
            colliders.Remove(id);
        }
    }

}
