using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreInteractable : BaseInteractable
{
    protected bool interactedWith = false;
    protected ParticleSystem particles;

    protected override void Start()
    {
        base.Start();
        particles = GetComponentInChildren<ParticleSystem>();
    }
    protected override void DoInteractionAction()
    {
        SendLore();
        FirstInteraction();
    }

    protected virtual void SendLore()
    {
        Debug.LogWarning("Base Lore Class");
    }

    protected virtual void SendToInventory()
    {
        Debug.LogWarning("Base Lore Class");
    }

    protected virtual void FirstInteraction()
    {
        if (!interactedWith)
        {
            SendToInventory();
            interactedWith = true;
            particles.Stop();
        }
    }
}
