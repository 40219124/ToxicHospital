using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointInteractable : BaseInteractable
{
    private PlayerController player;



    protected override void Start()
    {
        base.Start();


        if (!CheckpointStatus.Initialised)
        {
            //set up a reference to every movableInteractable in the scene before they get deactivated
            CheckpointStatus.AllInteractables = GameObject.FindObjectsOfType<BaseInteractable>();

            //get reference to all enemies
            CheckpointStatus.AllEnemies = GameObject.FindObjectsOfType<EnemyController>();


            CheckpointStatus.Initialised = true;
        }




        player = GameObject.FindObjectOfType<PlayerController>();
    }


    protected override void DoInteractionAction()
    {
        RestorePlayerHealthEffects();

        SavePayerData();
        SaveInventoryData();
        SaveInteractableData();
        SaveEnemyData();
        SaveEnemyData();

        // Logger.Log("Checkpoint activated");
        // Logger.Log(string.Format("Player Infection: {0}", CheckpointStatus.PlayerInfectionLevel));
        // Logger.Log(string.Format("Player Health(%): {0}", CheckpointStatus.PlayerHealthPercentage));
        // Logger.Log(string.Format("Player transform.position: {0}", CheckpointStatus.PlayerPosition));
        // Logger.Log(string.Format("Inventory Audio Logs: {0}", CheckpointStatus.Recordings));
        // Logger.Log(string.Format("Interactable list, element 0, name: {0}", CheckpointStatus.AllInteractables[0].name));
        // Logger.Log(string.Format("Interactable transfroms list, element 0, position: {0}", CheckpointStatus.AllInteractablesTransforms[0].position));
        // Logger.Log(string.Format("Interactable activity statuses list, element 0: {0}", CheckpointStatus.ActivyStatuses[0]));
    }

    private void RestorePlayerHealthEffects()
    {
        InfectionTracker healthEffects = player.gameObject.GetComponent<InfectionTracker>();
        healthEffects.InfectionProgress = 0;
        healthEffects.currentHealth = healthEffects.maxHealth;
    }

    private void SavePayerData()
    {

        InfectionTracker healthEffects = player.gameObject.GetComponent<InfectionTracker>();

        CheckpointStatus.PlayerInfectionLevel = healthEffects.InfectionProgress;
        CheckpointStatus.PlayerHealthPercentage = healthEffects.GetHealthPercentage();
        CheckpointStatus.PlayerPosition = player.transform.position;
        CheckpointStatus.PlayerRotation = player.transform.rotation;
    }

    private void SaveInventoryData()
    {
        CheckpointStatus.Recordings = new List<AudioLore>(Inventory.Instance.Recordings);
        CheckpointStatus.Letters = new List<VisualLore>(Inventory.Instance.Letters);
        CheckpointStatus.Reports = new List<VisualLore>(Inventory.Instance.Reports);
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
        CheckpointStatus.AllEnemyTransforms.Clear();
        CheckpointStatus.EnemiesFacingRight.Clear();

        foreach (EnemyController enemy in CheckpointStatus.AllEnemies)
        {
            CheckpointStatus.AllEnemyTransforms.Add(enemy.transform);
            CheckpointStatus.EnemiesFacingRight.Add(enemy.FacingRight);
        }

    }



    public static void LoadCheckpoint(InfectionTracker healthEffects)
    {
        healthEffects.InfectionProgress = CheckpointStatus.PlayerInfectionLevel;
        healthEffects.gameObject.transform.position = CheckpointStatus.PlayerPosition;
        healthEffects.gameObject.transform.rotation = CheckpointStatus.PlayerRotation;

        Inventory.Instance.Recordings = CheckpointStatus.Recordings;
        Inventory.Instance.Letters = CheckpointStatus.Letters;
        Inventory.Instance.Reports = CheckpointStatus.Reports;
        Debug.Log("Loaded Player");

        //load interactables' transforms and whether or not they are active
        for (int i = 0; i < CheckpointStatus.AllInteractables.Length; i++)
        {
            Logger.Log(string.Format("Player transform.position: {0}", CheckpointStatus.PlayerPosition));

            CheckpointStatus.AllInteractables[i].transform.position = CheckpointStatus.AllInteractablesTransforms[i].position;
            CheckpointStatus.AllInteractables[i].transform.rotation = CheckpointStatus.AllInteractablesTransforms[i].rotation;
            CheckpointStatus.AllInteractables[i].gameObject.SetActive(CheckpointStatus.ActivyStatuses[i]);

            Logger.Log("Loaded Interactibles");
        }

        //load all enemyies' transforms
        for (int i = 0; i < CheckpointStatus.AllEnemies.Length; i++)
        {
            CheckpointStatus.AllEnemies[i].transform.position = CheckpointStatus.AllEnemyTransforms[i].position;
            CheckpointStatus.AllEnemies[i].transform.rotation = CheckpointStatus.AllEnemyTransforms[i].rotation;
            CheckpointStatus.AllEnemies[i].transform.localScale = CheckpointStatus.AllEnemyTransforms[i].localScale;

            CheckpointStatus.AllEnemies[i].FacingRight = CheckpointStatus.EnemiesFacingRight[i];
            Debug.Log("Loaded Enemies");
        }

    }
}

