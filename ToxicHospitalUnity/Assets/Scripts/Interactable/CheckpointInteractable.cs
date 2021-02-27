using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointInteractable : BaseInteractable
{

    protected override void Awake()
    {
        //set up a referecne to every movableInteractable in the scene before they get deactivated
        if (!CheckpointStatus.Initialised)
        {
            CheckpointStatus.AllInteractables = GameObject.FindObjectsOfType<BaseInteractable>();
            CheckpointStatus.Initialised = true;
        }
    }


    // protected override void Start()
    // {
    //     base.Start();

    //     //set up a referecne to every movableInteractable in the scene before they get deactivated
    //     if (!CheckpointStatus.Initialised)
    //     {
    //         CheckpointStatus.AllInteractables = GameObject.FindObjectsOfType<BaseInteractable>();
    //         CheckpointStatus.Initialised = true;
    //     }
    // }


    protected override void DoInteractionAction()
    {
        SavePayerData();
        SaveInventoryData();
        SaveInteractableData();
        SaveEnemyData();

        // Debug.Log("Checkpoint activated");
        // Debug.Log(string.Format("Player Infection: {0}", CheckpointStatus.PlayerInfectionLevel));
        // Debug.Log(string.Format("Player Health(%): {0}", CheckpointStatus.PlayerHealthPercentage));
        // Debug.Log(string.Format("Player transform.position: {0}", CheckpointStatus.PlayerTransform.position));
        // Debug.Log(string.Format("Inventory Audio Logs: {0}", CheckpointStatus.Recordings));
        // Debug.Log(string.Format("Interactable list, element 0, name: {0}", CheckpointStatus.AllInteractables[0].name));
        // Debug.Log(string.Format("Interactable transfroms list, element 0, position: {0}", CheckpointStatus.AllInteractablesTransforms[0].position));
        // Debug.Log(string.Format("Interactable activity statuses list, element 0: {0}", CheckpointStatus.ActivyStatuses[0]));
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

