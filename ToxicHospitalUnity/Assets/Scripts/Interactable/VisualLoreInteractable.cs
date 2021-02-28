using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualLoreInteractable : BaseInteractable
{
    [SerializeField]
    protected VisualLore visualLore;

    protected bool interactedWith = false;
    protected ParticleSystem particles;

    protected override void Start()
    {
        base.Start();
        particles = GetComponentInChildren<ParticleSystem>();
    }
    protected override void DoInteractionAction()
    {
        VisualLoreCanvasManager.Instance.UpdateAndOpen(visualLore);
        if (!interactedWith)
        {
            interactedWith = true;
            particles.Stop();
        }
    }
}
