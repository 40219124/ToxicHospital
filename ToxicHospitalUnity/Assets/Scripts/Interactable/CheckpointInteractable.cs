using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointInteractable : BaseInteractable
{
    protected override void DoInteractionAction()
    {
        SavePayerData();
        SaveInventoryData();
        SaveInteractableData();
        SaveEnemyData();
    }

    private void SavePayerData()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        InfectionTracker healthEffects = player.GetComponent<InfectionTracker>();
        CheckpointStatus.PlayerInfectionLevel = healthEffects.InfectionProgress;
        CheckpointStatus.PlayerHealthPercentage = healthEffects.GetHealthPercentage();
        CheckpointStatus.PlayerTransform = player.transform;
    }

    private void SaveInventoryData()
    {
        CheckpointStatus.Recordings = Inventory.Instance.Recordings;
        CheckpointStatus.Letters = Inventory.Instance.Letters;
        CheckpointStatus.Reports = Inventory.Instance.Reports;
    }

    private void SaveInteractableData()
    {

    }

    private void SaveEnemyData()
    {
        //TODO enemies don't exist yet
    }
}

