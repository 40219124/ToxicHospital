using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointInteractable : BaseInteractable
{

    protected override void Start()
    {
        base.Start();

        //set up a referecne to every movableInteractable in the scene before they get deactivated
        if (!CheckpointStatus.Initialised)
        {
            CheckpointStatus.AllInteractables = GetComponents<BaseInteractable>();
            CheckpointStatus.Initialised = true;
        }
    }
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
        //clear lists of their previous content from otehr checkpoints
        CheckpointStatus.AllInteractablesTransforms.Clear();
        CheckpointStatus.ActivyStatuses.Clear();

        //save their transforms and whether or not they are active
        foreach (BaseInteractable interact in CheckpointStatus.AllInteractables)
        {
            CheckpointStatus.AllInteractablesTransforms.Add(interact.transform);
            CheckpointStatus.ActivyStatuses.Add(interact.isActiveAndEnabled);
        }

    }

    private void SaveEnemyData()
    {
        //TODO enemies don't exist yet
    }
}

