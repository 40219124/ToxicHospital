using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudibleLoreInteractable : LoreInteractable
{
    [SerializeField]
    protected AudioLore audioLore;

    protected override void SendLore()
    {
        RecordingPlayer.Instance.Activate(audioLore);
    }

    protected override void SendToInventory()
    {
        GameManager.AddToInventory.Invoke(audioLore);
    }
}