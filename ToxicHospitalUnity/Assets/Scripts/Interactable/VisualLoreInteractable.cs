using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualLoreInteractable : LoreInteractable
{
    [SerializeField]
    protected VisualLore visualLore;

    protected override void SendLore()
    {
        VisualLoreCanvasManager.Instance.UpdateAndOpen(visualLore);
    }

    protected override void SendToInventory()
    {
        GameManager.AddToInventory.Invoke(visualLore);
    }
}