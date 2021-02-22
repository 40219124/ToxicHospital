using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualLoreInteractable : BaseInteractable
{
    [SerializeField]
    protected VisualLore visualLore;
    protected override void DoInteractionAction()
    {
        VisualLoreCanvasManager.Instance.UpdateAndOpen(visualLore);
    }
}
